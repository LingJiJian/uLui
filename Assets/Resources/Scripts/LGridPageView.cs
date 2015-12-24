using UnityEngine;
using System.Collections.Generic;
using System.Security;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lui
{
    public class LGridPageViewPage : LTableViewCell
    {
        public List<LGridPageViewCell> gridCells { get; protected set; }
    }

    public class LGridPageViewCell : LTableViewCell
    {

    }

    /// <summary>
    /// 网格翻页
    /// </summary>
    public class LGridPageView : LTableView
    {
        public int gridCellsCount;
        public int cols;
        public int rows;

        public int pageIndex { get; protected set; }
        protected UnityAction<int> onPageChangedHandler;
        protected Vector2 gridCellsSize;
        protected int cellsMaxCountInPage;
        protected List<Vector2> gridCellsPosition;

        public LGridPageView()
        {
            this.gridCellsCount = 0;
            this.cols = 0;
            this.rows = 0;
            this.pageIndex = 0;
            this.gridCellsSize = Vector2.zero;
            autoRelocateSpeed = LTableView.AUTO_RELOCATE_SPPED;
            gridCellsPosition = new List<Vector2>();
        }

        public void updateGridCellsPosition()
        {
            float x = 0.0f;
            float y = GetComponent<RectTransform>().rect.height - gridCellsSize.y;

            for (int i = 0; i < cellsMaxCountInPage; ++i )
            {
                if ( i!= 0 && i % cols == 0)
                {
                    x = 0;
                    y = y - gridCellsSize.y;
                }
                gridCellsPosition.Add(new Vector2(x, y));
                x += gridCellsSize.x;
            }
        }

        public void updatePageCount()
        {
            cellsMaxCountInPage = cols * rows;
            if (gridCellsCount % cellsMaxCountInPage == 0)
            {
                cellsCount = gridCellsCount / cellsMaxCountInPage;
            }
            else
            {
                cellsCount = gridCellsCount / cellsMaxCountInPage + 1;
            }
        }

        public void reloadData()
        {
            updatePageCount();
            updateGridCellsPosition();
            base.reloadData();
        }

        protected override void onScrolling()
        {
            if (gridCellsCount == 0)
            {
                return;
            }

            base.onScrolling();
            Vector2 pageIdxOffset = default(Vector2);
            RectTransform rtran = GetComponent<RectTransform>();
            switch (direction)
            {
                case ScrollDirection.HORIZONTAL:
                    pageIdxOffset = getContentOffset() - new Vector2(rtran.rect.width / 2, 0);
                    break;
                case ScrollDirection.VERTICAL:
                    pageIdxOffset = getContentOffset() + new Vector2(0, rtran.rect.height / 2);
                    break;
                default:
                    break;
            }

            int page = cellBeginIndexFromOffset(pageIdxOffset);
            if (page != pageIndex)
            {
                pageIndex = page;
                if (onPageChangedHandler != null)
                {
                    onPageChangedHandler.Invoke(pageIndex);
                }
            }
        }

        public void updateCellAtIndex(int page)
        {
            LGridPageViewPage pageCell = (LGridPageViewPage)dequeueCell();
            if (pageCell == null)
            {
                pageCell = new LGridPageViewPage();

                List<LGridPageViewCell> gridCells = pageCell.gridCells;
                int beginIdx = page * cellsMaxCountInPage;
            }
        }

        public void setPageChangedHandler(UnityAction<int> action)
        {
            onPageChangedHandler = action;
        }
    }
}

/****************************************************************************
Copyright (c) 2015 Lingjijian

Created by Lingjijian on 2015

342854406@qq.com


Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Security;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SLua;

namespace Lui
{
    public class LGridPageViewPage : LTableViewCell
    {
        public List<LGridPageViewCell> gridCells { get; protected set; }
        public LGridPageViewPage()
        {
            gridCells = new List<LGridPageViewCell>();
            node = new GameObject();
        }
    }

    public class LGridPageViewCell : LTableViewCell
    {

    }

    /// <summary>
    /// 网格翻页
    /// </summary>
    [CustomLuaClassAttribute]
    public class LGridPageView : LTableView
    {
        public int gridCellsCount;
        public int cols;
        public int rows;

        public int pageIndex { get; protected set; }
        public UnityAction<int> onPageChangedHandler;
        public Vector2 gridCellsSize;
        protected int _cellsMaxCountInPage;
        protected List<Vector2> _gridCellsPosition;
        public void SetCellHandle(UnityAction<int, GameObject> act)
        {
            onCellHandle = act;
        }

        public LGridPageView()
        {
            this.gridCellsCount = 0;
            this.cols = 0;
            this.rows = 0;
            this.pageIndex = 0;
            this.gridCellsSize = Vector2.zero;
            autoRelocateSpeed = LTableView.AUTO_RELOCATE_SPPED;
            _gridCellsPosition = new List<Vector2>();
        }

        public void updateGridCellsPosition()
        {
            float x = 0.0f;
            float y = GetComponent<RectTransform>().rect.height - gridCellsSize.y;

            for (int i = 0; i < _cellsMaxCountInPage; ++i )
            {
                if ( i!= 0 && i % cols == 0)
                {
                    x = 0;
                    y = y - gridCellsSize.y;
                }
                _gridCellsPosition.Add(new Vector2(x, y));
                x += gridCellsSize.x;
            }
        }

        public void updatePageCount()
        {
            _cellsMaxCountInPage = cols * rows;
            if (gridCellsCount % _cellsMaxCountInPage == 0)
            {
                cellsCount = gridCellsCount / _cellsMaxCountInPage;
            }
            else
            {
                cellsCount = gridCellsCount / _cellsMaxCountInPage + 1;
            }
        }

        public override void reloadData()
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

        public override void updateCellAtIndex(int page)
        {
            LGridPageViewPage pageCell = (LGridPageViewPage)dequeueCell();
            if (pageCell == null)
            {
                pageCell = new LGridPageViewPage();

                List<LGridPageViewCell> gridCells = pageCell.gridCells;
                int beginIdx = page * _cellsMaxCountInPage;
                int endIdx = beginIdx + _cellsMaxCountInPage;

                for (int idx = beginIdx, i = 0; idx < endIdx;++idx,++i )
                {
                    LGridPageViewCell cell = null;
                    if (idx < gridCellsCount)
                    {
                        cell = _onGridDataSourceAdapterHandler(null, idx);
                        RectTransform rtran = cell.node.GetComponent<RectTransform>();
                        rtran.pivot = Vector2.zero;
                        rtran.sizeDelta = gridCellsSize;
                        cell.idx = idx;
                        cell.node.SetActive(true);
                        cell.node.transform.SetParent(pageCell.node.transform);
                        cell.node.transform.localPosition = _gridCellsPosition[i];
                        cell.node.transform.localScale = new Vector2(1, 1);
                        gridCells.Add(cell);
                    }
                    else
                    {
                        cell = _onGridDataSourceAdapterHandler(null, INVALID_INDEX);
                        RectTransform rtran = cell.node.GetComponent<RectTransform>();
                        rtran.pivot = Vector2.zero;
                        cell.idx = INVALID_INDEX;
                        cell.node.SetActive(false);
                        cell.node.transform.SetParent(pageCell.node.transform);
                        cell.node.transform.localPosition = _gridCellsPosition[i];
                        cell.node.transform.localScale = new Vector2(1, 1);
                        gridCells.Add(cell);
                    }
                }
            }
            else
            {
                List<LGridPageViewCell> gridCells = pageCell.gridCells;
                int beginIdx = page * _cellsMaxCountInPage;
                int endIdx = beginIdx + _cellsMaxCountInPage;

                for (int idx = beginIdx, i = 0; idx < endIdx; ++idx, ++i)
                {
                    LGridPageViewCell cell = gridCells[i];
                    if (idx < gridCellsCount)
                    {
                        cell.idx = idx;
                        cell = _onGridDataSourceAdapterHandler(cell, idx);
                        cell.node.SetActive(true);
                    }
                    else
                    {
                        cell.idx = INVALID_INDEX;
                        cell.reset();
                        cell = _onGridDataSourceAdapterHandler(cell, INVALID_INDEX);
                        cell.node.SetActive(false);
                    }
                }
            }

            pageCell.idx = page;
            RectTransform tran = pageCell.node.GetComponent<RectTransform>();
            switch(direction)
            {
                case ScrollDirection.HORIZONTAL:
                    tran.pivot = Vector2.zero;
                    break;
                default:
                    tran.pivot = new Vector2(0, 1);
                    break;
            }

            tran.sizeDelta = cellsSize;
            pageCell.node.SetActive(true);
            pageCell.node.transform.SetParent(container.transform);
            pageCell.node.transform.localScale = new Vector2(1, 1);
            pageCell.node.transform.localPosition = cellPositionFromIndex(page);
            insertSortableCell(pageCell, page);
            indices.Add(page, 1);
        }

        protected LGridPageViewCell _onGridDataSourceAdapterHandler(LGridPageViewCell cell, int idx)
        {
            if (cell == null)
            {
                cell = new LGridPageViewCell();
                cell.node = (GameObject)Instantiate(transform.Find("container/cell_tpl").gameObject);
            }
            cell.node.SetActive(idx != LScrollView.INVALID_INDEX);

            if(onCellHandle != null)
            {
                onCellHandle.Invoke(idx, cell.node);
            }
            return cell;
        }

        public void setPageChangedHandler(UnityAction<int> action)
        {
            onPageChangedHandler = action;
        }
    }
}

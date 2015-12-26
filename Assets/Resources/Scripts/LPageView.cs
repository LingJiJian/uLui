/****************************************************************************
Copyright (c) 2015 Lingjijian

Created by Lingjijian on 2015

342854406@qq.com
http://www.cocos2d-x.org

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

namespace Lui
{
    /// <summary>
    /// 翻页
    /// </summary>
    public class LPageView : LTableView
    {
        public int pageIndex { get; protected set; }
        protected UnityAction<int> onPageChangedHandler;

        public LPageView()
        {
            autoRelocate = true;
            autoRelocateSpeed = 900;
        }

        protected override void onScrolling()
        {
            if (cellsCount == 0)
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

        public override void updateCellAtIndex(int idx)
        {
            LTableViewCell cell = onDataSourceAdapterHandler.Invoke(dequeueCell(), idx);
            if (cell == null)
            {
                Debug.LogError("cell can not be NULL");
            }
            cell.idx = idx;

            RectTransform rtran = cell.node.GetComponent<RectTransform>();
            switch(direction)
            {
                case ScrollDirection.HORIZONTAL:
                    rtran.pivot = new Vector2(0,0);
                    break;
                default:
                    rtran.pivot = new Vector2(0,1);
                    break;
            }

            rtran.sizeDelta = cellsSize;
            cell.node.transform.SetParent(container.transform);
            cell.node.transform.localPosition = cellPositionFromIndex(idx);
            insertSortableCell(cell,idx);
            indices.Add(idx,1);
        }

        public void setPageChangedHandler(UnityAction<int> action)
        {
            onPageChangedHandler = action;
        }

		private LTableViewCell dataSourceAdaptTest(LTableViewCell cell, int idx)
		{
			if (cell == null)
			{
				cell = new LTableViewCell();
				cell.node = (GameObject)Instantiate(this.cellTemplate.node, Vector3.zero, cellTemplate.node.transform.rotation);
			}
			cell.node.transform.FindChild("Text").GetComponent<Text>().text = idx.ToString();
			return cell;
		}
		
		void Start()
		{
			this.cellsSize = new Vector2(150, 100);
			this.cellTemplate.node = Resources.Load("Prefabs/page_cell") as GameObject;

			this.cellsCount = 14;
			this.setDataSourceAdapterHandler(dataSourceAdaptTest);
			this.reloadData();
		}
    }
}

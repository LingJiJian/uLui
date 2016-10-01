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
    public class LTableViewCell
    {
        public int idx;
        protected GameObject _node;
        public GameObject node
        {
            get { return _node; }
            set { 
                _node = value;
                RectTransform rtran = _node.GetComponent<RectTransform>();
                if (rtran == null)
                {
                    rtran = _node.AddComponent<RectTransform>();
                }
                rtran.pivot = Vector2.zero;
                rtran.anchorMax = Vector2.zero;
                rtran.anchorMin = Vector2.zero;
            }
        }
        public void reset()
        {
            idx = LScrollView.INVALID_INDEX;
        }
    }

    /// <summary>
    /// 复用列表
    /// </summary>
    [CustomLuaClassAttribute]
    public class LTableView : LScrollView
    {
        public int cellsCount;
        public Vector2 cellsSize;
        public bool autoRelocate;
        
        protected List<LTableViewCell> cellsUsed;
        protected List<LTableViewCell> cellsFreed;
        protected List<float> positions;
        protected Dictionary<int, int> indices;
        protected UnityAction<int,GameObject> onCellHandle;
        public void SetCellHandle(UnityAction<int, GameObject> act)
        {
            onCellHandle = act;
        }

        public LTableView()
        {
            cellsCount = 0;
            cellsSize = Vector2.zero;
            direction = ScrollDirection.HORIZONTAL;
            autoRelocate = false;

            cellsUsed = new List<LTableViewCell>();
            cellsFreed = new List<LTableViewCell>();
            positions = new List<float>();
            indices = new Dictionary<int, int>();
        }

        public virtual void reloadData()
        {
            for (int i = 0; i < cellsUsed.Count;i++ )
            {
                LTableViewCell cell = cellsUsed[i];
                cellsFreed.Add(cell);
                //cell.node.transform.SetParent(null);
                cell.node.SetActive(false);
                cell.reset();
            }

            cellsUsed.Clear();
            indices.Clear();
            positions.Clear();
            updatePositions();
            setContentOffsetToTop();
            onScrolling();
            relocateContainer();
        }

        public void removeAllFromUsed()
        {
            foreach (LTableViewCell cell in cellsUsed)
            {
                Destroy(cell.node);
            }
            cellsUsed.Clear();
        }

        public void removeAllFromFreed()
        {
            foreach (LTableViewCell cell in cellsFreed)
            {
                Destroy(cell.node);
            }
            cellsFreed.Clear();
        }

        protected override void onScrolling()
        {
            if (cellsCount == 0)
            {
                return;
            }

            int beginIdx = 0, endIdx = 0;
            beginIdx = cellBeginIndexFromOffset(getContentOffset());
            endIdx = cellEndIndexFromOffset(getContentOffset());

            while (cellsUsed.Count > 0)
            {
                LTableViewCell cell = cellsUsed[0];
                int idx = cell.idx;

                if (idx < beginIdx)
                {
                    indices.Remove(idx);
                    cellsUsed.Remove(cell);
                    cellsFreed.Add(cell);
                    cell.reset();
                    
                    cell.node.SetActive(false);
                }
                else
                {
                    break;
                }
            }

            while (cellsUsed.Count > 0)
            {
                LTableViewCell cell = cellsUsed[cellsUsed.Count - 1];
                int idx = cell.idx;

                if (idx > endIdx && idx < cellsCount)
                {
                    indices.Remove(idx);
                    cellsUsed.RemoveAt(cellsUsed.Count - 1);
                    cellsFreed.Add(cell);
                    cell.reset();
                    cell.node.SetActive(false);
                }
                else
                {
                    break;
                }
            }

            for (int idx = beginIdx; idx <= endIdx && idx < cellsCount; ++idx)
            {
                if (indices.ContainsKey(idx))
                {
                    continue;
                }
                updateCellAtIndex(idx);
            }

            base.onScrolling();
        }

        protected void updatePositions()
        {
            if (direction == ScrollDirection.HORIZONTAL)
            {
                setContainerSize(new Vector2(cellsSize.x * cellsCount, cellsSize.y));
                for (int i = 0; i < cellsCount;i++ )
                {
                    positions.Add(cellsSize.x * i);
                }
            }
            else
            {
                float height = cellsSize.y * cellsCount;
                setContainerSize(new Vector2(cellsSize.x, height));
                height = Mathf.Max(height, GetComponent<RectTransform>().rect.height);
                for (int i = cellsCount - 1; i >= 0;--i )
                {
                    positions.Add(height);
                    height -= cellsSize.y;
                }
            }
        }

        protected override void onDraggingScrollEnded()
        {
            if (cellsCount == 0)
            {
                return;
            }

            if (autoRelocate)
            {
                Vector2 offset = getContentOffset();
                int idx = cellBeginIndexFromOffset(offset);
                Vector2 pointA = cellPositionFromIndex(idx);

                if (direction == ScrollDirection.HORIZONTAL)
                {
                    Vector2 pointB = new Vector2(pointA.x + cellsSize.x, 0);
                    float distanceA = Vector2.Distance(offset, -pointA);
                    float distanceB = Vector2.Distance(offset, -pointB);

                    if (distanceA < distanceB)
                    {
                        float duration = Mathf.Abs(distanceA) / autoRelocateSpeed;
                        setContentOffsetInDuration(-pointA, duration);
                    }
                    else
                    {
                        float duration = Mathf.Abs(distanceB) / autoRelocateSpeed;
                        setContentOffsetInDuration(-pointB, duration);
                    }
                }
                else
                {
                    Vector2 pointB = new Vector2(0, pointA.y - cellsSize.y);
                    Vector2 contentPoint = new Vector2(0, GetComponent<RectTransform>().rect.height);
                    offset = offset - contentPoint;
                    float distanceA = Vector2.Distance(offset, -pointA);
                    float distanceB = Vector2.Distance(offset, -pointB);

                    if (distanceA < distanceB)
                    {
                        float duration = Mathf.Abs(distanceA) / autoRelocateSpeed;
                        setContentOffsetInDuration(-pointA + contentPoint, duration);
                    }
                    else
                    {
                        float duration = Mathf.Abs(distanceB) / autoRelocateSpeed;
                        setContentOffsetInDuration(-pointB + contentPoint, duration);
                    }
                }
            }

            base.onDraggingScrollEnded();
        }

        protected LTableViewCell dequeueCell()
        {
            LTableViewCell cell = null;
            if (cellsFreed.Count == 0)
            {
                return null;
            }
            else
            {
                cell = cellsFreed[cellsFreed.Count - 1];
                cellsFreed.Remove(cell);
            }
            return cell;
        }

        protected int cellBeginIndexFromOffset(Vector2 offset)
        {
            if (cellsCount == 0)
            {
                return LScrollView.INVALID_INDEX;
            }

            switch (direction)
            {
                case ScrollDirection.HORIZONTAL:
                    {
                        float xos = -offset.x;
                        int idx = (int)(xos / cellsSize.x);

                        idx = Mathf.Max(idx, 0);
                        idx = Mathf.Min((int)cellsCount-1,idx);
                        return (int)idx;
                    }
                default:
                    {
                        float ofy = offset.y + container.GetComponent<RectTransform>().rect.height;
                        float xos = ofy - GetComponent<RectTransform>().rect.height;
                        int idx = (int)(xos / cellsSize.y);

                        idx = Mathf.Max(idx, 0);
                        idx = Mathf.Min((int)cellsCount - 1, idx);
                        return (int)idx;
                    }
            }
        }

        protected int cellEndIndexFromOffset(Vector2 offset)
        {
            if (cellsCount == 0)
            {
                return LScrollView.INVALID_INDEX;
            }

            switch (direction)
            {
                case ScrollDirection.HORIZONTAL:
                    {
                        float xos = -(offset.x + -GetComponent<RectTransform>().rect.width);
                        int idx = (int)(xos / cellsSize.x);

                        idx = Mathf.Max(idx, 0);
                        idx = Mathf.Min((int)cellsCount - 1, idx);

                        return (int)idx;
                    }
                default:
                    {
                        float ofy = offset.y + container.GetComponent<RectTransform>().rect.height;
                        int idx = (int)(ofy / cellsSize.y);

                        idx = Mathf.Max(idx, 0);
                        idx = Mathf.Min((int)cellsCount - 1, idx);

                        return (int)idx;
                    }
            }
        }

        protected Vector2 cellPositionFromIndex(int idx)
        {
            if (idx == LScrollView.INVALID_INDEX)
            {
                return Vector2.zero;
            }
            switch (direction)
            {
                case ScrollDirection.HORIZONTAL:
                    {
                        return new Vector2(positions[idx], 0);
                    }
                default:
                    {
                        return new Vector2(0, positions[idx]);
                    }
            }
        }

        protected void insertSortableCell(LTableViewCell cell, int idx)
        {
            if (cellsUsed.Count == 0)
            {
                cellsUsed.Add(cell);
            }
            else
            {
                for (int i = 0; i < cellsUsed.Count; i++ )
                {
                    if (cellsUsed[i].idx > idx)
                    {
                        cellsUsed.Insert(i, cell);
                        return;
                    }
                }
                cellsUsed.Add(cell);
            }
        }

        public LTableViewCell cellAtIndex(int idx)
        {
            if (!indices.ContainsKey(idx))
            {
                return null;
            }
            for (int i = 0; i < cellsUsed.Count; i++)
            {
                if (cellsUsed[i].idx == idx)
                {
                    return cellsUsed[i];
                }
            }
            return null;
        }

        public virtual void updateCellAtIndex(int idx)
        {
            LTableViewCell cell = _onDataSourceAdapterHandler(dequeueCell(), idx);
            if (cell == null)
            {
                Debug.LogError("cell can not be NULL");
            }
            cell.idx = idx;
            RectTransform rtran = cell.node.GetComponent<RectTransform>();
            switch (direction)
            {
                case ScrollDirection.HORIZONTAL:
                    rtran.pivot = Vector2.zero;
                    break;
                default:
                    rtran.pivot = new Vector2(0, 1);
                    break;
            }

            rtran.sizeDelta = cellsSize;
            cell.node.SetActive(true);
            cell.node.transform.SetParent(container.transform);
            cell.node.transform.localScale = new Vector2(1,1);
            cell.node.transform.localPosition = cellPositionFromIndex(idx);

            insertSortableCell(cell, idx);
            indices.Add(idx, 1);
        }

        protected LTableViewCell _onDataSourceAdapterHandler(LTableViewCell cell, int idx)
        {
            if (cell == null)
            {
                cell = new LTableViewCell();
                cell.node = (GameObject)Instantiate(transform.Find("container/cell_tpl").gameObject);
            }
            if(onCellHandle != null)
            {
                onCellHandle.Invoke(idx, cell.node);
            }
            return cell;
        }
    }
}
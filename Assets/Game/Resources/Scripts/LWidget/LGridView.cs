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
    public class LGridViewCell : LTableViewCell
    {
        public int row;
    }
        
    /// <summary>
    /// 网格
    /// </summary>
    [CustomLuaClassAttribute]
    public class LGridView : LScrollView
    {
        public Vector2 cellsSize;
        public int cellsCount;
        public int cols;
        public bool autoRelocate;

        protected int _rows;
        protected List<LGridViewCell> _cellsUsed;
        protected List<LGridViewCell> _cellsFreed;
        protected List<Vector2> _positions;
        protected Dictionary<int, int> _indices;
        protected UnityAction<int, GameObject> onCellHandle;
        public void SetCellHandle(UnityAction<int, GameObject> act)
        {
            onCellHandle = act;
        }

        public LGridView()
        {
            cellsCount = 0;
            cellsSize = Vector2.zero;
            cols = 0;
            _rows = 0;
            direction = ScrollDirection.VERTICAL;
            _cellsUsed = new List<LGridViewCell>();
            _cellsFreed = new List<LGridViewCell>();
            _positions = new List<Vector2>();
            _indices = new Dictionary<int, int>();
        }

        public void removeAllFromUsed()
        {
            foreach (LGridViewCell cell in _cellsUsed)
            {
                Destroy(cell.node);
            }
            _cellsUsed.Clear();
        }

        public void removeAllFromFreed()
        {
            foreach (LGridViewCell cell in _cellsFreed)
            {
                Destroy(cell.node);
            }
            _cellsFreed.Clear();
        }

        public void insertSortableCell(LGridViewCell cell, int idx)
        {
            if (_cellsUsed.Count == 0)
            {
                _cellsUsed.Add(cell);
            }
            else
            {
                for (int i = 0; i < _cellsUsed.Count; i++)
                {
                    if (_cellsUsed[i].idx > idx)
                    {
                        _cellsUsed.Insert(i, cell);
                        return;
                    }
                }
                _cellsUsed.Add(cell);
                return;
            }
        }

        protected Vector2 cellPositionFromIndex(int idx)
        {
            if (idx == LScrollView.INVALID_INDEX)
            {
                return _positions[0];
            }
            return _positions[idx];
        }

        public void updateCellAtIndex(int idx, int row)
        {
            if (cellsCount == 0)
            {
                return;
            }

            LGridViewCell cell = _onDataSourceAdapterHandler(dequeueCell(), idx);
            cell.idx = idx;
            cell.row = row;
            RectTransform rtran = cell.node.GetComponent<RectTransform>();
            rtran.pivot = new Vector2(0, 1);
            rtran.sizeDelta = cellsSize;
            cell.node.SetActive(true);
            cell.node.transform.SetParent(container.transform);
            cell.node.transform.localPosition = cellPositionFromIndex(idx);
            cell.node.transform.localScale = new Vector2(1, 1);
            insertSortableCell(cell, idx);

            _indices.Add(idx, 1);
        }

        protected int cellBeginRowFromOffset(Vector2 offset)
        {
            float ofy = offset.y + container.GetComponent<RectTransform>().rect.height;
            float xos = ofy - GetComponent<RectTransform>().rect.height;
            int row = (int)(xos / cellsSize.y);

            row = Mathf.Max(row, 0);
            row = Mathf.Min((int)_rows - 1, row);

            return (int)row;
        }

        protected int cellEndRowFromOffset(Vector2 offset)
        {
            float ofy = offset.y + container.GetComponent<RectTransform>().rect.height;
            int row = (int)(ofy / cellsSize.y);

            row = Mathf.Max(row, 0);
            row = Mathf.Min((int)_rows - 1, row);

            return (int)row;
        }

        protected int cellFirstIndexFromRow(int row)
        {
            return cols * row;
        }

        protected void updatePositions()
        {
            if (cellsCount == 0)
            {
                return;
            }
            RectTransform rtran = GetComponent<RectTransform>();

            _rows = cellsCount % cols == 0 ? cellsCount / cols : cellsCount / cols + 1;
            float height = Mathf.Max(_rows * cellsSize.y, rtran.rect.height);
            float width = cols * cellsSize.x;
            setContainerSize(new Vector2(width, height));

            float nx = 0.0f;
            float ny = height;

            for (int idx = 0; idx < cellsCount; ++idx)
            {
                if (idx != 0 && idx % cols == 0)
                {
                    nx = 0.0f;
                    ny = ny - cellsSize.y;
                }
                _positions.Add(new Vector2(nx, ny));
                nx += cellsSize.x;
            }
        }

        public List<LGridViewCell> getCells()
        {
            List<LGridViewCell> ret = new List<LGridViewCell>();
            for (int i = 0; i < _cellsUsed.Count; i++)
            {
                ret.Add(_cellsUsed[i]);
            }
            return ret;
        }

        public LGridViewCell cellAtIndex(int idx)
        {
            if (!_indices.ContainsKey(idx))
            {
                return null;
            }
            for (int i = 0; i < _cellsUsed.Count; i++)
            {
                if (_cellsUsed[i].idx == idx)
                {
                    return _cellsUsed[i];
                }
            }
            return null;
        }

        protected LGridViewCell dequeueCell()
        {
            LGridViewCell cell = null;
            if (_cellsFreed.Count == 0)
            {
                return null;
            }
            else
            {
                cell = _cellsFreed[0];
                _cellsFreed.Remove(cell);
            }
            return cell;
        }

        public void reloadData()
        {
			if (_cellsUsed.Count > 0) 
			{
				LGridViewCell cell = _cellsUsed [0];
				while (cell != null) {
					_cellsFreed.Add (cell);
					_cellsUsed.Remove (cell);
                    //cell.node.transform.SetParent (null);
                    cell.reset ();
                    cell.node.SetActive(false);
                }
			}
            _indices.Clear();
            _positions.Clear();
            updatePositions();
            setContentOffsetToTop();
            onScrolling();

            relocateContainer();
        }

        protected override void onScrolling()
        {
			base.onScrolling ();

            int beginRow = 0, endRow = 0;
            beginRow = cellBeginRowFromOffset(getContentOffset());
            endRow = cellEndRowFromOffset(getContentOffset());

            while (_cellsUsed.Count > 0)
            {
                LGridViewCell cell = _cellsUsed[0];
                int row = cell.row;
                int idx = cell.idx;

                if (row < beginRow)
                {
                    _indices.Remove(idx);
                    _cellsUsed.Remove(cell);
                    _cellsFreed.Add(cell);
                    cell.reset();
                    //cell.node.transform.SetParent(null);
                    cell.node.SetActive(false);
                }
                else
                {
                    break;
                }
            }

            while (_cellsUsed.Count > 0)
            {
                LGridViewCell cell = _cellsUsed[_cellsUsed.Count - 1];
                int row = cell.row;
                int idx = cell.idx;

                if (row > endRow && row < _rows)
                {
                    _indices.Remove(idx);
                    _cellsUsed.Remove(cell);
					_cellsFreed.Add(cell);
                    cell.reset();
                    //cell.node.transform.SetParent(null);
                    cell.node.SetActive(false);
                }
                else
                {
                    break;
                }
            }

            for (int row = beginRow; row <= endRow && row < _rows; ++row)
            {
                int cellBeginIndex = cellFirstIndexFromRow(row);
                int cellEndIndex = cellBeginIndex + cols;

                for (int idx = cellBeginIndex; idx < cellEndIndex && idx < cellsCount; ++idx)
                {
                    if (_indices.ContainsKey(idx))
                    {
                        continue;
                    }
                    updateCellAtIndex(idx, row);
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
                int row = cellBeginRowFromOffset(offset);
                Vector2 pointA = cellPositionFromIndex(cellFirstIndexFromRow(row));
                Vector2 pointB = new Vector2(0, pointA.y - cellsSize.y);
                Vector2 contentPoint = new Vector2(0, GetComponent<RectTransform>().rect.height);
                offset = offset - contentPoint;
                pointA.x = 0;

                float distanceA = Vector2.Distance(offset, -pointA);
                float distanceB = Vector2.Distance(offset, -pointB);

                if (distanceA < distanceB)
                {
                    float duration = Mathf.Abs(distanceA) / LTableView.AUTO_RELOCATE_SPPED;
                    setContentOffsetInDuration(-pointA + contentPoint, duration);
                }
                else
                {
                    float duration = Mathf.Abs(distanceB) / LTableView.AUTO_RELOCATE_SPPED;
                    setContentOffsetInDuration(-pointB + contentPoint, duration);
                }
            }

            base.onDraggingScrollEnded();
        }

        protected LGridViewCell _onDataSourceAdapterHandler(LGridViewCell cell, int idx)
        {
            if (cell == null)
            {
                cell = new LGridViewCell();
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
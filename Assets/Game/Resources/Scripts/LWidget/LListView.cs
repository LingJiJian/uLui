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
    /// <summary>
    /// 列表
    /// </summary>
    [CustomLuaClassAttribute]
    public class LListView : LScrollView
    {
        public static Vector2 HorizontalNodeAnchorPoint = Vector2.zero;
        public static Vector2 VerticalNodeAnchorPoint = Vector2.zero;

        public int limitNum;
        protected float _layoutIndexSize;
        public List<GameObject> nodeList { get; protected set; }
        public List<GameObject> freeList { get; protected set; }

        public LListView()
        {
            this.limitNum = 0;
            this._layoutIndexSize = 0;
            this.direction = ScrollDirection.VERTICAL;
            this.nodeList = new List<GameObject>();
            this.freeList = new List<GameObject>();
        }

        public GameObject getNodeAtIndex(int idx)
        {
            return nodeList[idx];
        }

        public void insertNodeAtLast(GameObject node)
        {
            if (node != null)
            {
                nodeList.Add(node);
            }
            else
            {
                Debug.Log("LListView::insertNodeAtLast node is null");
            }
            if (limitNum > 0)
            {
                checkRecycleItem();
                reloadData();
            }
        }

        public void insertNodeAtFront(GameObject node)
        {
            if (node != null)
            {
                nodeList.Insert(0,node);
            }
            else
            {
                Debug.Log("LListView::insertNodeAtFront node is null");
            }
            if (limitNum > 0)
            {
                checkRecycleItem();
            }
        }

        public void insertNode(GameObject node,int idx)
        {
            if (idx >= nodeList.Count)
            {
                insertNodeAtLast(node);
                return;
            }
            nodeList.Insert(idx, node);
            if (limitNum > 0)
            {
                checkRecycleItem();
                reloadData();
            }
        }

        public void removeNodeAtIndex(int idx)
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            nodeList.RemoveAt(idx);
        }

        public void removeNode(GameObject node)
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            nodeList.Remove(node);
        }

        public void removeFrontNode()
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            nodeList.RemoveAt(0);
        }

        public void removeLastNode()
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            nodeList.RemoveAt(nodeList.Count - 1);
        }

        public void removeAllNodes()
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            nodeList.Clear();
        }

        protected void updateNodesPosition()
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            _layoutIndexSize = 0;
            switch (direction)
            {
                case ScrollDirection.HORIZONTAL:
                    {
                        GameObject obj = null;
                        for (int i = 0; i < nodeList.Count;i++ )
                        {
                            obj = nodeList[i];
                            obj.GetComponent<RectTransform>().pivot = HorizontalNodeAnchorPoint;
                            obj.transform.localPosition = new Vector2(_layoutIndexSize, 0);
                            _layoutIndexSize += obj.GetComponent<RectTransform>().rect.width;
                        }
                    }
                    break;
                case ScrollDirection.VERTICAL:
                    {
                        float allNodesSize = 0;
                        GameObject obj = null;
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            obj = nodeList[i];
                            allNodesSize += obj.GetComponent<RectTransform>().rect.height;
                        }

                        _layoutIndexSize = allNodesSize;
                        RectTransform rtran = GetComponent<RectTransform>();
                        allNodesSize = Mathf.Max(rtran.rect.height, allNodesSize);
                        setContainerSize(new Vector2(rtran.rect.width, allNodesSize));

                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            obj = nodeList[i];
                            allNodesSize -= obj.GetComponent<RectTransform>().rect.height;
                            obj.GetComponent<RectTransform>().pivot = VerticalNodeAnchorPoint;
                            obj.transform.SetParent(container.transform);
                            obj.transform.localScale = new Vector2(1, 1);
                            obj.transform.localPosition = new Vector2(0, allNodesSize);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void reloadData()
        {
            if (direction == ScrollDirection.VERTICAL)
            {
                RectTransform rtran = GetComponent<RectTransform>();
                float oldHeight = rtran.rect.height;
                updateNodesPosition();
                float newHeight = rtran.rect.height - oldHeight;
                setContentOffset(getContentOffset() - new Vector2(0, newHeight));
            }
            else
            {
                updateNodesPosition();
            }

            relocateContainer();
        }

        protected void checkRecycleItem()
        {
            if (limitNum > 0)
            {
                if (nodeList.Count >= limitNum)
                {
                    GameObject obj = null;
                    for (int i = 0; i < nodeList.Count - limitNum; i++)
                    {
                        obj = nodeList[i];
                        nodeList.Remove(obj);
                        freeList.Add(obj);
                        obj.SetActive(false);
                    }
                }
            }
        }

        public GameObject dequeueItem()
        {
            GameObject ret = null;
            if (limitNum > 0)
            {
                if (freeList.Count > 0)
                {
                    ret = freeList[0];
                    ret.SetActive(true);
                    freeList.RemoveAt(0);
                }
            }
            if (ret == null)
            {
                ret = (GameObject)Instantiate(transform.Find("container/cell_tpl").gameObject);
            }
            return ret;
        }
    }
}

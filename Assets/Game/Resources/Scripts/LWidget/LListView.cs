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

namespace Lui
{
    /// <summary>
    /// 列表项
    /// </summary>
    public class LListNode
    {
        public GameObject obj;
        public int tpl_id;
        public LListNode(GameObject obj, int tpl_id)
        {
            this.obj = obj;
            this.tpl_id = tpl_id;
        }
    }


    /// <summary>
    /// 列表
    /// </summary>
    [SLua.CustomLuaClass]
    public class LListView : LScrollView
    {
        public static Vector2 HorizontalNodeAnchorPoint = Vector2.zero;
        public static Vector2 VerticalNodeAnchorPoint = Vector2.zero;

        public int limitNum;
        protected float _layoutIndexSize;
        public List<LListNode> nodeList { get; protected set; }
        public Dictionary<int, List<LListNode>> freeDic { get; protected set; }

        public LListView()
        {
            this.limitNum = 0;
            this._layoutIndexSize = 0;
            this.direction = ScrollDirection.VERTICAL;
            this.nodeList = new List<LListNode>();
            this.freeDic = new Dictionary<int, List<LListNode>>();
        }

        public GameObject getNodeAtIndex(int idx)
        {
			for (int i = 0; i < nodeList.Count; i++) {
				if (i == idx) {
					return nodeList [i].obj;
				}
			}
            return null;
        }

        public int getIndexByObject(GameObject obj)
        {
            for (int i = 0; i < nodeList.Count; i++) {
                if (obj == nodeList [i].obj) {
                    return i;
                }
            }
            return -1;
        }

        public void insertNodeAtLast(GameObject node, int tpl_id)
        {
            if (node != null)
            {
                nodeList.Add(new LListNode(node,tpl_id));
            }
            else
            {
                Debug.Log("LListView::insertNodeAtLast node is null");
            }
            if (limitNum > 0)
            {
                checkRecycleItem();
            }
        }

        public void insertNodeAtFront(GameObject node, int tpl_id)
        {
            if (node != null)
            {
                nodeList.Insert(0, new LListNode(node, tpl_id));
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

        public void insertNode(GameObject node, int tpl_id,int idx)
        {
            if (idx >= nodeList.Count)
            {
                insertNodeAtLast(node, tpl_id);
                return;
            }
            nodeList.Insert(idx, new LListNode(node, tpl_id));
            if (limitNum > 0)
            {
                checkRecycleItem();
            }
        }

        public void removeNodeAtIndex(int idx)
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            if(limitNum > 0)
            {
                LListNode node = nodeList[idx];
                pushFreePool(node);
            }else
            {
                Object.Destroy(nodeList[idx].obj);
            }
            nodeList.RemoveAt(idx);
        }

        public void removeNode(GameObject node)
        {
            if (nodeList.Count == 0)
            {
                return;
            }

            LListNode del = null;
            foreach(LListNode elem in nodeList)
            {
                if(elem.obj == node)
                {
                    del = elem;
                    nodeList.Remove(elem);
                    break;
                }
            }
            if(limitNum > 0)
            {
                pushFreePool(del);
            }
            else
            {
                Object.Destroy(node);
            }
        }

        public void removeFrontNode()
        {
            if (nodeList.Count == 0)
            {
                return;
            }

            if (limitNum > 0)
            {
                pushFreePool(nodeList[0]);
            }else
            {
                Object.Destroy(nodeList[0].obj);
            }
            nodeList.RemoveAt(0);
        }

        public void removeLastNode()
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            if (limitNum > 0)
            {
                pushFreePool(nodeList[nodeList.Count - 1]);
            }
            else
            {
                Object.Destroy(nodeList[nodeList.Count - 1].obj);
            }    
            nodeList.RemoveAt(nodeList.Count - 1);
        }

        public void removeAllNodes()
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            for (int i = 0; i < nodeList.Count; i++)
            {
                if(limitNum > 0)
                {
                    pushFreePool(nodeList[i]);
                }else
                {
                    Object.Destroy(nodeList[i].obj);
                }
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
                            obj = nodeList[i].obj;
                            obj.GetComponent<RectTransform>().pivot = HorizontalNodeAnchorPoint;
                            obj.transform.SetParent(container.transform);
                            obj.transform.localScale = new Vector3(1, 1,1);
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
                            obj = nodeList[i].obj;
                            allNodesSize += obj.GetComponent<RectTransform>().rect.height;
                        }

                        _layoutIndexSize = allNodesSize;
                        RectTransform rtran = GetComponent<RectTransform>();
                        allNodesSize = Mathf.Max(rtran.rect.height, allNodesSize);
                        setContainerSize(new Vector2(rtran.rect.width, allNodesSize));

                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            obj = nodeList[i].obj;
                            allNodesSize -= obj.GetComponent<RectTransform>().rect.height;
                            obj.GetComponent<RectTransform>().pivot = VerticalNodeAnchorPoint;
                            obj.transform.SetParent(container.transform);
                            obj.transform.localScale = new Vector3(1, 1,1);
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

            //unactive all tpl cell
            for(int i = 0; i < 10; i++)
            {
                Transform tran = transform.Find("container/cell_tpl" + i);
                if (tran != null) {
                    tran.gameObject.SetActive(false);
                }
            }

            relocateContainer();
        }

        protected override void onScrolling()
        {
            base.onScrolling();

            Vector2 worldPos = transform.position;
            Rect rect = transform.GetComponent<RectTransform>().rect;
            float resolution = Screen.height / 720.0f;

            foreach(LListNode node in nodeList)
            {
                if(node.obj.transform.position.y < worldPos.y - 300 * resolution ||
                    node.obj.transform.position.y > worldPos.y + rect.height + 300 * resolution)
                {
                    node.obj.SetActive(false);
                }else
                {
                    node.obj.SetActive(true);
                }
            }
        }

		public void scrollToCell (GameObject cell,float duration)
		{
			Vector2 cellPos = cell.transform.localPosition;
			if (direction == ScrollDirection.HORIZONTAL) {
				cellPos = new Vector2 (cellPos.x * -1, 0); 
			} else if (direction == ScrollDirection.VERTICAL) {
				cellPos = new Vector2 (0, cellPos.y * -1); 
			}
			setContentOffsetInDuration(cellPos,duration);
		}

        protected void checkRecycleItem()
        {
            if (limitNum > 0)
            {
                if (nodeList.Count > limitNum)
                {
                    int count = nodeList.Count - limitNum;
                    GameObject obj = null;
                    for(int i=0;i<count;i++){
                        removeFrontNode();
                    }
                }
            }
        }

        private void pushFreePool(LListNode node)
        {
            if (!freeDic.ContainsKey(node.tpl_id))
                freeDic.Add(node.tpl_id, new List<LListNode>());

            freeDic[node.tpl_id].Add(node);
            node.obj.SetActive(false);
        }

        private LListNode popFreePool(int id)
        {
            if (!freeDic.ContainsKey(id))
                freeDic.Add(id, new List<LListNode>());
            LListNode node = null;

            if (freeDic[id].Count > 0) {
                node = freeDic[id][0];
                freeDic[id].RemoveAt(0);
                node.obj.SetActive(true);
            }
            return node;
        }

        public GameObject dequeueItem(int id)
        {
            GameObject ret = null;
            if(limitNum > 0){
                LListNode node = popFreePool(id);
                if (node != null) ret = node.obj;
            }

            if (ret == null)
            {
                ret = Instantiate(transform.Find("container/cell_tpl"+id).gameObject);
                ret.SetActive(true);
            }
            return ret;
        }
    }
}

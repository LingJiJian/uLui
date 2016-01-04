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
    /// 列表
    /// </summary>
    public class LListView : LScrollView
    {
        public static Vector2 HorizontalNodeAnchorPoint = Vector2.zero;
        public static Vector2 VerticalNodeAnchorPoint = Vector2.zero;

        public int limitNum;
        protected float layoutIndexSize;
        public List<GameObject> nodeList { get; protected set; }
        public List<GameObject> freeList { get; protected set; }
        public GameObject itemTemplate;
        public Rect bounceBox;

        public LListView()
        {
            this.limitNum = 0;
            this.layoutIndexSize = 0;
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
                reloadData();
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
            layoutIndexSize = 0;
            switch (direction)
            {
                case ScrollDirection.HORIZONTAL:
                    {
                        GameObject obj = null;
                        for (int i = 0; i < nodeList.Count;i++ )
                        {
                            obj = nodeList[i];
                            obj.GetComponent<RectTransform>().pivot = HorizontalNodeAnchorPoint;
                            obj.transform.localPosition = new Vector2(layoutIndexSize, 0);
                            layoutIndexSize += obj.GetComponent<RectTransform>().rect.width;
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

                        layoutIndexSize = allNodesSize;
                        RectTransform rtran = GetComponent<RectTransform>();
                        allNodesSize = Mathf.Max(rtran.rect.height, allNodesSize);
                        setContainerSize(new Vector2(rtran.rect.width, allNodesSize));

                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            obj = nodeList[i];
                            allNodesSize -= obj.GetComponent<RectTransform>().rect.height;
                            obj.GetComponent<RectTransform>().pivot = VerticalNodeAnchorPoint;
                            obj.transform.SetParent(container.transform);
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
                if (nodeList.Count > limitNum)
                {
                    GameObject obj = null;
                    for (int i = 0; i < nodeList.Count - limitNum; i++)
                    {
                        obj = nodeList[i];
                        nodeList.Remove(obj);
                        freeList.Add(obj);
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
                    freeList.RemoveAt(0);
                }
            }
            if (ret == null)
            {
                ret = (GameObject)Instantiate(itemTemplate, Vector3.zero, itemTemplate.transform.rotation);
            }
            return ret;
        }

        protected override void onScrolling()
        {
            base.onScrolling();

            GameObject obj = null;
            for (int i = 0; i < nodeList.Count; i++)
            {
				obj = nodeList[i];
				Vector2 pos = obj.transform.position;
				Vector2 topPoint = new Vector2(pos.x,pos.y+obj.GetComponent<RectTransform>().rect.height);
				if (bounceBox.Contains(pos) ||
				    bounceBox.Contains(topPoint))
                {
					obj.SetActive(true);
                }
                else
                {
					obj.SetActive(false);
                }
            }
        }

		void Start()
		{
			RectTransform rtran = GetComponent<RectTransform>();
			this.bounceBox = new Rect(transform.position.x,
			                     transform.position.y,
			                     rtran.rect.width,
			                     rtran.rect.height);

			this.itemTemplate = Resources.Load("Prefabs/list_cell") as GameObject;
			this.limitNum = 10;
			for (int i=0; i<30; i++) {
				GameObject item = dequeueItem ();
				item.GetComponent<RectTransform>().sizeDelta = new Vector2(100,40+Random.Range(0,40));
				item.GetComponent<Text>().text = i.ToString();
				this.insertNodeAtLast(item);
			}
			this.reloadData();
		}
    }
}

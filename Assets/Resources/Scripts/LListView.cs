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
        protected Rect bounceBox;

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
            for (int i = 0; i < container.transform.childCount;i++ )
            {
                GameObject obj = container.transform.GetChild(i).gameObject;
                Destroy(obj);
            }
            
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

        void Awake()
        {
            RectTransform rtran = GetComponent<RectTransform>();
            bounceBox = new Rect(transform.position.x,
                                 transform.position.y,
                                 rtran.rect.width,
                                 rtran.rect.height);
        }

        protected override void onScrolling()
        {
            base.onScrolling();

            GameObject obj = null;
            for (int i = 0; i < nodeList.Count; i++)
            {
                obj = nodeList[i];
                if (!bounceBox.Contains(obj.transform.position))
                {
                    if (obj.transform.parent)
                    {
                        obj.transform.SetParent(null);
                    }
                }
                else
                {
                    if (obj.transform.parent == null)
                    {
                        obj.transform.SetParent(container.transform);
                    }
                }
            }
        }
    }
}

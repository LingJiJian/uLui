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

namespace Lui
{
    
    public class LExpandListView : LScrollView
    {
        protected List<LExpandNode> _expandableNodeList;
        public int nodeNum;
        public int nodeItemNum;

        public GameObject cell_tpl;
        public GameObject cell_sub_tpl;

        public LExpandListView()
        {
            direction = ScrollDirection.VERTICAL;
            _expandableNodeList = new List<LExpandNode>();
        }

        public void expand(int idx)
        {
            _expandableNodeList[idx].setExpanded(true);
        }

        public void collapse(int idx)
        {
            _expandableNodeList[idx].setExpanded(false);
        }

        public void insertExpandableNodeAtLast(LExpandNode node)
        {
            if (node == null)
            {
                Debug.LogWarning("insert node is null");
                return;
            }

            _expandableNodeList.Add(node);
            node.transform.SetParent(container.transform);
        }

        public void insertExpandableNodeAtFront(LExpandNode node)
        {
            if (node == null)
            {
                Debug.LogWarning("insert node is null");
                return;
            }

            _expandableNodeList.Insert(0, node);
            node.transform.SetParent(container.transform);
        }

        public void removeExpandNode(LExpandNode node)
        {
            if (node == null)
            {
                Debug.LogWarning("insert node is null");
                return;
            }

            if (_expandableNodeList.Count == 0) return;
            _expandableNodeList.Remove(node);
        }

        public void removeExpandNodeAtIndex(int idx)
        {
            if (_expandableNodeList.Count == 0) return;
            _expandableNodeList.RemoveAt(idx);
            Destroy(_expandableNodeList[idx]);
        }

        public void removeLastExpandNode()
        {
            if (_expandableNodeList.Count == 0) return;
            _expandableNodeList.RemoveAt(_expandableNodeList.Count - 1);
            Destroy(_expandableNodeList[_expandableNodeList.Count - 1]);
        }

        public void removeFrontExpandNode()
        {
            if (_expandableNodeList.Count == 0) return;
            _expandableNodeList.RemoveAt(0);
            Destroy(_expandableNodeList[0]);
        }

        public void removeAllExpandNodes()
        {
            if (_expandableNodeList.Count == 0) return;
			int len = _expandableNodeList.Count;
            for(int i=0;i<len; i++)
            {
                Destroy(_expandableNodeList[i].gameObject);
            }
            _expandableNodeList.Clear();
        }

        public List<LExpandNode> getExpandableNodes()
        {
            return _expandableNodeList;
        }

        public int getExpandableNodeCount()
        {
            return _expandableNodeList.Count;
        }

        public LExpandNode getExpandableNodeAtIndex(int idx)
        {
            return _expandableNodeList[idx];
        }

        public void updateNodesPosition()
        {
            if (_expandableNodeList.Count == 0) return;

            float allNodesHeight = 0.0f;
            int nodeLen = _expandableNodeList.Count;
            for(int i=0;i< nodeLen; i++)
            {
                LExpandNode node = _expandableNodeList[i];
                allNodesHeight += node.gameObject.GetComponent<RectTransform>().rect.height;

                if (node.isExpanded())
                {
                    List<GameObject> nodeItems = node.getExpandableNodeItemList();
                    int len = nodeItems.Count;
                    if (len > 0)
                    {
                        for(int _i=0;_i<len;_i++)
                        {
							GameObject obj = nodeItems[_i];
                            obj.SetActive(true);
                            allNodesHeight += obj.GetComponent<RectTransform>().rect.height;
                        }
                    }
                }
                else
                {
                    List<GameObject> nodeItems = node.getExpandableNodeItemList();
					int len = nodeItems.Count;
                    if (len > 0)
                    {
                        for(int _i=0;_i<len;_i++)
                        {
                            nodeItems[_i].SetActive(false);
                        }
                    }
                }
            }

            Rect rect = GetComponent<RectTransform>().rect;
            allNodesHeight = Mathf.Max(allNodesHeight, rect.height);
            setContainerSize(new Vector2(rect.width, allNodesHeight));

            for (int i = 0; i < nodeLen; i++)
            {
                LExpandNode node = _expandableNodeList[i];
                RectTransform rtran = node.gameObject.GetComponent<RectTransform>();
                allNodesHeight = allNodesHeight - rtran.rect.height;

                rtran.pivot = Vector2.zero;
                rtran.anchorMax = new Vector2(0, 0);
                rtran.anchorMin = new Vector2(0, 0);
                node.transform.SetParent(container.transform);
                node.transform.localPosition = new Vector2(0, allNodesHeight);
                node.transform.localScale = new Vector3(1, 1, 1);

                if (node.isExpanded())
                {
                    List<GameObject> itemLists = node.getExpandableNodeItemList();
                    for(int j = 0; j < itemLists.Count; j++)
                    {
                        RectTransform _rtran = itemLists[j].GetComponent<RectTransform>();
                        allNodesHeight = allNodesHeight - _rtran.rect.height;

                        _rtran.pivot = Vector2.zero;
                        _rtran.anchorMax = new Vector2(0, 0);
                        _rtran.anchorMin = new Vector2(0, 0);
                        itemLists[j].transform.SetParent(container.transform);
                        itemLists[j].transform.localPosition = new Vector2(0, allNodesHeight);
                        itemLists[j].transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
        }

        public void prepare()
        {
            if (nodeNum > 0)
            {
                if (cell_tpl == null) { 
                    cell_tpl = transform.Find("container/cell_tpl").gameObject;
                }
                if (cell_sub_tpl == null){
                    cell_sub_tpl = transform.Find("container/cell_sub_tpl").gameObject;
                }
                cell_tpl.SetActive(false);
                cell_sub_tpl.SetActive(false);
                
                for (int i = 0; i < nodeNum; i++)
                {
                    GameObject nodeObj = Instantiate(cell_tpl);
                    nodeObj.SetActive(true);
                    nodeObj.transform.SetParent(container.transform);
                    LExpandNode node = nodeObj.AddComponent<LExpandNode>();
                    node.tpl = cell_sub_tpl;
                    node.prepare(nodeItemNum);
                    insertExpandableNodeAtLast(node);
                }
            }
        }

        public void reloadData()
        {
            if(direction != ScrollDirection.VERTICAL)
            {
                Debug.LogWarning("LExpandListView should be Vertical");
                return;
            }
            float oldHeight = container.GetComponent<RectTransform>().rect.height;
            updateNodesPosition();

            float newHeight = container.GetComponent<RectTransform>().rect.height - oldHeight;
            setContentOffset(getContentOffset() - new Vector2(0, newHeight));
            relocateContainer();
        }

    }

}
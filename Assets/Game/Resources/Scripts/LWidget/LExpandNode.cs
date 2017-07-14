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
using UnityEngine.Events;


namespace Lui { 

    
    public class LExpandNode : MonoBehaviour
    {
        protected bool _expanded;
        protected int _idx;
        protected List<GameObject> _expandNodeItemList;
        [HideInInspector]
        public GameObject tpl;

        public LExpandNode()
        {
            _idx = -1;
            _expanded = false;
            _expandNodeItemList = new List<GameObject>();
        }

        public void insertItemNodeAtLast(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("insert obj is null");
                return;
            }

            _expandNodeItemList.Add(obj);
        }

        public void insertItemNodeAtFront(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("insert obj is null");
                return;
            }

            _expandNodeItemList.Insert(0, obj);
        }

        public void removeItemNode(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("remove obj is null");
                return;
            }
            if (_expandNodeItemList.Count == 0) return;

            _expandNodeItemList.Remove(obj);
            Destroy(obj);
        }

        public void removeItemNodeAtIndex(int idx)
        {
            if (_expandNodeItemList.Count == 0) return;
            _expandNodeItemList.RemoveAt(idx);
            Destroy(_expandNodeItemList[idx]);
        }

        public GameObject getItemNodeAtIndex(int idx)
        {
            if (_expandNodeItemList.Count == 0) return null;
            return _expandNodeItemList[idx];
        }

        public void removeAllItemNodes()
        {
            if (_expandNodeItemList.Count == 0) return;
			int len = _expandNodeItemList.Count;
            for (int i=0;i<len;i++)
            {
                Destroy(_expandNodeItemList[i]);
            }
            _expandNodeItemList.Clear();
        }

        public void setExpanded(bool value)
        {
            _expanded = value;
        }

        public bool isExpanded()
        {
            return _expanded;
        }

        public List<GameObject> getExpandableNodeItemList()
        {
            return _expandNodeItemList;
        }

        public void prepare(int itemNums)
        {
            removeAllItemNodes();

            for (int i = 0; i < itemNums; i++) {
                GameObject obj = Instantiate(this.tpl);
                obj.transform.SetParent(this.transform.parent);
                insertItemNodeAtLast(obj);
            }
        }

        //public void setOnClick(string path,UnityAction<LExpandNode> cb)
        //{

        //}

    }
}
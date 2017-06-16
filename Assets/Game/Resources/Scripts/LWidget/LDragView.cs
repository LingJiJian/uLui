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
    public class LDragView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public UnityAction<Vector2> onBeginDrag;
        public UnityAction<Vector2> onDrag;
        public UnityAction<Vector2> onEndDrag;

        [SLua.DoNotToLua]
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(onBeginDrag!=null)
                onBeginDrag.Invoke(eventData.position);
        }

        [SLua.DoNotToLua]
        public void OnDrag(PointerEventData eventData)
        {
            if(onDrag!=null)
                onDrag.Invoke(eventData.position);
        }

        [SLua.DoNotToLua]
        public void OnEndDrag(PointerEventData eventData)
        {
            if(onEndDrag!=null)
                onEndDrag.Invoke(eventData.position);
        }
    }

}

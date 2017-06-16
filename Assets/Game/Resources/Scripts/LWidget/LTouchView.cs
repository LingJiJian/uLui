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
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lui
{
    public class LTouchView : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public UnityAction<Vector2> onMoveBeginHandler;
        public UnityAction<Vector2> onMoveHandler;
        public UnityAction<Vector2> onMoveEndHandler;
        public UnityAction<GameObject> onClickHandler2D;
        public UnityAction<Vector2> onClickHandler;
        private Vector2 _lastPoint;
		private Collider2D _lastTarget;
		private bool _hasCancel;

        public LTouchView()
        {
        }

        [SLua.DoNotToLua]
        public void OnPointerDown(PointerEventData eventData)
        {
            _lastPoint = eventData.position;
			_lastTarget = null;
			_hasCancel = false;


            if(onMoveBeginHandler != null)
            {
                onMoveBeginHandler.Invoke(eventData.position);
            }
        }

        [SLua.DoNotToLua]
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 offset = eventData.position - _lastPoint;
            _lastPoint = eventData.position;
			_hasCancel = true;

			if (onMoveHandler != null)
            {
				onMoveHandler.Invoke(offset);
            }
        }

        [SLua.DoNotToLua]
        public void OnPointerUp(PointerEventData eventData)
        {
            Vector3 worldPos = Vector3.zero;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
#else
            worldPos = Camera.main.ScreenToWorldPoint (eventData.position);
#endif
            if (onMoveEndHandler != null && _hasCancel)
            {
                onMoveEndHandler.Invoke(worldPos);
            }

            if (onClickHandler != null && (_hasCancel == false))
            {
                onClickHandler.Invoke(worldPos);
            }
        }
    }

}

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
using SLua;

namespace Lui
{
    [CustomLuaClass]
    public class LTouchView : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public UnityAction onMoveBeginHandler;
        public UnityAction<Vector2> onMoveHandler;
        public UnityAction onMoveEndHandler;
        public UnityAction<GameObject> onClickHandler2D;
        private Vector2 _lastPoint;
		private Collider2D _lastTarget;
		private bool _hasCancel;

        public LTouchView()
        {
        }

        [DoNotToLua]
        public void OnPointerDown(PointerEventData eventData)
        {
            _lastPoint = eventData.position;
			_lastTarget = null;
			_hasCancel = false;

			if (onClickHandler2D != null)
			{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
#else
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(eventData.position), Vector2.zero);
#endif
                if (hit.collider != null) {
					_lastTarget = hit.collider;
				}
			}

            if(onMoveBeginHandler != null)
            {
                onMoveBeginHandler.Invoke();
            }
        }
        [DoNotToLua]
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
        
        [DoNotToLua]
        public void OnPointerUp(PointerEventData eventData)
        {
			if (onClickHandler2D != null && _hasCancel == false) 
			{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
#else
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(eventData.position), Vector2.zero);
#endif
                if (hit.collider != null) {
					if (hit.collider == _lastTarget) {
						onClickHandler2D.Invoke (hit.collider.gameObject);
					}
				}
			}

            if (onMoveEndHandler != null)
            {
                onMoveEndHandler.Invoke();
            }
        }
    }

}

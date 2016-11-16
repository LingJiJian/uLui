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
    public class LTouchView : MonoBehaviour
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        , IPointerDownHandler, IPointerUpHandler, IDragHandler
#endif
    {
        public UnityAction<Vector2> onMoveHandler;
		public UnityAction<GameObject> onClickHandler2D;
		public UnityAction<GameObject> onClickHandler;
        private Vector2 _lastPoint;
		private Collider2D _lastTarget2D;
		private Collider _lastTarget;
		private bool _hasCancel;

#if UNITY_ANDROID || UNITY_IPHONE
        [DoNotToLua]
        void Update()
        {
            if( !LUtil.Windows )
            {
				if (onMoveHandler != null)
                {
					if (Input.touchCount > 0)
                    {
						if (Input.GetTouch (0).phase == TouchPhase.Moved) {
							
							onMoveHandler.Invoke (Input.GetTouch (0).deltaPosition);

						} else if (Input.GetTouch (0).phase == TouchPhase.Began) {

							_hasCancel = false;

							if (onClickHandler2D != null) {

								_lastTarget2D = null;

								RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), Vector2.zero);
								if (hit.collider != null) {
									_lastTarget2D = hit.collider;
								}
							} else if (onClickHandler != null) {

								_lastTarget = null;

								Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
								RaycastHit hit;
								if (Physics.Raycast (ray, out hit)) {
									if (hit.collider != null) {
										_lastTarget = hit.collider;
									}
								}
							}

						}else if (Input.GetTouch(0).phase == TouchPhase.Canceled){

							if (onClickHandler2D != null) {
								_hasCancel = true;
							}
						} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {

							if (onClickHandler2D != null) {
							
								RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), Vector2.zero);
								if (hit.collider != null) {
									if (hit.collider == _lastTarget2D && _hasCancel == false) {
										onClickHandler2D.Invoke (hit.collider.gameObject);
									}
								}
							} else if (onClickHandler != null) {
							
								Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
								RaycastHit hit;
								if (Physics.Raycast (ray, out hit)) {
									if (hit.collider != null) {
										if (hit.collider == _lastTarget && _hasCancel == false) {
											onClickHandler.Invoke (hit.collider.gameObject);
										}
									}
								}
							}
						}
                    }
                }
            }
        }


#endif

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        [DoNotToLua]
        public void OnPointerDown(PointerEventData eventData)
        {
            _lastPoint = eventData.position;
			_lastTarget2D = null;
			_hasCancel = false;

			if (onClickHandler2D != null) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit.collider != null) {
					_lastTarget2D = hit.collider;
				}
			} else if (onClickHandler != null) {
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider != null) {
						_lastTarget = hit.collider;
					}
				}
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
			if (onMoveHandler != null)
            {
				onMoveHandler.Invoke(Vector2.zero);
            }

			if (onClickHandler2D != null && _hasCancel == false) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit.collider != null) {
					if (hit.collider == _lastTarget2D) {
						onClickHandler2D.Invoke (hit.collider.gameObject);
					}
				}
			} else if (onClickHandler != null && _hasCancel == false) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider != null) {
						if (hit.collider == _lastTarget) {
							onClickHandler.Invoke (hit.collider.gameObject);
						}
					}
				}
			}
        }
#endif
    }

}

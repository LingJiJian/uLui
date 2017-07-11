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
    public enum ScrollDirection
    {
        HORIZONTAL,
        VERTICAL,
        BOTH
    }
    /// <summary>
    /// 滑块
    /// </summary>
	public class LScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler , IEndDragHandler
    {
        public static int INVALID_INDEX = -1;
        public static float RELOCATE_DURATION = 0.2f;
        public static float AUTO_RELOCATE_SPPED = 100.0f;
        public static float INERTANCE_SPEED = 0.96f;
		public static float RESISTANCE_SPEED = 0.8f;
        protected float autoRelocateSpeed;
        
        public bool bounceable;
        protected GameObject container;
        public ScrollDirection direction;
        private Vector2 lastMovePoint;
        private Vector2 maxOffset;
        private Vector2 minOffset;
        protected Vector2 scrollDistance;
        public bool dragable;
        private bool _isPicking;
        public bool pickEnable;
		public bool inertanceEnable;
		private float _scrollPerc;
		private bool _isInertanceFinish;
		private bool _isDraging;
		private bool _hasDragBegin;
        [HideInInspector]
        public GameObject curPickObj;

        public delegate T0 LDataSourceAdapter<T0, T1>(T0 arg0, T1 arg1);
        public UnityAction onMoveCompleteHandler;
        public UnityAction<float> onScrollingHandler;
        public UnityAction onScrollBeginHandler;
        public UnityAction onDraggingScrollEndedHandler;
        public UnityAction<GameObject> onPickBeginHandler;
        public UnityAction<Vector3> onPickIngHandler;
        public UnityAction<GameObject> onPickEndHandler;

        public LScrollView()
        {
            autoRelocateSpeed = AUTO_RELOCATE_SPPED;
            direction = ScrollDirection.BOTH;
            lastMovePoint = Vector2.zero;
            bounceable = true;
            scrollDistance = Vector2.zero;
            dragable = true;
            maxOffset = Vector2.zero;
            minOffset = Vector2.zero;
        }

        void Awake()
        {
            container = transform.Find("container").gameObject;

            updateLimitOffset();

            RectTransform rtran = container.GetComponent<RectTransform>();
            rtran.pivot = Vector2.zero;
            rtran.anchorMax = Vector2.zero;
            rtran.anchorMin = Vector2.zero;

            rtran = GetComponent<RectTransform>();
            rtran.pivot = Vector2.zero;
            rtran.anchorMax = Vector2.zero;
            rtran.anchorMin = Vector2.zero;
        }

        public void setContainerSize(Vector2 size)
        {
            Vector2 cs = GetComponent<RectTransform>().rect.size;
            int width = Mathf.Max((int)cs.x, (int)size.x);
            int height = Mathf.Max((int)cs.y, (int)size.y);

            container.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            updateLimitOffset();
        }

        protected void updateLimitOffset()
        {
            Vector2 size = GetComponent<RectTransform>().rect.size;
            Vector2 innSize = container.GetComponent<RectTransform>().rect.size;
            maxOffset.x = 0;
            minOffset.x = size.x - innSize.x;

            maxOffset.y = 0;
            minOffset.y = size.y - innSize.y;

            if (direction == ScrollDirection.HORIZONTAL)
            {
                minOffset.y = 0;
            }else if (direction == ScrollDirection.VERTICAL)
            {
                minOffset.x = 0;
            }
        }
		
		public void Update()
        {
			if (_hasDragBegin == false)
				return;
			if (_isDraging) return;
            if (inertanceEnable)
            {
                Vector2 offset = getContentOffset() + scrollDistance;
                if (validateOffset(ref offset)){
					if (Mathf.Abs (scrollDistance.x) >= 1f || Mathf.Abs (scrollDistance.y) >= 1f) {
						scrollDistance *= RESISTANCE_SPEED;
						setContentOffsetWithoutCheck (getContentOffset () + scrollDistance);

						_isInertanceFinish = false;

					} else {
						if (!_isInertanceFinish) {
							_isInertanceFinish = true;

							relocateContainerWithoutCheck(offset);
						}
					}
				}else{
					if (Mathf.Abs (scrollDistance.x) >= 1f || Mathf.Abs (scrollDistance.y) >= 1f) {
						scrollDistance *= INERTANCE_SPEED;
						setContentOffsetWithoutCheck (getContentOffset () + scrollDistance);

						_isInertanceFinish = false;

					} else {
						if (!_isInertanceFinish) {
							_isInertanceFinish = true;

							onDraggingScrollEnded ();
						}
					}
				}
            }
        }
			
		
		public void OnBeginDrag(PointerEventData eventData)
        {
            if(_isDraging) return;
			_hasDragBegin = true;

            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (dragable)
            {
                lastMovePoint = point;

				scrollDistance = Vector2.zero;
				_isDraging = true;
                LeanTween.cancel(container);
                
                onScrollBegin();
                onScrolling();
            }

            if (pickEnable)
            {
                _isPicking = false;
            }
        }

        
        public void OnDrag(PointerEventData eventData)
        {
            if (pickEnable && _isPicking)
            {
                if (onPickIngHandler != null)
                    onPickIngHandler.Invoke(eventData.position);
                return;
            }
                
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (dragable)
            {
                scrollDistance = point - lastMovePoint;
                lastMovePoint = point;

                if(pickEnable && (Mathf.Abs(scrollDistance.y) > Mathf.Abs(scrollDistance.x)))
                {
                    _isPicking = true;
                    if (onPickBeginHandler != null)
                    {
                        onPickBeginHandler.Invoke(curPickObj);
                    }
                    return;
                }

                switch (direction)
                {
                    case ScrollDirection.HORIZONTAL:
                        scrollDistance.y = 0;
                        break;
                    case ScrollDirection.VERTICAL:
                        scrollDistance.x = 0;
                        break;
                    default:
                        break;
                }

				Vector2 vec = getContentOffset () + scrollDistance;
				if (validateOffset (ref vec)) {
					if (direction == ScrollDirection.VERTICAL) {
						scrollDistance.y *= 0.2f;
					} else if (direction == ScrollDirection.HORIZONTAL) {
						scrollDistance.x *= 0.2f;
					}
				}
                setContentOffsetWithoutCheck(getContentOffset() + scrollDistance);
            }
        }

        
		public void OnEndDrag(PointerEventData eventData)
        {
            if (dragable)
            {
				_isDraging = false;

                Vector2 offset = getContentOffset();
                if (validateOffset(ref offset))
                {
                    relocateContainerWithoutCheck(offset);
                }
                else
                {
					if(!inertanceEnable)
                    	onDraggingScrollEnded();
                }
            }

            if (pickEnable)
            {
                _isPicking = false;

                if (onPickEndHandler != null)
                    onPickEndHandler.Invoke(curPickObj);

                curPickObj = null;
            }
        }

        protected void relocateContainerWithoutCheck(Vector2 offset)
        {
            setContentOffsetEaseInWithoutCheck(offset, RELOCATE_DURATION);
        }

        protected void relocateContainer()
        {
            Vector2 offset = getContentOffset();
            if (validateOffset(ref offset))
            {
                setContentOffsetEaseInWithoutCheck(offset, RELOCATE_DURATION);
            }
        }

        protected void setContentOffsetEaseInWithoutCheck(Vector2 offset, float duration)
        {
            LeanTween.cancel(container);
            LeanTween.moveLocal(container, offset, duration)
                .setEase(LeanTweenType.easeInQuad)
                .setOnUpdate((float val) => { onScrolling(); })
                .setOnComplete(onMoveComplete);
                
            onScrolling();
        }

        protected void setContentOffsetEaseIn(Vector2 offset, float duration, float rate)
        {
            validateOffset(ref offset);
            
            setContentOffsetEaseInWithoutCheck(offset, duration);
        }

        public void setContentOffset(Vector2 offset)
        {
			validateOffset (ref offset);

            container.transform.localPosition = offset;
			onScrolling();
        }

        public void setContentOffsetWithoutCheck(Vector2 offset)
        {
            container.transform.localPosition = offset;
            onScrolling();
        }

        public void setContentOffsetToTop()
        {
            if (direction == ScrollDirection.VERTICAL)
            {
                Vector2 point = new Vector2(0, -(container.GetComponent<RectTransform>().rect.height - GetComponent<RectTransform>().rect.height));
                setContentOffset(point);
            }
        }

        public void setContentOffsetToBottom()
        {
            if (direction == ScrollDirection.VERTICAL)
            {
                setContentOffset(maxOffset);
            }
        }

        public void setContentOffsetToRight()
        {
	        if( direction == ScrollDirection.HORIZONTAL )
	        {
		        setContentOffset(minOffset);
	        }
        }

        public void setContentOffsetToLeft()
        {
            if (direction == ScrollDirection.HORIZONTAL )
	        {
		        setContentOffset(maxOffset);
	        }
        }

        public void setContentOffsetInDuration(Vector2 offset, float duration)
        {
            if (bounceable)
            {
                validateOffset(ref offset);
            }
            setContentOffsetInDurationWithoutCheck(offset, duration);
        }

        public void setContentOffsetInDurationWithoutCheck(Vector2 offset, float duration)
        {
			LeanTween.cancel (container);
            LeanTween.moveLocal(container, offset, duration)
                .setOnUpdate((float val) => { onScrolling(); })
                .setOnComplete(onMoveComplete);
            
            onScrolling();
        }

        protected bool validateOffset(ref Vector2 point)
        {
            float x = point.x, y = point.y;
            x = Mathf.Max(x, minOffset.x);
            x = Mathf.Min(x, maxOffset.x);
            y = Mathf.Max(y, minOffset.y);
            y = Mathf.Min(y, maxOffset.y);

            if (point.x != x || point.y != y)
            {
                point.x = x;
                point.y = y;
                return true;
            }

            point.x = x;
            point.y = y;
            return false;
        }

		protected bool validateOffsetBounce(Vector2 point)
		{
			float ratio = Screen.height / 720.0f;
			float x = point.x, y = point.y;
			x = Mathf.Max(x, minOffset.x - 100 * ratio);
			x = Mathf.Min(x, maxOffset.x + 100 * ratio);
			y = Mathf.Max(y, minOffset.y - 100 * ratio);
			y = Mathf.Min(y, maxOffset.y + 100 * ratio);

			if (point.x != x || point.y != y)
			{
				//point.x = x;
				//point.y = y;
				return true;
			}

			//point.x = x;
			//point.y = y;
			return false;
		}

        public Vector2 getContentOffset()
        {
            return container.transform.localPosition;
        }

        protected void onMoveComplete()
        {
            if (onMoveCompleteHandler != null)
            {
                onMoveCompleteHandler.Invoke();
            }
        }

        protected virtual void onScrolling()
		{
			this._scrollPerc = 0.0f;
			if (direction == ScrollDirection.HORIZONTAL) {
				float width = this.GetComponent<RectTransform>().rect.width;
                float containerWidth = container.GetComponent<RectTransform> ().rect.width;
				this._scrollPerc = containerWidth - width == 0 ? 0 : -container.transform.localPosition.y / (containerWidth - width);

			} else if(direction == ScrollDirection.VERTICAL) {
				
                float height = this.GetComponent<RectTransform>().rect.height;
                float containerHeight = container.GetComponent<RectTransform> ().rect.height;
				this._scrollPerc = containerHeight - height == 0 ? 0 : -container.transform.localPosition.y / (containerHeight - height);
			}
			if (onScrollingHandler!=null)
				onScrollingHandler.Invoke (this._scrollPerc);
        }

        protected virtual void onScrollBegin()
        {
            if (onScrollBeginHandler !=null)
            {
                onScrollBeginHandler.Invoke();
            }
        }

        protected virtual void onDraggingScrollEnded()
        {
            if (onDraggingScrollEndedHandler!=null)
            {
                onDraggingScrollEndedHandler.Invoke();
            }
        }

    }

}

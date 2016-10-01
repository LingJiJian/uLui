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
using SLua;

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
    [CustomLuaClass]
    public class LScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static int INVALID_INDEX = -1;
        public static float RELOCATE_DURATION = 0.2f;
        public static float AUTO_RELOCATE_SPPED = 600.0f;
        protected float autoRelocateSpeed;
        
        public bool bounceable;
        protected GameObject container;
        public ScrollDirection direction;
        private Vector2 lastMovePoint;
        private Vector2 maxOffset;
        private Vector2 minOffset;
        protected Vector2 scrollDistance;
        public bool dragable;

        public delegate T0 LDataSourceAdapter<T0, T1>(T0 arg0, T1 arg1);
        public UnityAction onMoveCompleteHandler;
        public UnityAction onScrollingHandler;
        public UnityAction onDraggingScrollEndedHandler;

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

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (dragable)
            {
                lastMovePoint = point;
                LeanTween.cancel(container);
                onScrolling();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (dragable)
            {
                scrollDistance = point - lastMovePoint;
                lastMovePoint = point;

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

                setContentOffset(getContentOffset() + scrollDistance);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragable)
            {
                Vector2 offset = getContentOffset();
                if (validateOffset(ref offset))
                {
                    relocateContainerWithoutCheck(offset);
                }
                else
                {
                    onDraggingScrollEnded();
                }
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
            if (!bounceable)
            {
                validateOffset(ref offset);
            }
            setContentOffsetEaseInWithoutCheck(offset, duration);
        }

        public void setContentOffset(Vector2 offset)
        {
            if (!bounceable)
            {
                validateOffset(ref offset);
            }
            LeanTween.cancel(container);
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
            if (onScrollingHandler!=null)
            {
                onScrollingHandler.Invoke();
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

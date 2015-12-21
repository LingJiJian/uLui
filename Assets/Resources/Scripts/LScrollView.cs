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
    public class LScrollView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        protected const float MOVE_OVERFLOW = 10f;
        protected const float RELOCATE_DURATION = 0.2f;

        public bool bounceable;
        public GameObject container;
        public ScrollDirection direction;
        private Vector2 lastMovePoint;
        private Vector2 maxOffset;
        private Vector2 minOffset;
        protected Vector2 scrollDistance;
        public bool dragable;

        public LScrollView()
        {
            direction = ScrollDirection.BOTH;
            lastMovePoint = Vector2.zero;
            bounceable = true;
            scrollDistance = Vector2.zero;
            dragable = true;
            maxOffset = Vector2.zero;
            minOffset = Vector2.zero;
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

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (dragable)
            {
                lastMovePoint = point;
                iTween.Stop(container);
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

        public void OnPointerUp(PointerEventData eventData)
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
            iTween.Stop(container);
            iTween.MoveTo(container, iTween.Hash(
                "position", transform.TransformPoint(offset), 
                "time", duration,
                "easetype",iTween.EaseType.easeInQuad,
                "onupdate","onScrolling",
                "onupdatetarget",gameObject,
                "oncomplete", "onMoveComplete",
                "oncompletetarget",gameObject));

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
            else
            {
                validateOffset(ref offset, new Vector2(0, 100));
            }
            iTween.Stop(container);
            container.transform.localPosition = offset;
            onScrolling();
        }

        public void setContentOffsetWithoutCheck(Vector2 offset)
        {
            container.transform.localPosition = offset;
            onScrolling();
        }

        protected bool validateOffset(ref Vector2 point)
        {
            return validateOffset(ref point, Vector2.zero);
        }

        protected bool validateOffset(ref Vector2 point, Vector2 append)
        {
            float x = point.x, y = point.y;
            x = Mathf.Max(x, minOffset.x );
            x = Mathf.Min(x, maxOffset.x + append.x);
            y = Mathf.Max(y, minOffset.y );
            y = Mathf.Min(y, maxOffset.y + append.y);

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
            Debug.Log("onMoveComplete");
        }

        void Awake()
        {
            updateLimitOffset();
        }

        protected void onScrolling()
        {
            Debug.Log("onScrolling");

        }

        protected void onDraggingScrollEnded()
        {
            Debug.Log("onDraggingScrollEnded");

        }

    }

}

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
        protected const float MOVE_EASEIN_RATE = 0.5f;
        protected const float RELOCATE_DURATION = 0.2f;
        protected const float DEACCELERATE_INTERVAL = 0.245f;

        public bool bounceable;
        public bool deaccelerateable;
        public GameObject container;
        public ScrollDirection direction;
        private Vector2 lastMovePoint;
        private Vector2 maxOffset;
        private Vector2 minOffset;
        private bool deaccelerateScrolling;
        private bool animatedScrolling;
        protected float dragSpeed;
        protected Vector2 scrollDistance;
        protected Vector2 touchBeganPoint;
        public bool dragable;

        public LScrollView()
        {
            direction = ScrollDirection.BOTH;
            lastMovePoint = Vector2.zero;
            bounceable = false;
            deaccelerateable = true;
            dragSpeed = 0;
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
            touchBeganPoint = point;
            if (dragable)
            {
                lastMovePoint = point;
                stoppedDeaccelerateScroll();
                stoppedAnimatedScroll();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            float distance = Vector2.Distance(point, touchBeganPoint);
            if (distance < MOVE_OVERFLOW)
            {
               
            }
            
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
                if (deaccelerateable && direction != ScrollDirection.BOTH && Time.deltaTime < DEACCELERATE_INTERVAL)
                {
                    Vector2 endPoint = transform.InverseTransformPoint(eventData.position);
                    switch (direction)
                    {
                        case ScrollDirection.HORIZONTAL:
                            dragSpeed = Mathf.Abs(touchBeganPoint.x - endPoint.x) / Time.deltaTime;
                            break;
                        default:
                            dragSpeed = Mathf.Abs(touchBeganPoint.y - endPoint.y) / Time.deltaTime;
                            break;
                    }
                }
                else
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
        }

        protected void relocateContainerWithoutCheck(Vector2 offset)
        {
            setContentOffsetEaseInWithoutCheck(offset, RELOCATE_DURATION, MOVE_EASEIN_RATE);
        }

        protected void relocateContainer()
        {
            Vector2 offset = getContentOffset();
            if (validateOffset(ref offset))
            {
                setContentOffsetEaseInWithoutCheck(offset, RELOCATE_DURATION, MOVE_EASEIN_RATE);
            }
        }

        protected void setContentOffsetEaseInWithoutCheck(Vector2 offset, float duration, float rate)
        {
            iTween.Stop(container);
            iTween.MoveTo(container, iTween.Hash(
                "position", transform.TransformPoint(offset), 
                "time", duration,
                "easetype",iTween.EaseType.easeInQuad,
                "oncomplete", "stoppedAnimatedScroll"));

            onScrolling();
        }

        protected void setContentOffsetEaseIn(Vector2 offset, float duration, float rate)
        {
            if (!bounceable)
            {
                validateOffset(ref offset);
            }
            setContentOffsetEaseInWithoutCheck(offset, duration, rate);
        }

        public void setContentOffset(Vector2 offset)
        {
            if (!bounceable)
            {
                validateOffset(ref offset);
            }
            iTween.Stop(container);
            container.transform.localPosition = offset;
            onScrolling();
        }

        protected bool validateOffset(ref Vector2 point)
        {
            float x = point.x; 
            float y = point.y;
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

        protected void stoppedDeaccelerateScroll()
        {
            iTween.Stop(container);
        }

        protected void stoppedAnimatedScroll()
        {
            iTween.Stop(container);
            onScrolling();
        }

        void Awake()
        {
            updateLimitOffset();
        }

        protected void onScrolling()
        {


        }

        protected void onDraggingScrollEnded()
        {


        }

    }

}

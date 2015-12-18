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

    class LScrollViewContainer : MonoBehaviour
    {
        public void reset()
        {
            int len = transform.childCount;
            for (int i = 0; i < len; i++)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                Destroy(obj);
            }
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.pivot = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            transform.localPosition = Vector2.zero;
        }
    }

    /// <summary>
    /// 滑块
    /// </summary>
    public class LScrollView
    {
        public bool bounceable;
        public bool deaccelerateable;
        protected LScrollViewContainer container;
        public ScrollDirection direction;
        private Vector2 lastMovePoint;
        private bool dragging;
        private bool touchMoved;
        private Vector2 maxOffset;
        private Vector2 minOffset;
        private bool deaccelerateScrolling;
        private bool animatedScrolling;
        protected float dragSpeed;
        protected Vector2 scrollDistance;

        public LScrollView()
        {
            direction = ScrollDirection.BOTH;
            lastMovePoint = Vector2.zero;
            bounceable = true;
            dragging = false;
            deaccelerateable = true;
            dragSpeed = 0;
            scrollDistance = Vector2.zero;

        }

        public LScrollViewContainer getContainer()
        {
            return container;
        }
    }
}

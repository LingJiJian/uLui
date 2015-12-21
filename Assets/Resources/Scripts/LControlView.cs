using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace Lui
{
    /// <summary>
    /// 摇杆
    /// </summary>
    public class LControlView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        protected const float MOVE_DELAY = 0.5f;
        protected const int PARAM_PRE = 10;

        public Vector2 centerPoint;
        public int radius;
        public bool relocateWithAnimation;
        public GameObject joyStick;
        private Vector2 lastPoint;
        private UnityAction<float, float> onControlHandler;
        private Rect joyStickBoundBox;

        public LControlView()
        {
            this.radius = 100;
            this.centerPoint = Vector2.zero;
            this.lastPoint = Vector2.zero;
            this.relocateWithAnimation = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            stopAnimateUpdate();
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (joyStick)
            {
                if (eventData.pointerEnter == joyStick)
                {
                    onExecuteEventHandle();
                }
            }
            else
            {
                lastPoint = point;
                onExecuteEventHandle();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (joyStick)
            {
                Vector2 point = transform.InverseTransformPoint(eventData.position);
                float dis = Vector3.Distance(centerPoint, point);
                joyStick.transform.localPosition = dis < radius ? point : new Vector2(
                    ((point.x - centerPoint.x) / dis) * radius + centerPoint.x,
                    ((point.y - centerPoint.y) / dis) * radius + centerPoint.y);
            }
            else
            {
                Vector2 point = transform.InverseTransformPoint(eventData.position);
                float dis = Vector3.Distance(centerPoint, point);
                lastPoint = dis < radius ? point : new Vector2(
                    ((point.x - centerPoint.x) / dis) * radius + centerPoint.x,
                    ((point.y - centerPoint.y) / dis) * radius + centerPoint.y);
            }

            onExecuteEventHandle();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (joyStick)
            {
                if (!relocateWithAnimation)
                {
                    onExecuteEventHandle();
                }
                relocateJoystick(relocateWithAnimation);
            }
            else
            {
                onExecuteEventHandle();
            }
        }

        public void pushControlHandler(UnityAction<float, float> act)
        {
            onControlHandler += act;
        }

        protected void relocateJoystick(bool anim)
        {
            if (anim)
            {
                iTween.MoveTo(joyStick, iTween.Hash(
                    "position", transform.TransformPoint(centerPoint),
                    "time", MOVE_DELAY,
                    "onupdate", "onExecuteEventHandle",
                    "onupdatetarget", gameObject));
            }
            else
            {
                joyStick.transform.localPosition = centerPoint;
            }
        }

        protected void stopAnimateUpdate()
        {
            if (joyStick)
            {
                iTween.Stop(joyStick);
            }
        }

        void onExecuteEventHandle()
        {
            if (onControlHandler == null)
            {
                return;
            }

            if (joyStick)
            {
                Vector2 v = joyStick.transform.localPosition;
                Vector2 offset = v - centerPoint;
                onControlHandler.Invoke(offset.x / PARAM_PRE, offset.y / PARAM_PRE);
            }
            else
            {
                Vector2 offset = lastPoint - centerPoint;
                onControlHandler.Invoke(offset.x / PARAM_PRE, offset.y / PARAM_PRE);
            }
        }

        void Start()
        {
            pushControlHandler((float x, float y) =>
            {

                //Debug.Log(" offset x,y " + x + " " + y);
            });
        }
    }

}
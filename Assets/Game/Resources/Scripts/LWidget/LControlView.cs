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
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using SLua;

namespace Lui
{
    /// <summary>
    /// 摇杆
    /// </summary>
    [CustomLuaClassAttribute]
    public class LControlView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        protected const float MOVE_TIME = 0.5f;
        protected const int PARAM_PRE = 10;

        public Vector2 centerPoint;
        public int radius;
        public bool relocateWithAnimation;
        public GameObject joyStick;
        private Vector2 _lastPoint;
        public UnityAction<float, float,bool> onControlHandler;

        public LControlView()
        {
            this.radius = 100;
            this.centerPoint = Vector2.zero;
            this._lastPoint = Vector2.zero;
            this.relocateWithAnimation = true;
        }
        [DoNotToLua]
        public void OnPointerDown(PointerEventData eventData)
        {
            stopAnimateUpdate();
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (joyStick)
            {
                if (eventData.pointerEnter == joyStick)
                {
                    onExecuteEventHandle(false);
                }
            }
            else
            {
                _lastPoint = point;
                onExecuteEventHandle(false);
            }
        }
        [DoNotToLua]
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
                _lastPoint = dis < radius ? point : new Vector2(
                    ((point.x - centerPoint.x) / dis) * radius + centerPoint.x,
                    ((point.y - centerPoint.y) / dis) * radius + centerPoint.y);
            }

            onExecuteEventHandle(false);
        }
        [DoNotToLua]
        public void OnPointerUp(PointerEventData eventData)
        {
            if (joyStick)
            {
                if (!relocateWithAnimation)
                {
                    onExecuteEventHandle(true);
                }
                relocateJoystick(relocateWithAnimation);
            }
            else
            {
                onExecuteEventHandle(true);
            }
        }

        protected void relocateJoystick(bool anim)
        {
            if (anim)
            {
                LeanTween.move(joyStick, transform.TransformPoint(centerPoint), MOVE_TIME)
                    .setOnUpdate((float val) => { onExecuteEventHandle(false); })
                    .setOnComplete(() => { onExecuteEventHandle(true); });
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
                LeanTween.cancel(joyStick);
            }
        }

        void onExecuteEventHandle(bool isFinish)
        {
            if (onControlHandler == null)
            {
                return;
            }

            if (joyStick)
            {
                Vector2 v = joyStick.transform.localPosition;
                Vector2 offset = v - centerPoint;
                onControlHandler.Invoke(offset.x / PARAM_PRE, offset.y / PARAM_PRE, isFinish);
            }
            else
            {
                Vector2 offset = _lastPoint - centerPoint;
                onControlHandler.Invoke(offset.x / PARAM_PRE, offset.y / PARAM_PRE, isFinish);
            }
        }
    }

}
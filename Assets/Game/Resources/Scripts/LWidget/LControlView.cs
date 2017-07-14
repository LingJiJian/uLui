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

namespace Lui
{
    /// <summary>
    /// 摇杆
    /// </summary>
    
    public class LControlView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public float MOVE_TIME = 0.5f;
		public float BG_FLOW_SPEED = 8f;

        public Vector2 centerPoint;
        public int radius;
        public int radiusBg;
		public float flowDis;
        public bool relocateWithAnimation;
        public GameObject joyStick;
        public GameObject joyBg;
		public GameObject rotateBg;
		private Vector2 _dragPoint;
		private bool _isDraging;

        private Vector2 _lastPoint;
        public UnityAction<float, float,bool> onControlHandler;
        public UnityAction<bool> onControlChangeHandler;

        public LControlView()
        {
            this.radius = 150;
            this.radiusBg = 140;
			this.flowDis = 65;
            this.centerPoint = Vector2.zero;
            this._lastPoint = Vector2.zero;
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
                    onExecuteEventHandle(false);
                }
            }
            else
            {
                _lastPoint = point;
                onExecuteEventHandle(false);
            }
			if (onControlChangeHandler != null) {
				onControlChangeHandler.Invoke(true);
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
                _lastPoint = dis < radius ? point : new Vector2(
                    ((point.x - centerPoint.x) / dis) * radius + centerPoint.x,
                    ((point.y - centerPoint.y) / dis) * radius + centerPoint.y);
            }
	
			
			if (rotateBg) {
                rotateBg.SetActive(true);
				Vector2 point = joyStick.transform.localPosition;
				Vector2 toPos = point;
				Vector2 fromPos = centerPoint;
				float angle = Mathf.Rad2Deg * Mathf.Atan ((fromPos.y - toPos.y) / (fromPos.x - toPos.x));
				if(fromPos.x - toPos.x < 0.0f){
					angle = angle - 90;
				}else{
					angle = angle + 90;
				}
				rotateBg.transform.rotation = Quaternion.Euler (0, 0, angle);
			}

			_isDraging = true;
			_dragPoint = eventData.position;

            onExecuteEventHandle(false);
        }

        
        public void OnPointerUp(PointerEventData eventData)
        {
			_isDraging = false;

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
				
			if (onControlChangeHandler != null) {
				onControlChangeHandler.Invoke (false);
			}

            if(rotateBg)
            {
                rotateBg.SetActive(false);
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
            if(joyBg)
            {
                LeanTween.cancel(joyBg);
            }
        }

		void LateUpdate(){
			
			if (joyBg ) {

				if (_isDraging) {
					Vector2 point = transform.InverseTransformPoint (_dragPoint);
					float dis = Vector3.Distance (centerPoint, point);
					if (dis >= radiusBg) {

						Vector3 targetPos = new Vector2 (
							((point.x - centerPoint.x) / dis) * radiusBg + centerPoint.x,
							((point.y - centerPoint.y) / dis) * radiusBg + centerPoint.y);
						Vector3 offset = targetPos - joyBg.transform.localPosition;

						if ( Vector2.Distance(joyBg.transform.localPosition,targetPos) < flowDis)
							return;
						joyBg.transform.localPosition += (offset * Time.deltaTime * BG_FLOW_SPEED);
					} else {

						Vector3 targetWorldPos = joyStick.transform.position;
						Vector3 bgWorldPos = joyBg.transform.position;
						Vector3 offset = targetWorldPos - bgWorldPos;

						if ( Vector2.Distance(joyBg.transform.position,targetWorldPos) < flowDis)
							return;
						joyBg.transform.position += offset * Time.deltaTime * BG_FLOW_SPEED;
					}
				} else {
					
					Vector3 targetWorldPos = joyStick.transform.position;
					Vector3 bgWorldPos = joyBg.transform.position;
					Vector3 offset = targetWorldPos - bgWorldPos;

					if ( Vector2.Distance(joyBg.transform.position,targetWorldPos) < 0.01)
						return;
					joyBg.transform.position += offset * Time.deltaTime * BG_FLOW_SPEED;
				}
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
				Vector2 offset = (v - centerPoint).normalized;
                onControlHandler.Invoke(offset.x , offset.y , isFinish);
            }
            else
            {
				Vector2 offset = (_lastPoint - centerPoint).normalized;
                onControlHandler.Invoke(offset.x , offset.y , isFinish);
            }
        }
    }

}
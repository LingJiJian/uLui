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
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lui
{
	[SLua.CustomLuaClass]
	public class LSwitch : MonoBehaviour,IPointerDownHandler, IPointerUpHandler, IDragHandler {

		public GameObject bar;
		private Vector2 _p1;
		private Vector2 _p2;
		private bool _value;
		private Vector2 _lastPoint;
		public UnityAction<bool> onValueHandler;

		void Start()
		{
			_p1 = bar.transform.localPosition;
			_p2 = new Vector2 (-_p1.x, _p1.y);
		}

		[SLua.DoNotToLua]
		public void OnPointerDown(PointerEventData eventData)
		{
			_lastPoint = eventData.position;
		}

		[SLua.DoNotToLua]
		public void OnPointerUp(PointerEventData eventData)
		{
			LeanTween.cancel (bar);
			if (bar.transform.localPosition.x <= 0) {
				LeanTween.moveLocalX (bar, _p1.x, 0.1f).setOnComplete(()=>{
					_value = false;
					if(onValueHandler!=null) onValueHandler.Invoke(_value);
				});
			} else {
				LeanTween.moveLocalX (bar, _p2.x, 0.1f).setOnComplete(()=>{
					_value = true;
					if(onValueHandler!=null) onValueHandler.Invoke(_value);
				});
			}
		}

		[SLua.DoNotToLua]
		public void OnDrag(PointerEventData eventData)
		{
			Vector2 offset = (eventData.position - _lastPoint);
			_lastPoint = eventData.position;

			Vector2 pos = bar.transform.localPosition;
			pos += new Vector2 (offset.x, 0);

			if (Mathf.Abs (pos.x) <= Mathf.Abs (_p1.x)) {
				bar.transform.localPosition = pos;
			}
		}

		public void setValue(bool value)
		{
			_value = value;
			bar.transform.localPosition = value ? _p1 : _p2;
		}
	}
}
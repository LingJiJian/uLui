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
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class LInputfield : MonoBehaviour,IPointerClickHandler {
		
	private Vector3 originPos;
	public Transform container;
	private bool _isPoping;
	private bool _isFocus;
	private float offsetY = 0.0f;

	void Start(){
		if(container == null){
			container = this.transform;
		}

		originPos = container.position;
		GetComponent<InputField> ().onEndEdit.AddListener ((string input) => {
			_isFocus = false;
			_isPoping = false;
			container.position = new Vector3(container.position.x,container.position.y - offsetY,0);
		});
	}

	public static int GetKeyboardHeight()
	{
		#if !UNITY_EDITOR
		#if UNITY_ANDROID
		using(AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

			using(AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
			{
			View.Call("getWindowVisibleDisplayFrame", Rct);

			return Screen.height - Rct.Call<int>("height");
			}
		}
		#elif UNITY_IOS
			return (int)TouchScreenKeyboard.area.height;
		#endif
		#else
			return 0;
		#endif
	}

	void Update(){
		if (_isFocus) {
			if (GetKeyboardHeight () > 0 && _isPoping == false) {
				_isPoping = true;

				float targetHeight = GetKeyboardHeight() + 130;
				if(transform.position.y < targetHeight){
					offsetY = targetHeight - transform.position.y;
					container.position = new Vector3(container.position.x,container.position.y + offsetY,0);
				}
			}

		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_isFocus = true;
	}
		
}
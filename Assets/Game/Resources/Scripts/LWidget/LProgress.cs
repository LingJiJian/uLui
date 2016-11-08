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
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[SLua.CustomLuaClass]
public class LProgress : MonoBehaviour {

	public float maxValue;
	public float minValue;
	private float _value;
	public Image bar;
	public Text label;
	private bool _isStartProgress;
	private float _arrivePercentage;
	private float _startProgressStep;
	public UnityAction onProgress;
	public UnityAction onProgressEnd;

	public LProgress()
	{
		maxValue = 100;
		minValue = 0;
		_value = 15;
		_isStartProgress = false;
		_arrivePercentage = 0;
		_startProgressStep = 0;
	}

	public void setValue(float value)
	{
		this._value = Mathf.Min(maxValue, Mathf.Max(minValue, value));
		bar.fillAmount = this._value / maxValue;
	}

	public float getPercentage()
	{
		return (_value - minValue) / (maxValue - minValue); 
	}

	public void startProgress(float value,float step)
	{
		if (value <= minValue && value >= maxValue) {
			return;
		}

		_arrivePercentage = (value - minValue) / (maxValue - minValue);
		_startProgressStep = step;
		_isStartProgress = true;
	}

	void Update()
	{
		if (_isStartProgress) {
			_value = _value + _startProgressStep;
			float perc = getPercentage ();
			bar.fillAmount = perc;

			if (label) {
				label.text = (perc * 100 ).ToString("0.0")+"%";
			}

			if (perc < _arrivePercentage) {
				
				if (onProgress!=null)
					onProgress.Invoke ();
			} else {

				if (onProgressEnd!=null)
					onProgressEnd.Invoke ();
				_isStartProgress = false;
			}
		}
	}
}

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
public class LSlider : LProgress, IDragHandler,IPointerDownHandler,IPointerUpHandler
{

    public Image block;
    private float _width;
    private float _height;
    public UnityAction<PointerEventData> onPointerDownHandle;
    public UnityAction<PointerEventData> onPointerUpHandle;

    void Start()
    {
        _width = GetComponent<RectTransform>().rect.width;
        _height = GetComponent<RectTransform>().rect.height;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 point = validSlidePoint(transform.InverseTransformPoint(eventData.position));

        if (onProgress != null)
            onProgress.Invoke();

        setValue((point.x + _width / 2) / _width * maxValue);
    }

    [SLua.DoNotToLua]
    public void OnPointerDown(PointerEventData data)
    {
        if (onPointerDownHandle != null)
        {
            onPointerDownHandle.Invoke(data);
        }
    }

    [SLua.DoNotToLua]
    public void OnPointerUp(PointerEventData data)
    {
        if (onPointerUpHandle != null)
        {
            onPointerUpHandle.Invoke(data);
        }
    }

    private Vector3 validSlidePoint(Vector2 point)
    {
        return new Vector3(Mathf.Max(-_width / 2, Mathf.Min(_width / 2, point.x)), 0.0f, 0.0f);
    }

    public void setValue(float value)
    {
        base.setValue(value);
        block.transform.localPosition = validSlidePoint(new Vector2(-_width/2 + _width * getPercentage(),0));
    }
}
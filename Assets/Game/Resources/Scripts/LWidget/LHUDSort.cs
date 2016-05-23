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
using System.Collections.Generic;
using System.Collections;
using SLua;

[CustomLuaClass]
public class LHUDSort : MonoBehaviour {

    private List<Transform> list;

    // Use this for initialization
    void Start () {
        list = new List<Transform>();
    }

    // Update is called once per frame
    void Update () {

        list.Clear();

        foreach(Transform t in transform)
        {
            if (t.gameObject.GetComponent<LHUDView>() != null)
            {
                list.Add(t);
            }
        }

        list.Sort((a, b) =>
        {
            return (int)((a.position.z - b.position.z) * 1000);
        });

        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.SetSiblingIndex(i);
        }
    }
}

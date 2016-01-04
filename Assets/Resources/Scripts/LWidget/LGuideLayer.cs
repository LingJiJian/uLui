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

public class LGuideLayer : MonoBehaviour {

	public GameObject panel_guide_sub1;
	public GameObject panel_guide_sub2;
	public GameObject panel_guide_sub3;
	public GameObject panel_guide_sub4;
	public GameObject btn_target;

    void Start()
    {

		Vector2 btn_size = btn_target.GetComponent<RectTransform> ().rect.size;
		Vector2 btn_pos = btn_target.transform.position;

		Vector2 root_size = gameObject.GetComponent<RectTransform> ().rect.size;
	
		panel_guide_sub1.transform.position = new Vector3 (0,root_size.y,0);
		panel_guide_sub1.GetComponent<RectTransform> ().sizeDelta = new Vector2 (root_size.x, root_size.y-btn_pos.y-btn_size.y/2);

		panel_guide_sub2.transform.position = new Vector3 (root_size.x, btn_pos.y);
		panel_guide_sub2.GetComponent<RectTransform> ().sizeDelta = new Vector2 (root_size.x - btn_pos.x - btn_size.x / 2, btn_size.y);

		panel_guide_sub3.transform.position = new Vector3 (0, 0);
		panel_guide_sub3.GetComponent<RectTransform> ().sizeDelta = new Vector2 (root_size.x, btn_pos.y - btn_size.y / 2);

		panel_guide_sub4.transform.position = new Vector3 (0, btn_pos.y);
		panel_guide_sub4.GetComponent<RectTransform> ().sizeDelta = new Vector2 (btn_pos.x - btn_size.x / 2, btn_size.y);
	}
	

}

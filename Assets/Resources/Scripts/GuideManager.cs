using UnityEngine;
using System.Collections;

public class GuideManager : MonoBehaviour {

	public GameObject panel_guide_sub1;
	public GameObject panel_guide_sub2;
	public GameObject panel_guide_sub3;
	public GameObject panel_guide_sub4;
	public GameObject btn_target;
	public GameObject panel_guide;

	// Use this for initialization
	// warning!!! this update just for test!!!  you should use Start()
	void Update () {

		Vector2 btn_size = btn_target.GetComponent<RectTransform> ().rect.size;
		Vector2 btn_pos = btn_target.transform.position;

		Vector2 root_size = panel_guide.GetComponent<RectTransform> ().rect.size;
	
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

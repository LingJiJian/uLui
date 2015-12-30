using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class LButton : MonoBehaviour {

    // Use this for initialization
    void Start() {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //Application.LoadLevel("uLui");
            LWindowManager wm = LSingleton.getInstance("LWindowManager") as LWindowManager;
            wm.runWindow("Window1",typeof(Window1),WindowHierarchy.Normal);
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

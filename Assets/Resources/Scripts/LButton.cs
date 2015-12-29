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
            Application.LoadLevel("uLui");
        
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

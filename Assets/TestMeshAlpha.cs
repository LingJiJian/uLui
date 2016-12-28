using UnityEngine;
using System.Collections;

public class TestMeshAlpha : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //LeanTween.alphaVertex(gameObject, 0, 0.5f);
        this.transform.GetComponent<MeshRenderer>().material.color = new Color(0,1,0,0.5f);
    }
}

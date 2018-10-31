using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CemetaryBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Image>().color = Vector4.one * 0.6f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

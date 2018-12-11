using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneOperation : MonoBehaviour {

	void Start( ) {
	}
	
	void Update( ) {
	}

	public bool MouseTouch( ) { 
		//マウスがクリックされたら
		if ( Input.GetMouseButtonDown( 0 ) ) {
			return true;
		} else { 
			return false;	
		}
	}
}

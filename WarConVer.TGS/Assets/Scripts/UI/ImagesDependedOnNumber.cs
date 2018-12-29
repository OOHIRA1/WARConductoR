using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ImagesDependedOnNumber : MonoBehaviour {
	protected Point _point = null;
	protected SpriteRenderer[ ] _image = null;	//Scene上にあるImage配列

	void Update( ) {
		UpdateteImages( );
	}

	public abstract void UpdateteImages( );
}

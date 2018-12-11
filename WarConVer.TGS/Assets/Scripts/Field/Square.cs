using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
	[ SerializeField ] int _index = 0;	//マスの通し番号
	
	public int Index {
		get { return _index; }
		private set { _index = value; } 
	}
}

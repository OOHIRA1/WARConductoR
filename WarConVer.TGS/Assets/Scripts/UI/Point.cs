using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	[ SerializeField ] int _max_point = 0;
	int _point_num = 0;

	public int Max_Point {
		get { return _max_point; }
		set { _max_point = value; }
	}

	public int Point_Num { 
		get { return _point_num; }
		set { _point_num = value; }
	}

	void Start( ) {
		Point_Num = _max_point;
	}

	//ポイントを減らす------------------------
	public void DecreasePoint( int point ) {	//減らすポイントが現在の値より多かったら
		if ( !DecreasePointConfirmation( point ) ) {
			Debug.Log( "減らすポイントが大きすぎる" );
			return;	
		}
		
		Point_Num -= point;	
	}
	//----------------------------------------

	//減らすポイントが今あるポイントより多いかどうかを調べる
	public bool DecreasePointConfirmation( int point ) { 
		if ( point > Point_Num ) { 
			return false;	
		} else { 
			return true;	
		}
	}

}

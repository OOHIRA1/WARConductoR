using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	[ SerializeField ] int _initial_point = 0;
	int _point_num = 0;

	public int Initial_Point {
		get { return _initial_point; }
		private set { _initial_point = value; }
	}

	public int Point_Num { 
		get { return _point_num; }
		private set { _point_num = value; }
	}

	void Awake( ) {
		Point_Num = Initial_Point;	//他のクラスのStartでPoint_Numを見たいためAwakeで処理
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

	public void addPoint( int point ) { 
		Point_Num += point;	
	}

	//減らすポイントが今あるポイントより多いかどうかを調べる--
	public bool DecreasePointConfirmation( int point ) { 
		if ( point > Point_Num ) { 
			return false;	
		} else { 
			return true;	
		}
	}
	//--------------------------------------------------------
}

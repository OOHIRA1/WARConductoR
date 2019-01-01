using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	const int MAX_MAGIC_POINT = 12;

	[ SerializeField ] int _maxPoint = 0;	//最大値
	[ SerializeField ] int _point = 0;

	public int Max_Point {
		get { return _maxPoint; }
	}

	public int Point_Num { 
		get { return _point; }
	}

	//ポイントを減らす------------------------
	public void DecreasePoint( int point ) {	//減らすポイントが現在の値より多かったら
		if ( !DecreasePointConfirmation( point ) ) {
			Debug.Log( "減らすポイントが大きすぎる" );
			return;	
		}
		
		_point -= point;	
	}
	//----------------------------------------

	
	//ポイントを増やす---------------------
	public void IncreasePoint( int point ) {
		_point += point;	

		if ( _point > _maxPoint ) { 
			_point = _maxPoint;
		}
	}
	//-------------------------------------


	//減らすポイントが今あるポイントより多いかどうかを調べる--
	public bool DecreasePointConfirmation( int point ) { 
		if ( point > Point_Num ) { 
			return false;	
		} else { 
			return true;	
		}
	}
	//--------------------------------------------------------

	//最大値を増やす--------------------------------
	public void IncreaseMaxPoint( int point ) { 
		_maxPoint += point;

		if ( _maxPoint > MAX_MAGIC_POINT ) { 
			_maxPoint = MAX_MAGIC_POINT;
		}
	}
	//----------------------------------------------
}

//MPだけどうしても特別処理が必要。なんかいい方法ある？派生クラスつくる？
//そうすればその種類でのPointしか使わない機能をまとめらるので奇麗かも？
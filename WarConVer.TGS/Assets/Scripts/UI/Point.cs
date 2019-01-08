using UnityEngine;

public class Point : MonoBehaviour {
	[ SerializeField ] int _maxPoint = 0;	//最大値
	[ SerializeField ] int _pointNum = 0;

	public int Max_Point {
		get { return _maxPoint; }
	}

	public int Point_Num { 
		get { return _pointNum; }
	}

	//ポイントを減らす------------------------
	public void DecreasePoint( int point ) {	//減らすポイントが現在の値より多かったら
		if ( !DecreasePointConfirmation( point ) ) {
			Debug.Log( "減らすポイントが大きすぎる" );
			return;	
		}
		
		_pointNum -= point;	
	}
	//----------------------------------------

	
	//ポイントを増やす---------------------
	public void IncreasePoint( int point ) {
		_pointNum += point;	

		if ( _pointNum > _maxPoint ) { 
			_pointNum = _maxPoint;
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
	}
	//----------------------------------------------
}
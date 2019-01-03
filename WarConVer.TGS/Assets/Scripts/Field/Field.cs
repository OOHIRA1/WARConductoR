using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
	public enum DIRECTION {						//移動する方角
		LEFT_FORWARD = 1,
		FORWAED,
		RIGHT_FORWARD,
		LEFT,
		RIGHT,
		LEFT_BACK,
		BACK,
		RIGHT_BACK
	}

	Square[ ] _squares = new Square[ 20 ];		//マス
	int _maxIndex = 0; 

	public int Max_Index {
		get{ return _maxIndex; }
	}


	void Awake( ) {
		_squares = this.gameObject.GetComponentsInChildren< Square >( );
	}

	void Start( ) { 
		_maxIndex = _squares.Length;	
	}


	//指定した番号のマスを返す-------------
	public Square getSquare( int index ) { 
		return _squares[ index - 1 ];
	}
	//-------------------------------------


	//現在のマスから指定した方向の指定した距離にあるマスを返す--------------------------------------
	public Square SquareInThatDirection( Square nowSquare, DIRECTION direction, int distance ) {
		if ( distance < 0 ) return null;

		int index = 0;
		I_SearchSquare searchSquare = CreateISearchSquare( direction );
		index = searchSquare.SearchSquare( nowSquare.Index, direction, distance );
		if ( index == -1 ) {
			return null;
		} else { 
			return _squares[ index - 1 ];
		}
	}
	//---------------------------------------------------------------------------------------------


	//現在のマスから指定した範囲のマスの色を赤くする----------------
	public void ShowRange( List< Square > squares, bool value ) { 
		for ( int i = 0; i < squares.Count; i++ ) { 
			squares[ i ].ChangeColor( value );
		}
	}
	//--------------------------------------------------------------


	//Strategyパターンでマスを調べるアルゴリズムを変更している (Commandパターンかも？)-----
	I_SearchSquare CreateISearchSquare( DIRECTION direction ) { 
		switch ( direction ) {
			
			case DIRECTION.FORWAED:
			case DIRECTION.BACK:
				return new ForwardAndBackSearchSquare( );
			
			case DIRECTION.LEFT:
			case DIRECTION.RIGHT:
				return new LeftAndRightSearchSquare( );

			case DIRECTION.LEFT_FORWARD:
			case DIRECTION.RIGHT_FORWARD:
			case DIRECTION.LEFT_BACK:
			case DIRECTION.RIGHT_BACK:
				return new DiagonalSearchSquare( );

			default:
				return null;
		}
	}
	//-------------------------------------------------------------------------------------
}

//インスペクター上のIndexと配列のIndexがずれているせいでIndexを合わせるためにいろんなことろで無駄な計算をしている(Index - 1, Index + 1 など)
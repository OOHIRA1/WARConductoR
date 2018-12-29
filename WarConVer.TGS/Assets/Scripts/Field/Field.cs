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
	int _max_index = 0; 

	public int Max_Index {
		get{ return _max_index; }	
	}

	void Awake( ) {
		_squares = this.gameObject.GetComponentsInChildren< Square >( );
	}

	void Start( ) { 
		_max_index = _squares.Length;	
	}

	//Strategyパターンでマスを調べるアルゴリズムを変更している (Commandパターンかも？)-----
	ISearchSquare CreateISearchSquare( DIRECTION direction ) { 
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

	//現在のマスから指定した方向の指定した距離にあるマスを返す-----------------------------
	public Square SquareInThatDirection( Square now_square, DIRECTION direction, int distance ) {
		if ( distance < 0 ) return null;

		int index = 0;
		ISearchSquare search_square = CreateISearchSquare( direction );
		index = search_square.SearchSquare( now_square.Index, direction, distance );
		if ( index == -1 ) { 
			return null;
		} else { 
			return _squares[ index - 1 ];
		}
	}
	//------------------------------------------------------------------------------------

	public Square getSquare( int index ) { 
		return _squares[ index - 1 ];
	}

	//現在のマスから指定した範囲のマスの色を赤くする
	public void ShowRange( List< Square > squares, bool value ) { 
		for ( int i = 0; i < squares.Count; i++ ) { 
			squares[ i ].ChangeColor( value );
		}
	}
}

//インスペクター上のIndexと配列のIndexがずれているせいでIndexを合わせるためにいろんなことろで無駄な計算をしている(Index - 1, Index + 1 など)
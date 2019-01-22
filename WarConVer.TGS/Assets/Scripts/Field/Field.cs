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

	const int MAX_SQUARE = 20;

	Square[ ] _squares = new Square[ MAX_SQUARE ];		//マス
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
		return _squares[ index ];
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
			return _squares[ index ];
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


	//移動できる場所を事前に調べる関数-------------------------------------------------------------------------------------------------
	public List< Square > MovePossibleSquare( CardMain card, Square nowSquare, int distans = 1 ) { 
		List< Square > squares = new List< Square >( );

		//移動できるマスだけ格納
		List< DIRECTION > directions = card.getDirections( card.gameObject.tag, card.Card_Data._directionOfTravel );
		for ( int i = 0; i < directions.Count; i++ ) {
			Square square = SquareInThatDirection( nowSquare, directions[ i ], distans );

			if ( square == null ) continue;
			if ( square.On_Card != null ) {
				if ( square.On_Card.gameObject.tag == card.gameObject.tag ) continue;	//マスにあるのが自分のカードだったらcontinue
			}

			squares.Add( square );
				
		}

		return squares;
	}
	//----------------------------------------------------------------------------------------------------------------------------------

	
	//攻撃効果をするマスにカードがあるマスを事前に調べる関数-----------------------------------------------------------------------------------
	public List< Square > AttackEffectPossibleOnCardSquare( CardMain card, Square nowSquare ) { 
		List< Square > squares = new List< Square >( );

		//攻撃できるマスだけ格納
		List< Field.DIRECTION > directions = card.getDirections( card.gameObject.tag, card.Card_Data._effect_direction );
		for ( int i = 0; i < directions.Count; i++ ) {
			Square square = SquareInThatDirection( nowSquare, directions[ i ], card.Card_Data._effect_distance );

			if ( square == null ) continue;
			if ( square.On_Card == null ) continue;
			squares.Add( square );
				
		}

		Debug.Log( squares );
		return squares;
	}
	//------------------------------------------------------------------------------------------------------------------------------------

	//召喚できるマスを事前に調べる関数-----------------------------
	public List< Square > SummonSquare( string player ) { 
		List< Square > squares = new List< Square >( );
		
		if ( player == ConstantStorehouse.TAG_PLAYER1 ) { 
			for ( int i = 0; i < _maxIndex; i++ ) { 
				Square square = getSquare( i );

				if ( ( square.Index ) / ConstantStorehouse.SQUARE_ROW_NUM != ConstantStorehouse.FIFTH_ROW_INDEX )  continue;
				if ( square.On_Card != null ) continue;

				squares.Add( square );
			}		
		}
		
		if ( player == ConstantStorehouse.TAG_PLAYER2 ) { 
			for ( int i = 0; i < _maxIndex; i++ ) { 
				Square square = getSquare( i );
		
				if ( ( square.Index ) / ConstantStorehouse.SQUARE_ROW_NUM != ConstantStorehouse.FIRST_ROW_INDEX )  continue;
				if ( square.On_Card != null ) continue;
		
				squares.Add( square );
			}		
		}

		return squares;
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
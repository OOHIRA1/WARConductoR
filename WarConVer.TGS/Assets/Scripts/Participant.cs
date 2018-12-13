using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour {
	[ SerializeField ] Field _field				 = null;
	[ SerializeField ] Point _active_point		 = null;
	[ SerializeField ] Point _life_point		 = null;

	void Start( ) {
	}
	
	
	void Update( ) {
	}

	//カードのを移動させる
	public void CardMove( CardMain card, Square now_square, Field.DIRECTION[ ] directions, int distans, Square touch_square, int move_ap ) {
		List< Square > squares = new List< Square >( );

		//移動できるマスだけ格納-------------------------------------------------------------------
		for ( int i = 0; i < directions.Length; i++ ) {
			Square square = _field.SquareInThatDirection( now_square, directions[ i ], distans );
			if ( square != null ) {
				squares.Add( square );
			}
		}
		//-----------------------------------------------------------------------------------------


		//移動できるマスの中に移動したいマスがあるか探す-------------------------------------------
		for ( int i = 0; i < squares.Count; i++ ) { 
			if ( squares[ i ].Index == touch_square.Index ) {	//あったら移動する
				card.gameObject.transform.position = touch_square.gameObject.transform.position;
				now_square.On_Card = null;	//現在のマスから乗っていたカードを外す
				_active_point.DecreasePoint( move_ap );
				touch_square.On_Card = card;
				return;
			}
		}
		//------------------------------------------------------------------------------------------
	}


	public void DirectAttack( Participant opponent_player, int move_ap ) {
		_active_point.DecreasePoint( move_ap );
		opponent_player._life_point.DecreasePoint( 1 );
	}


	//アクティブポイントが足りてるかどうかを判定する-----------
	public bool  DecreaseActivePointConfirmation( int point ) { 
		return _active_point.DecreasePointConfirmation( point );
	}
	//---------------------------------------------------------


	//移動できるマスの色を変える
	public void SquareChangeColor( Square now_square, Field.DIRECTION[ ] directions, int distans, bool value ) {

		List< Square > squares = new List< Square >( );
		
		//移動できるマスだけ格納-------------------------------------------------------------------
		for ( int i = 0; i < directions.Length; i++ ) {
			Square square = _field.SquareInThatDirection( now_square, directions[ i ], distans );
			if ( square != null ) {
				squares.Add( square );
			}
		}
		//------------------------------------------------------------------------------------------

		_field.ShowRange( squares, value );	//移動できるマスの色を変える

	}
}

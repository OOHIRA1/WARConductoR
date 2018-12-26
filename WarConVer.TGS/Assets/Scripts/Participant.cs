using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour {

	//バトルの結果
	enum BATTLE_RESULT {
		NOT_BATTLE,
		BOTH_DEATH,
		PLAYER_WIN,
		PLAYER_DEFEAT,
		BOTH_ALIVE,
	}

	[ SerializeField ] Field _field			= null;
	[ SerializeField ] Point _active_point	= null;
	[ SerializeField ] Point _life_point	= null;

	//カードを移動させる-----------------------------------------------------------------------------------------------------------------------------
	public void CardMove( CardMain card, Square now_square, Square move_square, string player ) {
		List< Square > squares = new List< Square >( );

		//移動できるマスだけ格納
		squares = MovePossibleSquare( card, now_square, player );

		BATTLE_RESULT result = BATTLE_RESULT.NOT_BATTLE;
		//移動できるマスの中に移動したいマスがあるか探す
		for ( int i = 0; i < squares.Count; i++ ) { 
			if ( squares[ i ].Index == move_square.Index ) {										//あったら移動する
				if ( !IsOnCardType( "Player2", squares[ i ] ) ) {
					result = Battle( card, squares[ i ].On_Card );
					Debug.Log( result );
				}

				//戦闘の結果によって移動処理を変える
				switch( result ) { 
					case BATTLE_RESULT.BOTH_DEATH:
						now_square.On_Card = null;
						move_square.On_Card = null;
						break;

					case BATTLE_RESULT.NOT_BATTLE:
					case BATTLE_RESULT.PLAYER_WIN:
						now_square.On_Card = null;
						move_square.On_Card = card;
						card.gameObject.transform.position = move_square.transform.position;
						break;

					case BATTLE_RESULT.PLAYER_DEFEAT:
						now_square.On_Card = null;
						break;

					case BATTLE_RESULT.BOTH_ALIVE:
						break;

					default:
						Debug.Log( "予期せぬ勝敗が起きている" );
						return;
				}
				_active_point.DecreasePoint( card._cardDates.move_ap );
				//now_square.On_Card = null;														//現在のマスから乗っていたカードを外す
				//move_square.On_Card = card;														//移動したマスにカードを乗せる
				return;
			}
		}
		
	}
	//----------------------------------------------------------------------------------------------------------------------------------------------


	public void DirectAttack( Participant opponent_player, int move_ap ) {
		_active_point.DecreasePoint( move_ap );
		opponent_player._life_point.DecreasePoint( 1 );
	}


	//アクティブポイントが足りてるかどうかを判定する-----------
	public bool  DecreaseActivePointConfirmation( int point ) { 
		return _active_point.DecreasePointConfirmation( point );
	}
	//---------------------------------------------------------


	//指定のマスの色を変える------------------------------------------------------------------------------------------
	public void SquareChangeColor( List< Square > squares, bool change_red ) {
		_field.ShowRange( squares, change_red );	//指定できるマスの色を変える
	}
	//--------------------------------------------------------------------------------------------------------------------


	//攻撃効果(オーバロード)-----------------------------------------------------------------------------------------------------------------------
	public void UseEffect( CardMain card, Square now_square ) {
		List< Square > squares = new List< Square >( );
	
		//効果範囲内でカードがあるマスだけ格納
		squares = AttackEffectPossibleOnCardSquare( card, now_square );
		
	
		for ( int i = 0; i < squares.Count; i++ ) { 
			squares[ i ].On_Card.Damage( card._cardDates.effect_damage );
		}

		_active_point.DecreasePoint( card._cardDates.effect_ap );
	}
	//--------------------------------------------------------------------------------------------------------------------------------------------


	//移動効果(オーバロード)-----------------------------------------------------------------------------------------------------------------------
	public void UseEffect( CardMain card, Square now_square, Square touch_square, string player ) { 
		CardMove( card, now_square, touch_square, player );
	}
	//---------------------------------------------------------------------------------------------------------------------------------------------

	
	//回復効果(オーバロード)----------------------------------------
	public void UseEffect( CardMain card ) {
		card._cardDates.hp += card._cardDates.effect_recovery_point;
		_active_point.DecreasePoint( card._cardDates.effect_ap );
	}
	//--------------------------------------------------------------


	//移動できる場所を事前に調べる関数-------------------------------------------------------------------------------------------------
	public List< Square > MovePossibleSquare ( CardMain card, Square now_square, string player ) { 
		List< Square > squares = new List< Square >( );

		//移動できるマスだけ格納
		for ( int i = 0; i < card._cardDates.directions.Length; i++ ) {
			Square square = _field.SquareInThatDirection( now_square, card._cardDates.directions[ i ], card._cardDates.distance );

			if ( square != null ) {
				if ( IsOnCardType( player, square ) ) {
					squares.Add( square );
				}
			}
		}

		return squares;
	}
	//----------------------------------------------------------------------------------------------------------------------------------


	//攻撃するマスにカードがあるマスを事前に調べる関数-----------------------------------------------------------------------------------
	public List< Square > AttackEffectPossibleOnCardSquare ( CardMain card, Square now_square ) { 
		List< Square > squares = new List< Square >( );

		//移動できるマスだけ格納
		for ( int i = 0; i < card._cardDates.directions.Length; i++ ) {
			Square square = _field.SquareInThatDirection( now_square, card._cardDates.directions[ i ], card._cardDates.distance );

			if ( square != null ) {
				if ( IsOnCard( square ) ) {
					squares.Add( square );
				}
			}
		}

		return squares;
	}
	//------------------------------------------------------------------------------------------------------------------------------------

	//マスにカードが存在するかどうか------
	bool IsOnCard( Square square ) { 
		if ( square.On_Card != null ) { 
			return true;	
		} else { 
			return false;	
		}
		
	}
	//------------------------------------

	//マスにカードがあったら自分のじゃないかを調べる関数-----------
	bool IsOnCardType( string player, Square square ) {
		if ( IsOnCard( square ) ) {
			CardMain on_card = square.On_Card;
			if ( on_card.gameObject.tag == player ) { 
				return false;
			} else {
				return true;	
			}
		}

		return true;
	}
	//----------------------------------------------------


	//カードの戦闘------------------------------------------------------------------------------------------------
	BATTLE_RESULT Battle( CardMain player_card, CardMain enemy_card ) { 
		player_card.Damage( enemy_card._cardDates.attack_point );
		enemy_card.Damage( player_card._cardDates.attack_point );

		if ( player_card._cardDates.hp == 0 && enemy_card._cardDates.hp == 0 ) return BATTLE_RESULT.BOTH_DEATH;	

		if ( enemy_card._cardDates.hp == 0 ) return BATTLE_RESULT.PLAYER_WIN;

		if ( player_card._cardDates.hp == 0 ) return BATTLE_RESULT.PLAYER_DEFEAT;

		if ( player_card._cardDates.hp > 0 && enemy_card._cardDates.hp > 0 ) return BATTLE_RESULT.BOTH_ALIVE;

		return BATTLE_RESULT.NOT_BATTLE;
	}
	//-----------------------------------------------------------------------------------------------------------
}


//カードが存在するか、カードのtypeが何か、事前に調べる関数は別クラスにしたほうがいいかも。ブリッジバターンとかいいかも？
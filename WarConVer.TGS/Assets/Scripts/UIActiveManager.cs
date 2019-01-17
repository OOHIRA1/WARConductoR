using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActiveManager : MonoBehaviour {

	public enum BUTTON { 
		BACK,	
		MOVE,
		DIRECT_ATTACK,
		EFFECT,
		EFFECT_YES,
		TURN_END_COLOR,
	}

	//const int MAX_ACTION_COUNT = 3;	
	const int FIRST_ROW_INDEX = 0;
	const int SQUARE_ROW_NUM = 4;

	[ SerializeField ] Field _field = null;

	//ボタン
	[ SerializeField ] List< GameObject > _UIButtons = new List< GameObject >( );
	[ SerializeField ] GameObject _turnEndButtonMono = null;

	MainSceneOperation _mainSceneOperation = new MainSceneOperation( );


	//全てのボタンの表示を切り替える---------------------
	public void AllButtonActiveChanger( bool active ) {
		for ( int i = 0; i < _UIButtons.Count; i++ ) {
			_UIButtons[ i ].SetActive( active );
			MonoAndColorSwap( ( BUTTON )i );
		}
	}
	//---------------------------------------------------
	
	
	//指定のボタンの表示を切り替える----------------------------------------------
	public void ButtonActiveChanger( bool active, params BUTTON[ ] buttons ) {
		for ( int i = 0; i < buttons.Length; i++ ) {
			_UIButtons[ ( int )buttons[ i ] ].SetActive( active );
			MonoAndColorSwap( buttons[ i ] );
		}
	}
	//----------------------------------------------------------------------------


	//フィールドカードの操作系UIの表示処理----------------------------------------------------------------------------
	public void ShowCardOperationUI( CardMain card, Participant turnPlayer, Square nowSquare ) {
		//自分のカードだったら
		if ( card.gameObject.tag == turnPlayer.gameObject.tag ) {
				ButtonActiveChanger( true, BUTTON.BACK );

				EffectButtonActiveConditions( card, turnPlayer, nowSquare );	
				MoveButtonActiveConditions( card, turnPlayer, nowSquare );
				DirectAttackButtonActiveConditions( card, turnPlayer, nowSquare );
		}

		//相手のカードだったら
		if ( card.gameObject.tag != turnPlayer.gameObject.tag ) {
			ButtonActiveChanger( true, BUTTON.BACK );
		}
	}
	//------------------------------------------------------------------------------------------------------------------

	
	//エフェクトボタンの表示処理------------------------------------------------------------------------------
	void EffectButtonActiveConditions( CardMain card, Participant turnPlayer, Square nowSquare ) { 
		//効果の種類によって処理を変える
		//効果ボタン表示条件
		switch ( card.Card_Data._effect_type ) { 
			case CardData.EFFECT_TYPE.ATTACK:	
				if ( turnPlayer.DecreaseActivePointConfirmation( card.Card_Data._necessaryAPForEffect ) && 
					 _field.AttackEffectPossibleOnCardSquare( card, nowSquare ).Count > 0 ) {

					ButtonActiveChanger( true, BUTTON.EFFECT );
				}
				break;

			case CardData.EFFECT_TYPE.MOVE:
				if ( turnPlayer.DecreaseActivePointConfirmation( card.Card_Data._necessaryAPForEffect ) && 
					 _field.MovePossibleSquare( card, nowSquare ).Count > 0 &&
					 card.Action_Count < card.MAX_ACTION_COUNT ) {

					ButtonActiveChanger( true, BUTTON.EFFECT );
				}
				break;

			case CardData.EFFECT_TYPE.RECOVERY:
				if ( turnPlayer.DecreaseActivePointConfirmation( card.Card_Data._necessaryAPForEffect ) && 
					 card.Card_Data._toughness < card.Card_Data._maxToughness ) {

					ButtonActiveChanger( true, BUTTON.EFFECT );
				}
				break;

			case CardData.EFFECT_TYPE.NO_EFFECT:
				break;

			default:
				Debug.Log( "想定していない効果がボタン表示に来ています" );
				break;
		}
	}
	//--------------------------------------------------------------------------------------------------------

	
	//移動ボタンの表示処理-----------------------------------------------------------------------------
	void MoveButtonActiveConditions( CardMain card, Participant turnPlayer, Square nowSquare ) {
		//APが消費する分あって移動できるマスがあったてまだ行動できるカードだったら
		if ( turnPlayer.DecreaseActivePointConfirmation( card.Card_Data._necessaryAP ) &&
			 _field.MovePossibleSquare( card, nowSquare ).Count > 0 &&
			 card.Action_Count < card.MAX_ACTION_COUNT ) {

			ButtonActiveChanger( true, BUTTON.MOVE );

		}
	}
	//-------------------------------------------------------------------------------------------------

	
	//攻撃ボタンの表示処理---------------------------------------------------------------------------------------------
	void DirectAttackButtonActiveConditions( CardMain card, Participant turnPlayer, Square nowSquare ) { 
		//消費するAPがあってまだ行動できるカードだったら
		if ( turnPlayer.DecreaseActivePointConfirmation( card.Card_Data._necessaryAP ) &&
			 card.Action_Count < card.MAX_ACTION_COUNT ) {

			if ( ( ( nowSquare.Index ) / SQUARE_ROW_NUM == FIRST_ROW_INDEX ) && card.gameObject.tag == "Player1" ) {		//一列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
				ButtonActiveChanger( true, BUTTON.DIRECT_ATTACK );
			}

			//if ( ( ( _nowSquare.Index ) / SQUARE_ROW_NUM == FIFTH_ROW_INDEX ) && _card.gameObject.tag == "Player2" ) {		//五列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
			//	_directAttackButton.SetActive( true );
			//}
		}
	}
	//-------------------------------------------------------------------------------------------------------------------

	
	//カラーボタンとモノクロボタンを入れ替える---------------
	void MonoAndColorSwap( BUTTON button ) { 
		if ( button == BUTTON.TURN_END_COLOR  ) {				//カラーボタンが表示されていたらモノクロボタンを非表示にする。
			if ( _UIButtons[ ( int )button ].activeInHierarchy ) {		//カラーボタンが非表示だったらモノクロボタンを表示する。
				_turnEndButtonMono.SetActive( false );
			} else { 
				_turnEndButtonMono.SetActive( true );	
			}
		}
	}
	//-------------------------------------------------------
}

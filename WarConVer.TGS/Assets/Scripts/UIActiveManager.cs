using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActiveManager : MonoBehaviour {
	//const int MAX_ACTION_COUNT = 3;	
	const int FIRST_ROW_INDEX = 0;
	const int SQUARE_ROW_NUM = 4;

	public enum BUTTON { 
		BACK,	
		MOVE,
		DIRECTATTACK,
		EFFECT,
		EFFECT_YES,
		TURNEND,
	}

	//ボタン
	[ SerializeField ] List< GameObject > _UIButtons = new List< GameObject >( );

	MainSceneOperation _mainSceneOperation = new MainSceneOperation( );

	public void AllButtonActiveChanger( bool active ) {
		for ( int i = 0; i < _UIButtons.Count; i++ ) { 
			_UIButtons[ i ].SetActive( active );	
		}
	}
	
	public void ButtonActiveChanger( bool active, params BUTTON[ ] buttons ) {
		for ( int i = 0; i < buttons.Length; i++ ) {
			_UIButtons[ ( int )buttons[ i ] ].SetActive( active );
		}
	}


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


	void EffectButtonActiveConditions( CardMain card, Participant turnPlayer, Square nowSquare ) { 
		//効果の種類によって処理を変える
		//効果ボタン表示条件
		switch ( card._cardDates.effect_type ) { 
			case CardData.EFFECT_TYPE.ATTACK:	
				if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.effect_ap ) && 
					 turnPlayer.AttackEffectPossibleOnCardSquare( card, nowSquare ).Count > 0 ) {

					ButtonActiveChanger( true, BUTTON.EFFECT );
				}
				break;

			case CardData.EFFECT_TYPE.MOVE:
				if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.effect_ap ) && 
					 turnPlayer.MovePossibleSquare( card, nowSquare ).Count > 0 &&
					 card._cardDates.actionCount < card.MAX_ACTION_COUNT ) {

					ButtonActiveChanger( true, BUTTON.EFFECT );
				}
				break;

			case CardData.EFFECT_TYPE.RECOVERY:
				if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.effect_ap ) && 
					 card._cardDates.hp < card._cardDates.max_hp ) {

					ButtonActiveChanger( true, BUTTON.EFFECT );
				}
				break;

			default:
				Debug.Log( "想定していない効果がボタン表示に来ています" );
				break;
		}
	}


	void MoveButtonActiveConditions( CardMain card, Participant turnPlayer, Square nowSquare ) {
		//APが消費する分あって移動できるマスがあったてまだ行動できるカードだったら
		if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.move_ap ) &&
			 turnPlayer.MovePossibleSquare( card, nowSquare ).Count > 0 &&
			 card._cardDates.actionCount < card.MAX_ACTION_COUNT ) {

			ButtonActiveChanger( true, BUTTON.MOVE );

		}
	}

	void DirectAttackButtonActiveConditions( CardMain card, Participant turnPlayer, Square nowSquare ) { 
		//消費するAPがあってまだ行動できるカードだったら
		if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.move_ap ) &&
			 card._cardDates.actionCount < card.MAX_ACTION_COUNT ) {

			if ( ( ( nowSquare.Index ) / SQUARE_ROW_NUM == FIRST_ROW_INDEX ) && card.gameObject.tag == "Player1" ) {		//一列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
				ButtonActiveChanger( true, BUTTON.DIRECTATTACK );
			}

			//if ( ( ( _nowSquare.Index ) / SQUARE_ROW_NUM == FIFTH_ROW_INDEX ) && _card.gameObject.tag == "Player2" ) {		//五列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
			//	_directAttackButton.SetActive( true );
			//}
		}
	}
}

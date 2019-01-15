using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
	const int MAX_ACTION_COUNT = 3;							//移動回数の最大値
	const int SQUARE_ROW_NUM = 4;
	const int FIRST_ROW_INDEX = 0;
	const int FIFTH_ROW_INDEX = 4;

	//ボタン
	[ SerializeField ]GameObject _returnButton	   = null;
	[ SerializeField ]GameObject _moveButton		   = null;
	[ SerializeField ]GameObject _directAttackButton = null;
	[ SerializeField ]GameObject _effectButton	   = null;
	[ SerializeField ]GameObject _effectYesBuuton	   = null;
	[ SerializeField ]GameObject _turnEndButton	   = null;
	MainSceneOperation _mainSceneOperation = new MainSceneOperation( );


	//フィールドカードの操作系UIの表示処理----------------------------------------------------------------------------
	void ShowCardOperationUI( CardMain card, Participant turnPlayer, Square nowSquare ) { 
		if ( card.gameObject.tag == turnPlayer.gameObject.tag ) {
				_returnButton.SetActive( true );

				//効果の種類によって処理を変える
				switch ( card._cardDates.effect_type ) { 
					case CardMain.EFFECT_TYPE.ATTACK:	
						if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.effect_ap ) && 
							 turnPlayer.AttackEffectPossibleOnCardSquare( card, nowSquare ).Count > 0 ) {

							_effectButton.SetActive( true );

						}
						break;

					case CardMain.EFFECT_TYPE.MOVE:
						if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.effect_ap ) && 
							 turnPlayer.MovePossibleSquare( card, nowSquare ).Count > 0 &&
							 card._cardDates.actionCount < MAX_ACTION_COUNT ) {

							_effectButton.SetActive( true );

						}
						break;

					case CardMain.EFFECT_TYPE.RECOVERY:
						if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.effect_ap ) && 
							 card._cardDates.hp < card._cardDates.max_hp ) {

							_effectButton.SetActive( true );

						}
						break;

					default:
						Debug.Log( "想定していない効果がボタン表示に来ています" );
						break;
				}

				//APが消費する分あって移動できるマスがあったてまだ行動できるカードだったら
				if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.move_ap ) &&
					 turnPlayer.MovePossibleSquare( card, nowSquare ).Count > 0 &&
					 card._cardDates.actionCount < MAX_ACTION_COUNT ) {

					_moveButton.SetActive( true );

				}

				//消費するAPがあってまだ行動できるカードだったら
				if ( turnPlayer.DecreaseActivePointConfirmation( card._cardDates.move_ap ) &&
					 card._cardDates.actionCount < MAX_ACTION_COUNT ) {

					if ( ( ( nowSquare.Index ) / SQUARE_ROW_NUM == FIRST_ROW_INDEX ) && card.gameObject.tag == "Player1" ) {		//一列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
						_directAttackButton.SetActive( true );
					}

					//if ( ( ( _nowSquare.Index ) / SQUARE_ROW_NUM == FIFTH_ROW_INDEX ) && _card.gameObject.tag == "Player2" ) {		//五列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
					//	_directAttackButton.SetActive( true );
					//}
				}

		}

		if ( card.gameObject.tag == "Player2" ) { 
			_returnButton.SetActive( true );
		}
		//if ( _card.gameObject.tag == _enemyPlayer.gameObject.tag ) { 
		//	_returnButton.SetActive( true );
		//}
	}
	//------------------------------------------------------------------------------------------------------------------

	//ボタン操作処理------------------------------------------------------------
	MainPhase.MAIN_PHASE_STATUS CardDetailsStatus( CardMain card ) {

		//戻るボタンを押したら
		if ( _mainSceneOperation.BackButtonClicked( ) ) { 
			_returnButton.SetActive( false );
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_effectButton.SetActive( false );
			return MainPhase.MAIN_PHASE_STATUS.IDLE;
			//_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			//_card.DeleteCardDetail( );
			//_card = null;
			//_nowSquare = null;
			//return;
		}

		//移動ボタンを押したら
		if ( _mainSceneOperation.MoveButtonClicked( ) ) { 
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_effectButton.SetActive( false );
			return MainPhase.MAIN_PHASE_STATUS.CARD_MOVE;
			//_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_MOVE;
			//_card.DeleteCardDetail( );
			//return;
		}

		//攻撃ボタンを押したら
		if ( _mainSceneOperation.AttackButtonClicked( ) ) {
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_returnButton.SetActive( false );
			_effectButton.SetActive( false );
			return MainPhase.MAIN_PHASE_STATUS.DIRECT_ATTACK;
			//_mainPhaseStatus = MAIN_PHASE_STATUS.DIRECT_ATTACK;
			//_card.DeleteCardDetail( );
			//return;
		}

		//効果ボタンを押したら
		if ( _mainSceneOperation.EffectButtonClicked( ) ) { 
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_effectButton.SetActive( false );

			//効果の種類によって処理を変える
			switch ( card._cardDates.effect_type ) { 
				case CardMain.EFFECT_TYPE.ATTACK:	
					_effectYesBuuton.SetActive( true );
					return MainPhase.MAIN_PHASE_STATUS.EFFECT_ATTACK;
					//_mainPhaseStatus = MAIN_PHASE_STATUS.EFFECT_ATTACK;
					//break;

				case CardMain.EFFECT_TYPE.MOVE:
					return MainPhase.MAIN_PHASE_STATUS.EFFECT_MOVE;
					//_mainPhaseStatus = MAIN_PHASE_STATUS.EFFECT_MOVE;
					//break;

				case CardMain.EFFECT_TYPE.RECOVERY:
					_effectYesBuuton.SetActive( true );
					return MainPhase.MAIN_PHASE_STATUS.EEFECT_RECOVERY;
					//_mainPhaseStatus = MAIN_PHASE_STATUS.EEFECT_RECOVERY;
					//break;

				default:
					Debug.Log( "想定していない効果がステータスに来ています" );
					break;
			}

			//_card.DeleteCardDetail( );
			//return;
		}

		return MainPhase.MAIN_PHASE_STATUS.CARD_DETAILS;
	}
	//--------------------------------------------------------------------------------

}

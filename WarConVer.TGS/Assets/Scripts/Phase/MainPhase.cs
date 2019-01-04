using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPhase : Phase {
	enum MAIN_PHASE_STATUS { 
		IDLE,
		CARD_MOVE,
		CARD_DETAILS,
		DIRECT_ATTACK,
		EFFECT_ATTACK,
		EFFECT_MOVE,
		EEFECT_RECOVERY,
		CARD_SUMMON,
	}
	
	const int MAX_ACTION_COUNT = 3;							//移動回数の最大値
	const int LOSE_CEMETARY_POINT = 10;
	const int SQUARE_ROW_NUM = 4;
	const int FIRST_ROW_INDEX = 0;
	const int FIFTH_ROW_INDEX = 4;

	MainSceneOperation _mainSceneOperation = null;
	Square _nowSquare					   = null;
	CardMain _card						   = null;
	CardMain _handCard					   = null;
	Participant _enemyPlayer			   = null;
	RayShooter _rayShooter				   = new RayShooter( );
	Vector3 _handCardPos				   = Vector3.zero;
	MAIN_PHASE_STATUS _mainPhaseStatus	   = MAIN_PHASE_STATUS.IDLE;

	//ボタン
	GameObject _returnButton	   = null;
	GameObject _moveButton		   = null;
	GameObject _directAttackButton = null;
	GameObject _effectButton	   = null;
	GameObject _effectYesBuuton	   = null;
	GameObject _turnEndButton	   = null;

	//テスト用
	CardMain _drawCard = null;


	public MainPhase( Participant turnPlayer, Participant enemyPlayer, MainSceneOperation mainSceneQperation, 
					  GameObject returnButton, GameObject moveButton, GameObject directAttackButton, GameObject effectButton, GameObject effectYesButton, GameObject turnEndButton, 
					  CardMain drawCard ) {

		_turnPlayer = turnPlayer;
		_enemyPlayer = enemyPlayer;
		_mainSceneOperation = mainSceneQperation;

		_returnButton = returnButton;
		_moveButton = moveButton;
		_directAttackButton = directAttackButton;
		_effectButton = effectButton;
		_effectYesBuuton = effectYesButton;
		_turnEndButton = turnEndButton;

		_drawCard = drawCard;

		Debug.Log( _turnPlayer.gameObject.tag + "メインフェーズ" );
	}


	public override void PhaseUpdate( ) {

		LoseTerms( );
		ActiveTurnEndButton( );
		switch ( _mainPhaseStatus ) {
			case MAIN_PHASE_STATUS.IDLE:
				IdleStatis( );
				return;

			case MAIN_PHASE_STATUS.CARD_DETAILS:
				CardDetailsStatus( );
				return;

			case MAIN_PHASE_STATUS.CARD_MOVE:
				MoveCardStatus( );
				return;

			case MAIN_PHASE_STATUS.DIRECT_ATTACK:
				DirectAttack( );
				return;

			case MAIN_PHASE_STATUS.EFFECT_ATTACK:
				AttackEffect( );
				return;

			case MAIN_PHASE_STATUS.EFFECT_MOVE:
				MoveEffect( );
				return;

			case MAIN_PHASE_STATUS.EEFECT_RECOVERY:
				RecoveryEffect( );
				return;

			case MAIN_PHASE_STATUS.CARD_SUMMON:
				SummonStatus( );
				return;

			default:
				Debug.Log( "存在しないステータスです" );
				return;
		}
	}
	

	public override bool IsNextPhaseFlag( ) {
		if ( _mainSceneOperation.TurnEndButtonClicked( ) ) { 
			_turnEndButton.SetActive( false );	
			return true;
		}

		return false; 	
	}


	//待機状態-----------------------------------------
	void IdleStatis( ) {
		if ( _mainSceneOperation.MouseTouch( ) ) {
			FieldCardTouch( );
			HandCardTouch( );

			//ここに手札のカードを何かしらの条件で詳細を表示するを書く？
		}
	}
	//--------------------------------------------------

	
	//フィールドのカードがタッチされたら処理------------------------
	void FieldCardTouch( ) { 
		//カードとそのマスの取得
		_nowSquare = _rayShooter.RayCastSquare( );
		if ( _nowSquare == null ) return;
		_card = _nowSquare.On_Card;
		if ( _card == null ) return;
		
		ShowCardDetails( );	//カードの詳細画像の表示
		ShowCardOperationUI( );	//プレイヤーによって表示するUI
		
		_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_DETAILS;
	}
	//---------------------------------------------------------------


	//手札のカードがタッチされたら処理-------------------------------------------
	void HandCardTouch( ) { 
		_handCard = _rayShooter.RayCastHandCard( _turnPlayer.gameObject.tag );
		if ( _handCard == null ) return;

		_handCardPos = _handCard.transform.position;
		_handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = false;
		_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_SUMMON;
	}
	//---------------------------------------------------------------------------


	//フィールドカードの詳細画像表示----------------------------------------------------------------------------------------------------
	void ShowCardDetails( ) { 
		_card.ShowCardDetail( );
	}
	//---------------------------------------------------------------------------------------------------------------------------


	//フィールドカードの操作系UIの表示処理----------------------------------------------------------------------------
	void ShowCardOperationUI( ) { 
		if ( _card.gameObject.tag == _turnPlayer.gameObject.tag ) {
				_returnButton.SetActive( true );

				//効果の種類によって処理を変える
				switch ( _card._cardDates.effect_type ) { 
					case CardMain.EFFECT_TYPE.ATTACK:	
						if ( _turnPlayer.DecreaseActivePointConfirmation( _card._cardDates.effect_ap ) && 
							 _turnPlayer.AttackEffectPossibleOnCardSquare( _card, _nowSquare ).Count > 0 ) {

							_effectButton.SetActive( true );

						}
						break;

					case CardMain.EFFECT_TYPE.MOVE:
						if ( _turnPlayer.DecreaseActivePointConfirmation( _card._cardDates.effect_ap ) && 
							 _turnPlayer.MovePossibleSquare( _card, _nowSquare ).Count > 0 &&
							 _card._cardDates.actionCount < MAX_ACTION_COUNT ) {

							_effectButton.SetActive( true );

						}
						break;

					case CardMain.EFFECT_TYPE.RECOVERY:
						if ( _turnPlayer.DecreaseActivePointConfirmation( _card._cardDates.effect_ap ) && 
							 _card._cardDates.hp < _card._cardDates.max_hp ) {

							_effectButton.SetActive( true );

						}
						break;

					default:
						Debug.Log( "想定していない効果がボタン表示に来ています" );
						break;
				}

				//APが消費する分あって移動できるマスがあったてまだ行動できるカードだったら
				if ( _turnPlayer.DecreaseActivePointConfirmation( _card._cardDates.move_ap ) &&
					 _turnPlayer.MovePossibleSquare( _card, _nowSquare ).Count > 0 &&
					 _card._cardDates.actionCount < MAX_ACTION_COUNT ) {

					_moveButton.SetActive( true );

				}

				//消費するAPがあってまだ行動できるカードだったら
				if ( _turnPlayer.DecreaseActivePointConfirmation( _card._cardDates.move_ap ) &&
					 _card._cardDates.actionCount < MAX_ACTION_COUNT ) {

					if ( ( ( _nowSquare.Index - 1 ) / SQUARE_ROW_NUM == FIRST_ROW_INDEX ) && _card.gameObject.tag == "Player1" ) {		//一列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
						_directAttackButton.SetActive( true );
					}

					if ( ( ( _nowSquare.Index - 1 ) / SQUARE_ROW_NUM == FIFTH_ROW_INDEX ) && _card.gameObject.tag == "Player2" ) {		//五列目にいたら攻撃ボタンを表示//修正するだろうからマジックナンバーを放置
						_directAttackButton.SetActive( true );
					}
				}

		}

		if ( _card.gameObject.tag == _enemyPlayer.gameObject.tag ) { 
			_returnButton.SetActive( true );
		}
	}
	//------------------------------------------------------------------------------------------------------------------

	
	//ボタン操作処理------------------------------------------------------------
	void CardDetailsStatus( ) { 

		//戻るボタンを押したら
		if ( _mainSceneOperation.BackButtonClicked( ) ) { 
			_returnButton.SetActive( false );
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_effectButton.SetActive( false );
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			_card.DeleteCardDetail( );
			_card = null;
			_nowSquare = null;
			return;
		}

		//移動ボタンを押したら
		if ( _mainSceneOperation.MoveButtonClicked( ) ) { 
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_effectButton.SetActive( false );
			_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_MOVE;
			_card.DeleteCardDetail( );
			return;
		}

		//攻撃ボタンを押したら
		if ( _mainSceneOperation.AttackButtonClicked( ) ) {
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_returnButton.SetActive( false );
			_effectButton.SetActive( false );
			_mainPhaseStatus = MAIN_PHASE_STATUS.DIRECT_ATTACK;
			_card.DeleteCardDetail( );
			return;
		}

		//効果ボタンを押したら
		if ( _mainSceneOperation.EffectButtonClicked( ) ) { 
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_effectButton.SetActive( false );

			//効果の種類によって処理を変える
			switch ( _card._cardDates.effect_type ) { 
				case CardMain.EFFECT_TYPE.ATTACK:	
					_mainPhaseStatus = MAIN_PHASE_STATUS.EFFECT_ATTACK;
					_effectYesBuuton.SetActive( true );
					break;

				case CardMain.EFFECT_TYPE.MOVE:
					_mainPhaseStatus = MAIN_PHASE_STATUS.EFFECT_MOVE;
					break;

				case CardMain.EFFECT_TYPE.RECOVERY:
					_mainPhaseStatus = MAIN_PHASE_STATUS.EEFECT_RECOVERY;
					_effectYesBuuton.SetActive( true );
					break;

				default:
					Debug.Log( "想定していない効果がステータスに来ています" );
					break;
			}

			_card.DeleteCardDetail( );
			return;
		}
	}
	//--------------------------------------------------------------------------------

	
	//カード移動状態処理----------------------------------------------------------------------------------------------------------------
	void MoveCardStatus( ) {
		List< Square > squares = _turnPlayer.MovePossibleSquare( _card, _nowSquare );

		_turnPlayer.SquareChangeColor( squares, true );		//移動できるマスの色を変える

		if ( _mainSceneOperation.MouseTouch( ) ) {
			Square square = _rayShooter.RayCastSquare( );	//マウスをクリックしたらレイを飛ばしてマスを取得する
			_turnPlayer.SquareChangeColor( squares, false );	//色をもとに戻す
			_returnButton.SetActive( false );

			if ( square != null ) {
				_turnPlayer.MoveCard( _card, _nowSquare, square );	//移動できるかどうかを判定し移動できたら移動する
			}

			//情報リセット
			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}

		//戻るボタンを押したら
		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_returnButton.SetActive( false );
			_turnPlayer.SquareChangeColor( squares, false );	//色をもとに戻す

			//情報リセット
			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------------------------------------------------------------------


	//ダイレクトアタック処理----------------------------------------------------
	void DirectAttack( ) { 
			_turnPlayer.DirectAttack( _enemyPlayer, _card._cardDates.move_ap );
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			_card = null;
			_nowSquare = null;
			return;
	}
	//--------------------------------------------------------------------------

	
	//召喚状態処理----------------------------------------------------------------------
	void SummonStatus( ) {
		_handCard.transform.position = _mainSceneOperation.getWorldMousePos( );
		List< Square > squares = _turnPlayer.SummonSquare( _turnPlayer.gameObject.tag );

		if ( _turnPlayer.DecreaseMPointConfirmation( _handCard._cardDates.mp ) ) { 
			_turnPlayer.SquareChangeColor( squares, true );
		}

		if ( !_mainSceneOperation.MouseConsecutivelyTouch( ) ) {
			if ( !_turnPlayer.DecreaseMPointConfirmation( _handCard._cardDates.mp ) ) {
				_handCard.transform.position = _handCardPos;
				_handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
				_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
				return;
			}

			Square square = _rayShooter.RayCastSquare( );

			if ( square == null ) {
				_turnPlayer.SquareChangeColor( squares, false );
				_handCard.transform.position = _handCardPos;
				_handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
				_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
				return;
			}

			for ( int i = 0; i < squares.Count; i++ ) { 
				if ( square.Index != squares[ i ].Index ) continue;

				_turnPlayer.SquareChangeColor( squares, false );
				_turnPlayer.Summon( _handCard, square, _turnPlayer.gameObject.tag );
				_handCard = null;
				_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
				return;
			}



			_turnPlayer.SquareChangeColor( squares, false );
			_handCard.transform.position = _handCardPos;
			_handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
		}
	}
	//--------------------------------------------------------------------------------------


	//攻撃効果中処理------------------------------------------------------------------------------
	void AttackEffect( ) {
		List< Square > squares = _turnPlayer.AttackEffectPossibleOnCardSquare( _card, _nowSquare );
		_turnPlayer.SquareChangeColor( squares, true );

		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_turnPlayer.SquareChangeColor( squares, false );
			_returnButton.SetActive( false );
			_effectYesBuuton.SetActive( false );

			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		if ( _mainSceneOperation.EffectYesButtonClicked( ) ) {
			_turnPlayer.SquareChangeColor( squares, false );
			_returnButton.SetActive( false );
			_effectYesBuuton.SetActive( false );
			_turnPlayer.UseEffect( _card, _nowSquare );

			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
	}
	//--------------------------------------------------------------------------------------------


	//移動効果中処理---------------------------------------------------------------------------------
	void MoveEffect( ) {
		List< Square > squares = _turnPlayer.MovePossibleSquare( _card, _nowSquare );

		_turnPlayer.SquareChangeColor( squares, true );

		if ( _mainSceneOperation.MouseTouch( ) ) {
			Square square = _rayShooter.RayCastSquare( );		//マウスをクリックしたらレイを飛ばしてマスを取得する
			_turnPlayer.SquareChangeColor( squares, false );	//色をもとに戻す
			_returnButton.SetActive( false );

			if ( square != null ) {
				_turnPlayer.UseEffect( _card, _nowSquare, square );	//移動できるかどうかを判定し移動できたら移動する
			}
			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}

		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_turnPlayer.SquareChangeColor( squares, false );
			_returnButton.SetActive( false );

			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//---------------------------------------------------------------------------------------------------


	//回復効果中処理----------------------------------------------
	void RecoveryEffect( ) {
		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_returnButton.SetActive( false );
			_effectYesBuuton.SetActive( false );

			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		if ( _mainSceneOperation.EffectYesButtonClicked( ) ) {
			_returnButton.SetActive( false );
			_effectYesBuuton.SetActive( false );
			_turnPlayer.UseEffect( _card );

			_card = null;
			_nowSquare = null;

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------

	
	void ActiveTurnEndButton( ) { 
		if ( _mainPhaseStatus == MAIN_PHASE_STATUS.IDLE && !_turnEndButton.activeInHierarchy ) { 
			_turnEndButton.SetActive( true );	
		}

		if ( _mainPhaseStatus != MAIN_PHASE_STATUS.IDLE ) { 
			_turnEndButton.SetActive( false );	
		}
	}


	void LoseTerms( ) { 
		if ( _enemyPlayer.Lefe_Point <= 0 ) { 
			_enemyPlayer.Lose_Flag = true;
		}

		if ( _enemyPlayer.Cemetary_Point >= LOSE_CEMETARY_POINT ) { 
			_enemyPlayer.Lose_Flag = true;	
		}

		if ( _turnPlayer.Cemetary_Point >= LOSE_CEMETARY_POINT ) { 
			_turnPlayer.Lose_Flag = true;	
		}
	}
}


//ボタンの参照とって表示を切り替えたりする場所はどこ？
//多分マスを連打しまくると計算やばそう

//MediatorパターンyやObserverパターンなどを使って(多分意味的にはMediatorパターン？)ボタンやマウスが押されたら通知してもらってそのときだけそれに対するupdateをする、
//などをすると無駄な処理が減るかも？しかし、相互参照になるため要相談
//例えば移動ボタンが押されたときに通知してさらにカード詳細状態だったら処理する。逆にマウスがクリックされたときにカード詳細状態だったら処理しないなど

//今現在、普通によくないプログラム。あっちこっち変更しないといけなくて可読性もよくない。条件分岐もありすぎてごり押し感半端ない

//手札のカードの詳細も見れたほうがいいが判定どうしよう？

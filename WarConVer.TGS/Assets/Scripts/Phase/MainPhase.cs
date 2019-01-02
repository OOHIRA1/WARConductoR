﻿using System.Collections;
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

	MainSceneOperation _mainSceneOperation = null;
	
	MAIN_PHASE_STATUS _mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
	Square _nowSquare    = null;
	RayShooter _rayShooter = new RayShooter( );
	CardMain _handCard = null;
	Participant _enemyPlayer = null;
	Vector3 _handCardPos = Vector3.zero;

	//ボタン
	GameObject _returnButton	   = null;
	GameObject _moveButton		   = null;
	GameObject _directAttackButton = null;
	GameObject _effectButton	   = null;
	GameObject _effectYesBuuton	   = null;
	GameObject _turnEndButton	   = null;

	//詳細系
	GameObject _cardDetailsImage   = null;	//生成する詳細画像のプレハブ
	GameObject _canvas			   = null;	//生成したあとに子にするため
	GameObject _details			   = null;	//生成した詳細画像を格納するため


	//テスト用
	CardMain _card	   = null;
	CardMain _drawCard = null;


	public MainPhase( Participant turnPlayer, Participant enemyPlayer, MainSceneOperation mainSceneQperation, 
					  GameObject returnButton, GameObject moveButton, GameObject directAttackButton, GameObject effectButton, GameObject effectYesButton, GameObject turnEndButton, 
					  GameObject cardDitailsImage, GameObject canvas,
					  Square nowSquare, CardMain card, CardMain drawCard ) {

		_turnPlayer = turnPlayer;
		_enemyPlayer = enemyPlayer;
		_nowSquare = nowSquare;
		_mainSceneOperation = mainSceneQperation;

		_returnButton = returnButton;
		_moveButton = moveButton;
		_directAttackButton = directAttackButton;
		_effectButton = effectButton;
		_effectYesBuuton = effectYesButton;
		_turnEndButton = turnEndButton;

		_cardDetailsImage = cardDitailsImage;
		_canvas = canvas;

		_card = card;
		_drawCard = drawCard;

		Debug.Log( "メインフェーズ" );
	}


	public override void PhaseUpdate( ) {
		TestDraw( );

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
		
		ShowCardDetails( );	//カードの詳細画像の表示(生成)
		ShowCardOperationUI( );	//プレイヤーによって表示するUI
		
		_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_DETAILS;
	}
	//---------------------------------------------------------------


	//手札のカードがタッチされたら処理-------------------------------------------
	void HandCardTouch( ) { 
		_handCard = _rayShooter.RayCastHandCard( );
		if ( _handCard == null ) return;

		_handCardPos = _handCard.transform.position;
		_handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = false;
		_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_SUMMON;
	}
	//---------------------------------------------------------------------------


	//フィールドカードの詳細画像表示----------------------------------------------------------------------------------------------------
	void ShowCardDetails( ) { 
		//カードの詳細プレハブの取得
		_details = Object.Instantiate( _cardDetailsImage, Vector3.zero, Quaternion.identity );
		Text attackPoint = _details.transform.Find( "Attack_Point_Background/Attack_Point" ).GetComponent< Text >( );
		Text hitPoint = _details.transform.Find( "Hit_Point_Background/Hit_Point" ).GetComponent< Text >( );

		//画像などの情報読み込み
		_details.GetComponent< Image >( ).sprite = _card.Card_Sprite_Renderer.sprite;
		attackPoint.text = _card._cardDates.attack_point.ToString( );
		hitPoint.text = _card._cardDates.hp.ToString( );

		//位置をずらしている
		_details.transform.parent = _canvas.transform;
		RectTransform detailsPos = _details.GetComponent< RectTransform >( );
		detailsPos.localPosition = new Vector3( -210, 0, 0 );	//あとでこの部分の処理は修正するだろうからマジックナンバーを放置
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
							 _turnPlayer.MovePossibleSquare( _card, _nowSquare ).Count > 0 ) {
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

				//APが消費する分あって移動できるマスがあったら
				if ( _turnPlayer.DecreaseActivePointConfirmation( _card._cardDates.move_ap ) && 
					 _turnPlayer.MovePossibleSquare( _card, _nowSquare ).Count > 0 ) {

					_moveButton.SetActive( true );

					if ( ( _nowSquare.Index - 1 ) / 4 == 0 ) {								//一列目にいたら//修正するだろうからマジックナンバーを放置
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
			Object.Destroy( _details );
			_details = null;
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
			Object.Destroy( _details );
			_details = null;
			return;
		}

		//攻撃ボタンを押したら
		if ( _mainSceneOperation.AttackButtonClicked( ) ) {
			_moveButton.SetActive( false );
			_directAttackButton.SetActive( false );
			_returnButton.SetActive( false );
			_effectButton.SetActive( false );
			_mainPhaseStatus = MAIN_PHASE_STATUS.DIRECT_ATTACK;
			Object.Destroy( _details );
			_details = null;
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

			Object.Destroy( _details );
			_details = null;
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
				if ( square.Index == squares[ i ].Index ) { 
					_turnPlayer.SquareChangeColor( squares, false );
					_turnPlayer.Summon( _handCard, square, _turnPlayer.gameObject.tag );
					_handCard = null;
					_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
					return;
				}
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


	void TestDraw( ) { 
		if ( Input.GetKeyDown( KeyCode.D ) ) { 
			_turnPlayer.Draw( _drawCard );	
		}	
	}
}


//ボタンの参照とって表示を切り替えたりする場所はどこ？
//詳細画像を生成して画像を入れたり動かしたりする場所はどこ？
//多分マスを連打しまくると計算やばそう

//MediatorパターンyやObserverパターンなどを使って(多分意味的にはMediatorパターン？)ボタンやマウスが押されたら通知してもらってそのときだけそれに対するupdateをする、
//などをすると無駄な処理が減るかも？しかし、相互参照になるため要相談
//例えば移動ボタンが押されたときに通知してさらにカード詳細状態だったら処理する。逆にマウスがクリックされたときにカード詳細状態だったら処理しないなど

//今現在、普通によくないプログラム。あっちこっち変更しないといけなくて可読性もよくない。条件分岐もありすぎてごり押し感半端ない

//今のやり方だと自分の今操作状態のプレイヤーを確認する必要がある(文字列でPlayer1とかやってるところをターンで変えらるようにする)

//手札のカードの詳細も見れたほうがいいが判定どうしよう？

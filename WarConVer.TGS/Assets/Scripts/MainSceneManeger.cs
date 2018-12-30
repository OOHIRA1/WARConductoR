﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManeger : MonoBehaviour {
	enum STATUS { 
		IDLE,
		CARD_MOVE,
		CARD_DETAILS,
		DIRECT_ATTACK,
		EFFECT_ATTACK,
		EFFECT_MOVE,
		EEFECT_RECOVERY,
		CARD_SUMMON,
	}

	[ SerializeField ] MainSceneOperation _main_scene_operation = null;
	[ SerializeField ] Participant _player1 = null;
	[ SerializeField ] Participant _player2 = null;
	
	STATUS _status = STATUS.IDLE;
	RayShooter _ray_shooter = new RayShooter( );

	//ボタン
	[ SerializeField ] GameObject _return_button		= null;
	[ SerializeField ] GameObject _move_button			= null;
	[ SerializeField ] GameObject _direct_attack_button = null;
	[ SerializeField ] GameObject _effect_button		= null;
	[ SerializeField ] GameObject _effect_yes_buuton	= null;

	//詳細系
	[ SerializeField ] GameObject _card_details_image = null;	//生成する詳細画像のプレハブ
	[ SerializeField ] GameObject _canvas			  = null;	//生成したあとに子にするため
	GameObject _details = null;									//生成した詳細画像を格納するため

	CardMain _hand_card = null;
	Vector3 _hand_card_pos = Vector3.zero;

	//テスト用
	[ SerializeField ] Square _now_square    = null;
	[ SerializeField ] CardMain _card		 = null;
	[ SerializeField ] CardMain _enemy_card  = null;
	[ SerializeField ] Square _enemy_square  = null;
	[ SerializeField ] CardMain _draw_card   = null;


	void Start( ) {
		_now_square.On_Card = _card;
		_card.gameObject.transform.position = _now_square.transform.position;
		_enemy_card.transform.position = _enemy_square.transform.position;
		_enemy_square.On_Card = _enemy_card;

		if ( _main_scene_operation == null ) { 
			Debug.Log( "[エラー]MainSceneOperationが参照を取れていない" );	
		}

		if ( _player1 == null ) { 
			Debug.Log( "[エラー]Participant(Player1)が参照を取れていない" );	
		}

		if ( _player2 == null ) { 
			Debug.Log( "[エラー]Participant(Player2)が参照を取れていない" );	
		}
	}

	
	void Update( ) {
		if ( _main_scene_operation == null ) {
			return;
		}

		if ( _player1 == null ) { 
			return;
		}

		if ( _player2 == null ) { 
			return;
		}

		TestCemetary( );
		TestDraw( );
		switch ( _status ) {
			case STATUS.IDLE:
				IdleStatis( );
				return;

			case STATUS.CARD_DETAILS:
				CardDetailsStatus( );
				return;

			case STATUS.CARD_MOVE:
				CardMoveStatus( );
				return;

			case STATUS.DIRECT_ATTACK:
				DirectAttack( );
				return;

			case STATUS.EFFECT_ATTACK:
				AttackEffect( );
				return;

			case STATUS.EFFECT_MOVE:
				MoveEffect( );
				return;

			case STATUS.EEFECT_RECOVERY:
				RecoveryEffect( );
				return;

			case STATUS.CARD_SUMMON:
				SummonStatus( );
				return;

			default:
				Debug.Log( "存在しないステータスです" );
				return;
		}
	}
	


	//待機状態-----------------------------------------
	void IdleStatis( ) {

		if ( _main_scene_operation.MouseTouch( ) ) {
			FieldCardTouch( );
			HandCardTouch( );
		}

	}
	//--------------------------------------------------

	
	//フィールドのカードがタッチされたら処理------------------------
	void FieldCardTouch( ) { 
		//カードとそのマスの取得
		_now_square = _ray_shooter.RayCastSquare( );
		if ( _now_square == null ) return;
		_card = _now_square.On_Card;
		if ( _card == null ) return;
		
		ShowCardDetails( );	//カードの詳細画像の表示(生成)
		ShowCardOperationUI( );	//プレイヤーによって表示するUI
		
		_status = STATUS.CARD_DETAILS;
	}
	//---------------------------------------------------------------


	//手札のカードがタッチされたら処理-------------------------------------------
	void HandCardTouch( ) { 
		_hand_card = _ray_shooter.RayCastHandCard( );
		if ( _hand_card == null ) return;

		_hand_card_pos = _hand_card.transform.position;
		_hand_card.gameObject.GetComponent< BoxCollider2D >( ).enabled = false;
		_status = STATUS.CARD_SUMMON;
	}
	//---------------------------------------------------------------------------


	//フィールドカードの詳細画像表示----------------------------------------------------------------------------------------------------
	void ShowCardDetails( ) { 
		//カードの詳細プレハブの取得
		_details = Instantiate( _card_details_image, transform.position, Quaternion.identity );
		Text attack_point = _details.transform.Find( "Attack_Point_Background/Attack_Point" ).GetComponent< Text >( );
		Text hit_point = _details.transform.Find( "Hit_Point_Background/Hit_Point" ).GetComponent< Text >( );

		//画像などの情報読み込み
		_details.GetComponent< Image >( ).sprite = _card.Card_Sprite_Renderer.sprite;
		attack_point.text = _card._cardDates.attack_point.ToString( );
		hit_point.text = _card._cardDates.hp.ToString( );

		//位置をずらしている
		_details.transform.parent = _canvas.transform;
		RectTransform details_pos = _details.GetComponent< RectTransform >( );
		details_pos.localPosition = new Vector3( -210, 0, 0 );	//あとでこの部分の処理は修正するだろうからマジックナンバーを放置
	}
	//---------------------------------------------------------------------------------------------------------------------------


	//フィールドカードの操作系UIの表示処理----------------------------------------------------------------------------
	void ShowCardOperationUI( ) { 
		if ( _card.gameObject.tag == "Player1" ) {
				_return_button.SetActive( true );

				//効果の種類によって処理を変える
				switch ( _card._cardDates.effect_type ) { 
					case CardMain.EFFECT_TYPE.ATTACK:	
						if ( _player1.DecreaseActivePointConfirmation( _card._cardDates.effect_ap ) && 
							 _player1.AttackEffectPossibleOnCardSquare( _card, _now_square ).Count > 0 ) {
							_effect_button.SetActive( true );
						}
						break;

					case CardMain.EFFECT_TYPE.MOVE:
						if ( _player1.DecreaseActivePointConfirmation( _card._cardDates.effect_ap ) && 
							 _player1.MovePossibleSquare( _card, _now_square ).Count > 0 ) {
							_effect_button.SetActive( true );
						}
						break;

					case CardMain.EFFECT_TYPE.RECOVERY:
						if ( _player1.DecreaseActivePointConfirmation( _card._cardDates.effect_ap ) && 
							 _card._cardDates.hp < _card._cardDates.max_hp ) {
							_effect_button.SetActive( true );
						}
						break;

					default:
						Debug.Log( "想定していない効果がボタン表示に来ています" );
						break;
				}

				//APが消費する分あって移動できるマスがあったら
				if ( _player1.DecreaseActivePointConfirmation( _card._cardDates.move_ap ) && 
					 _player1.MovePossibleSquare( _card, _now_square ).Count > 0 ) {

					_move_button.SetActive( true );

					if ( ( _now_square.Index - 1 ) / 4 == 0 ) {								//一列目にいたら//修正するだろうからマジックナンバーを放置
						_direct_attack_button.SetActive( true );
					}

				}
			}

			if ( _card.gameObject.tag == "Player2" ) { 
				_return_button.SetActive( true );
			}
	}
	//------------------------------------------------------------------------------------------------------------------

	
	//ボタン操作処理------------------------------------------------------------
	void CardDetailsStatus( ) { 

		//戻るボタンを押したら
		if ( _main_scene_operation.ReturnButton( ) ) { 
			_return_button.SetActive( false );
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_effect_button.SetActive( false );
			_status = STATUS.IDLE;
			Destroy( _details );
			_details = null;
			_card = null;
			_now_square = null;
			return;
		}

		//移動ボタンを押したら
		if ( _main_scene_operation.MoveButton( ) ) { 
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_effect_button.SetActive( false );
			_status = STATUS.CARD_MOVE;
			Destroy( _details );
			_details = null;
			return;
		}

		//攻撃ボタンを押したら
		if ( _main_scene_operation.DirectAttackButton( ) ) {
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_return_button.SetActive( false );
			_effect_button.SetActive( false );
			_status = STATUS.DIRECT_ATTACK;
			Destroy( _details );
			_details = null;
			return;
		}

		//効果ボタンを押したら
		if ( _main_scene_operation.EffectButton( ) ) { 
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_effect_button.SetActive( false );

			//効果の種類によって処理を変える
			switch ( _card._cardDates.effect_type ) { 
				case CardMain.EFFECT_TYPE.ATTACK:	
					_status = STATUS.EFFECT_ATTACK;
					_effect_yes_buuton.SetActive( true );
					break;

				case CardMain.EFFECT_TYPE.MOVE:
					_status = STATUS.EFFECT_MOVE;
					break;

				case CardMain.EFFECT_TYPE.RECOVERY:
					_status = STATUS.EEFECT_RECOVERY;
					_effect_yes_buuton.SetActive( true );
					break;

				default:
					Debug.Log( "想定していない効果がステータスに来ています" );
					break;
			}

			Destroy( _details );
			_details = null;
			return;
		}
	}
	//--------------------------------------------------------------------------------

	
	//カード移動状態処理----------------------------------------------------------------------------------------------------------------
	void CardMoveStatus( ) {
		List< Square > squares = _player1.MovePossibleSquare( _card, _now_square );

		_player1.SquareChangeColor( squares, true );		//移動できるマスの色を変える

		if ( _main_scene_operation.MouseTouch( ) ) {
			Square square = _ray_shooter.RayCastSquare( );	//マウスをクリックしたらレイを飛ばしてマスを取得する
			_player1.SquareChangeColor( squares, false );	//色をもとに戻す
			_return_button.SetActive( false );

			if ( square != null ) {
				_player1.CardMove( _card, _now_square, square );	//移動できるかどうかを判定し移動できたら移動する
			}

			//情報リセット
			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}

		//戻るボタンを押したら
		if ( _main_scene_operation.ReturnButton( ) ) {
			_return_button.SetActive( false );
			_player1.SquareChangeColor( squares, false );	//色をもとに戻す

			//情報リセット
			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------------------------------------------------------------------


	//ダイレクトアタック処理----------------------------------------------------
	void DirectAttack( ) { 
		if ( _card.gameObject.tag == "Player1" ) { 
			_player1.DirectAttack( _player2, _card._cardDates.move_ap );
			_status = STATUS.IDLE;
			_card = null;
			_now_square = null;
			return;
		}

		if ( _card.gameObject.tag == "Player2" ) { 
			_player2.DirectAttack( _player1, _card._cardDates.move_ap );
			_status = STATUS.IDLE;
			_card = null;
			_now_square = null;
			return;
		}
	}
	//--------------------------------------------------------------------------

	
	//召喚状態処理----------------------------------------------------------------------
	void SummonStatus( ) {
		_hand_card.transform.position = _main_scene_operation.getWorldMousePos( );
		List< Square > squares = _player1.SummonSquare( "Player1" );

		if ( _player1.DecreaseMPointConfirmation( _hand_card._cardDates.mp ) ) { 
			_player1.SquareChangeColor( squares, true );
		}

		if ( !_main_scene_operation.MouseConsecutivelyTouch( ) ) {
			if ( !_player1.DecreaseMPointConfirmation( _hand_card._cardDates.mp ) ) {
				_hand_card.transform.position = _hand_card_pos;
				_hand_card.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
				_status = STATUS.IDLE;
				return;
			}

			Square square = _ray_shooter.RayCastSquare( );

			if ( square == null ) {
				_player1.SquareChangeColor( squares, false );
				_hand_card.transform.position = _hand_card_pos;
				_hand_card.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
				_status = STATUS.IDLE;
				return;
			}

			for ( int i = 0; i < squares.Count; i++ ) { 
				if ( square.Index == squares[ i ].Index ) { 
					_player1.SquareChangeColor( squares, false );
					_player1.Summon( _hand_card, square, "Player1" );
					_hand_card = null;
					_status = STATUS.IDLE;
					return;
				}
			}



			_player1.SquareChangeColor( squares, false );
			_hand_card.transform.position = _hand_card_pos;
			_hand_card.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
			_status = STATUS.IDLE;
		}
	}
	//--------------------------------------------------------------------------------------


	//攻撃効果中処理------------------------------------------------------------------------------
	void AttackEffect( ) {
		List< Square > squares = _player1.AttackEffectPossibleOnCardSquare( _card, _now_square );
		_player1.SquareChangeColor( squares, true );

		if ( _main_scene_operation.ReturnButton( ) ) {
			_player1.SquareChangeColor( squares, false );
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );

			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}
		
		if ( _main_scene_operation.EffectYesButton( ) ) {
			_player1.SquareChangeColor( squares, false );
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );
			_player1.UseEffect( _card, _now_square );

			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}
		
	}
	//--------------------------------------------------------------------------------------------


	//移動効果中処理---------------------------------------------------------------------------------
	void MoveEffect( ) {
		List< Square > squares = _player1.MovePossibleSquare( _card, _now_square );

		_player1.SquareChangeColor( squares, true );

		if ( _main_scene_operation.MouseTouch( ) ) {
			Square square = _ray_shooter.RayCastSquare( );		//マウスをクリックしたらレイを飛ばしてマスを取得する
			_player1.SquareChangeColor( squares, false );	//色をもとに戻す
			_return_button.SetActive( false );

			if ( square != null ) {
				_player1.UseEffect( _card, _now_square, square );	//移動できるかどうかを判定し移動できたら移動する
			}
			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}

		if ( _main_scene_operation.ReturnButton( ) ) {
			_player1.SquareChangeColor( squares, false );
			_return_button.SetActive( false );

			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}
	}
	//---------------------------------------------------------------------------------------------------


	//回復効果中処理----------------------------------------------
	void RecoveryEffect( ) {
		if ( _main_scene_operation.ReturnButton( ) ) {
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );

			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}
		
		if ( _main_scene_operation.EffectYesButton( ) ) {
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );
			_player1.UseEffect( _card );

			_card = null;
			_now_square = null;

			_status = STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------


	

	void TestCemetary( ) { 
		if ( Input.GetKeyDown( KeyCode.C ) ) { 
			_player1.TestCemetary( );	
		}	
	}

	void TestDraw( ) { 
		if ( Input.GetKeyDown( KeyCode.D ) ) { 
			_player1.Draw( _draw_card );	
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
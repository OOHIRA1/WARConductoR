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

	MainSceneOperation _main_scene_operation = null;
	
	MAIN_PHASE_STATUS _main_phase_status = MAIN_PHASE_STATUS.IDLE;
	RayShooter _ray_shooter = new RayShooter( );
	CardMain _hand_card = null;
	Vector3 _hand_card_pos = Vector3.zero;

	//ボタン
	GameObject _return_button		 = null;
	GameObject _move_button			 = null;
	GameObject _direct_attack_button = null;
	GameObject _effect_button		 = null;
	GameObject _effect_yes_buuton	 = null;

	//詳細系
	GameObject _card_details_image = null;	//生成する詳細画像のプレハブ
	GameObject _canvas			   = null;	//生成したあとに子にするため
	GameObject _details			   = null;	//生成した詳細画像を格納するため


	//テスト用
	[ SerializeField ] Square _now_square    = null;
	[ SerializeField ] CardMain _card		 = null;
	[ SerializeField ] CardMain _enemy_card  = null;
	[ SerializeField ] Square _enemy_square  = null;
	[ SerializeField ] CardMain _draw_card   = null;


	public MainPhase( Participant player1, Participant player2, MainSceneOperation main_scene_qperation, 
					  GameObject return_button, GameObject move_button, GameObject direct_attack_button, GameObject effect_button, GameObject effect_yes_button, 
					  GameObject card_ditails_image, GameObject canvas,
					  Square now_square, CardMain card, CardMain enemy_card, Square enemy_square, CardMain draw_card ) {

		_player1 = player1;
		_player2 = player2;
		_main_scene_operation = main_scene_qperation;

		_return_button = return_button;
		_move_button = move_button;
		_direct_attack_button = direct_attack_button;
		_effect_button = effect_button;
		_effect_yes_buuton = effect_yes_button;

		_card_details_image = card_ditails_image;
		_canvas = canvas;

		_now_square = now_square;
		_card = card;
		_enemy_card = enemy_card;
		_enemy_square = enemy_square;
		_draw_card = draw_card;

		Debug.Log( "メインフェーズ" );
	}


	public override void PhaseUpdate( ) {
		TestDraw( );
		switch ( _main_phase_status ) {
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
		
		_main_phase_status = MAIN_PHASE_STATUS.CARD_DETAILS;
	}
	//---------------------------------------------------------------


	//手札のカードがタッチされたら処理-------------------------------------------
	void HandCardTouch( ) { 
		_hand_card = _ray_shooter.RayCastHandCard( );
		if ( _hand_card == null ) return;

		_hand_card_pos = _hand_card.transform.position;
		_hand_card.gameObject.GetComponent< BoxCollider2D >( ).enabled = false;
		_main_phase_status = MAIN_PHASE_STATUS.CARD_SUMMON;
	}
	//---------------------------------------------------------------------------


	//フィールドカードの詳細画像表示----------------------------------------------------------------------------------------------------
	void ShowCardDetails( ) { 
		//カードの詳細プレハブの取得
		_details = Object.Instantiate( _card_details_image, Vector3.zero, Quaternion.identity );
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
		if ( _main_scene_operation.BackButtonClicked( ) ) { 
			_return_button.SetActive( false );
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_effect_button.SetActive( false );
			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			Object.Destroy( _details );
			_details = null;
			_card = null;
			_now_square = null;
			return;
		}

		//移動ボタンを押したら
		if ( _main_scene_operation.MoveButtonClicked( ) ) { 
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_effect_button.SetActive( false );
			_main_phase_status = MAIN_PHASE_STATUS.CARD_MOVE;
			Object.Destroy( _details );
			_details = null;
			return;
		}

		//攻撃ボタンを押したら
		if ( _main_scene_operation.AttackButtonClicked( ) ) {
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_return_button.SetActive( false );
			_effect_button.SetActive( false );
			_main_phase_status = MAIN_PHASE_STATUS.DIRECT_ATTACK;
			Object.Destroy( _details );
			_details = null;
			return;
		}

		//効果ボタンを押したら
		if ( _main_scene_operation.EffectButtonClicked( ) ) { 
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_effect_button.SetActive( false );

			//効果の種類によって処理を変える
			switch ( _card._cardDates.effect_type ) { 
				case CardMain.EFFECT_TYPE.ATTACK:	
					_main_phase_status = MAIN_PHASE_STATUS.EFFECT_ATTACK;
					_effect_yes_buuton.SetActive( true );
					break;

				case CardMain.EFFECT_TYPE.MOVE:
					_main_phase_status = MAIN_PHASE_STATUS.EFFECT_MOVE;
					break;

				case CardMain.EFFECT_TYPE.RECOVERY:
					_main_phase_status = MAIN_PHASE_STATUS.EEFECT_RECOVERY;
					_effect_yes_buuton.SetActive( true );
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
		List< Square > squares = _player1.MovePossibleSquare( _card, _now_square );

		_player1.SquareChangeColor( squares, true );		//移動できるマスの色を変える

		if ( _main_scene_operation.MouseTouch( ) ) {
			Square square = _ray_shooter.RayCastSquare( );	//マウスをクリックしたらレイを飛ばしてマスを取得する
			_player1.SquareChangeColor( squares, false );	//色をもとに戻す
			_return_button.SetActive( false );

			if ( square != null ) {
				_player1.MoveCard( _card, _now_square, square );	//移動できるかどうかを判定し移動できたら移動する
			}

			//情報リセット
			_card = null;
			_now_square = null;

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			return;
		}

		//戻るボタンを押したら
		if ( _main_scene_operation.BackButtonClicked( ) ) {
			_return_button.SetActive( false );
			_player1.SquareChangeColor( squares, false );	//色をもとに戻す

			//情報リセット
			_card = null;
			_now_square = null;

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------------------------------------------------------------------


	//ダイレクトアタック処理----------------------------------------------------
	void DirectAttack( ) { 
		if ( _card.gameObject.tag == "Player1" ) { 
			_player1.DirectAttack( _player2, _card._cardDates.move_ap );
			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			_card = null;
			_now_square = null;
			return;
		}

		if ( _card.gameObject.tag == "Player2" ) { 
			_player2.DirectAttack( _player1, _card._cardDates.move_ap );
			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
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
				_main_phase_status = MAIN_PHASE_STATUS.IDLE;
				return;
			}

			Square square = _ray_shooter.RayCastSquare( );

			if ( square == null ) {
				_player1.SquareChangeColor( squares, false );
				_hand_card.transform.position = _hand_card_pos;
				_hand_card.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
				_main_phase_status = MAIN_PHASE_STATUS.IDLE;
				return;
			}

			for ( int i = 0; i < squares.Count; i++ ) { 
				if ( square.Index == squares[ i ].Index ) { 
					_player1.SquareChangeColor( squares, false );
					_player1.Summon( _hand_card, square, "Player1" );
					_hand_card = null;
					_main_phase_status = MAIN_PHASE_STATUS.IDLE;
					return;
				}
			}



			_player1.SquareChangeColor( squares, false );
			_hand_card.transform.position = _hand_card_pos;
			_hand_card.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
		}
	}
	//--------------------------------------------------------------------------------------


	//攻撃効果中処理------------------------------------------------------------------------------
	void AttackEffect( ) {
		List< Square > squares = _player1.AttackEffectPossibleOnCardSquare( _card, _now_square );
		_player1.SquareChangeColor( squares, true );

		if ( _main_scene_operation.BackButtonClicked( ) ) {
			_player1.SquareChangeColor( squares, false );
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );

			_card = null;
			_now_square = null;

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		if ( _main_scene_operation.EffectYesButtonClicked( ) ) {
			_player1.SquareChangeColor( squares, false );
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );
			_player1.UseEffect( _card, _now_square );

			_card = null;
			_now_square = null;

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
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

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			return;
		}

		if ( _main_scene_operation.BackButtonClicked( ) ) {
			_player1.SquareChangeColor( squares, false );
			_return_button.SetActive( false );

			_card = null;
			_now_square = null;

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//---------------------------------------------------------------------------------------------------


	//回復効果中処理----------------------------------------------
	void RecoveryEffect( ) {
		if ( _main_scene_operation.BackButtonClicked( ) ) {
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );

			_card = null;
			_now_square = null;

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		if ( _main_scene_operation.EffectYesButtonClicked( ) ) {
			_return_button.SetActive( false );
			_effect_yes_buuton.SetActive( false );
			_player1.UseEffect( _card );

			_card = null;
			_now_square = null;

			_main_phase_status = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------


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

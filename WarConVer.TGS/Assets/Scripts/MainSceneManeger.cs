using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManeger : MonoBehaviour {
	enum STATUS { 
		IDLE,
		CARD_MOVE,
		CARD_DETAILS,
		DIRECT_ATTACK,
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

	//詳細系
	[ SerializeField ] GameObject _card_details_image = null;	//生成する詳細画像のプレハブ
	[ SerializeField ] GameObject _canvas			  = null;	//生成したあとに子にするため
	GameObject _details = null;									//生成した詳細画像を格納するため

	//テスト用
	[ SerializeField ] Square _now_square = null;
	[ SerializeField ] CardMain _card = null;


	void Start( ) {
		_now_square.On_Card = _card;
		_card.gameObject.transform.position = _now_square.transform.position;
		_card = null;
		_now_square = null;

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

		testCardDamage( );
		switch ( _status ) {
			case STATUS.IDLE:
				CardDetailsPrint( );
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

		}
	}
	
	//カード移動状態
	void CardMoveStatus( ) {
		_player1.SquareChangeColor( _now_square, _card._cardDates.directions, _card._cardDates.distance, true );		//移動できるマスの色を変える

		if ( _main_scene_operation.MouseTouch( ) ) {
			Square square = _ray_shooter.RayCastSquare( );	//マウスをクリックしたらレイを飛ばしてマスを取得する
			_player1.CardMove( _card, _now_square, _card._cardDates.directions,_card._cardDates.distance, square, _card._cardDates.move_ap );	//移動できるかどうかを判定し移動できたら移動する
			_player1.SquareChangeColor( _now_square, _card._cardDates.directions, _card._cardDates.distance, false );	//色をもとに戻す
			_return_button.SetActive( false );

			//情報リセット-------
			_card = null;
			_now_square = null;
			//-------------------

			_status = STATUS.IDLE;
			return;
		}

		//戻るボタンを押したら-----------------------------------------------------------------------------------------
		if ( _main_scene_operation.ReturnButton( ) ) {
			_return_button.SetActive( false );
			_player1.SquareChangeColor( _now_square, _card._cardDates.directions, _card._cardDates.distance, false );	//色をもとに戻す

			//情報リセット-----------
			_card = null;
			_now_square = null;
			//-------------------

			_status = STATUS.IDLE;
			return;
		}
		//--------------------------------------------------------------------------------------------------------------
	}

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

	void CardDetailsPrint( ) {

		if ( _main_scene_operation.MouseTouch( ) ) {

			//カードとそのマスの取得---------------------------------------------------
			_now_square = _ray_shooter.RayCastSquare( );
			if ( _now_square == null ) return;
			_card = _now_square.On_Card;
			if ( _card == null ) return;
			//--------------------------------------------------------------------------

			//カードの詳細画像の表示(生成)-------------------------------------------------------------------------------------------------
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
			//------------------------------------------------------------------------------------------------------------------------------

			//プレイヤーによって表示するUI-------------------------------------------------------
			if ( _card.gameObject.tag == "Player1" ) {
				_return_button.SetActive( true );
				if ( _player1.DecreaseActivePointConfirmation( _card._cardDates.move_ap ) ) {	//APが消費する分あったら
					_move_button.SetActive( true );
					if ( ( _now_square.Index - 1 ) / 4 == 0 ) {								//一列目にいたら//修正するだろうからマジックナンバーを放置
						_direct_attack_button.SetActive( true );
					}
				}
			}

			if ( _card.gameObject.tag == "Player2" ) { 
				_return_button.SetActive( true );
			}
			//-------------------------------------------------------------------------------------

			_status = STATUS.CARD_DETAILS;
		}
	}

	void CardDetailsStatus( ) { 

		//戻るボタンを押したら--------------------------
		if ( _main_scene_operation.ReturnButton( ) ) { 
			_return_button.SetActive( false );
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_status = STATUS.IDLE;
			Destroy( _details );
			_details = null;
			_card = null;
			_now_square = null;
			return;
		}
		//----------------------------------------------

		//移動ボタンを押したら--------------------------
		if ( _main_scene_operation.MoveButton( ) ) { 
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_status = STATUS.CARD_MOVE;
			Destroy( _details );
			_details = null;
			return;
		}
		//-----------------------------------------------

		//攻撃ボタンを押したら--------------------------
		if ( _main_scene_operation.DirectAttackButton( ) ) {
			_move_button.SetActive( false );
			_direct_attack_button.SetActive( false );
			_return_button.SetActive( false );
			_status = STATUS.DIRECT_ATTACK;
			Destroy( _details );
			_details = null;
			return;
		}
		//-----------------------------------------------
	}

	void testCardDamage( ) { 
		if ( Input.GetKeyDown( KeyCode.A ) ) { 
			if ( _card == null ) return;
				_card._cardDates.hp--;
			if ( _card._cardDates.hp < 0 ) _card._cardDates.hp = 0;
			
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
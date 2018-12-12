using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManeger : MonoBehaviour {
	enum STATUS { 
		IDLE,
		CARD_MOVE,
		CARD_DETAILS,
	}

	[ SerializeField ] MainSceneOperation _main_scene_operation = null;
	[ SerializeField ] Participant _player = null;
	[ SerializeField ] Camera _camera = null;
	
	//ボタン
	[ SerializeField ] GameObject _return_button = null;
	[ SerializeField ] GameObject _move_button = null;

	GameObject _details = null;									//生成した詳細画像を格納するため

	[ SerializeField ] GameObject _card_details_image = null;	//生成する詳細画像のプレハブ
	[ SerializeField ] GameObject _canvas = null;				//生成したあとに子にするため

	STATUS _status = STATUS.IDLE;

	//テスト用
	[ SerializeField ] Square _now_square = null;
	[ SerializeField ] CardMain _card = null;

	void Start( ) {
		_now_square.On_Card = _card;
		_card.gameObject.transform.position = _now_square.transform.position;

		if ( _main_scene_operation == null ) { 
			Debug.Log( "[エラー]MainSceneOperationが参照を取れていない" );	
		}

		if ( _player == null ) { 
			Debug.Log( "[エラー]Participant(Player)が参照を取れていない" );	
		}

		if ( _camera == null ) {
			Debug.Log( "[エラー]Cameraが参照を取れていない" );
		}
	}
	
	void Update( ) {
		if ( _main_scene_operation == null ) {
			return;
		}

		if ( _player == null ) { 
			return;
		}

		if ( _camera == null ) {
			return;
		}

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
		}
	}
	
	//カード移動状態
	void CardMoveStatus( ) {
		_player.SquareChangeColor( _now_square, _card._cardDates.directions, _card._cardDates.distance, true );		//移動できるマスの色を変える

		if ( _main_scene_operation.MouseTouch( ) ) {
			//マウスをクリックしたらレイを飛ばしてマスを取得する--------
			RaycastHit2D hit = _main_scene_operation.RayCastSquare( );
			if ( hit.collider == null ) return;
			if ( hit.collider.gameObject.tag != "Square" ) return;

			Square square = hit.collider.gameObject.GetComponent< Square >( );
			Debug.Log( hit.collider.gameObject.ToString( ) );
			//----------------------------------------------------------

			//移動できるかどうかを判定し移動できたら移動する
			_player.CardMove( _card, _now_square, _card._cardDates.directions,_card._cardDates.distance, square );


			_player.SquareChangeColor( _now_square, _card._cardDates.directions, _card._cardDates.distance, false );	//色をもとに戻す
			_return_button.gameObject.SetActive( false );
			_card = null;
			_now_square = null;
			_status = STATUS.IDLE;
			return;
		}

		//戻るボタンを押したら-----------------------------------------------------------------------------------------
		if ( _main_scene_operation.ReturnButton( ) ) {
			_return_button.gameObject.SetActive( false );
			_player.SquareChangeColor( _now_square, _card._cardDates.directions, _card._cardDates.distance, false );	//色をもとに戻す

			_card = null;
			_now_square = null;
			_status = STATUS.IDLE;
			return;
		}
		//--------------------------------------------------------------------------------------------------------------
	}

	void CardDetailsPrint( ) {

		if ( _main_scene_operation.MouseTouch( ) ) {

			//カードとそのマスの取得---------------------------------------------------
			RaycastHit2D hit_square = _main_scene_operation.RayCastSquare( );
			if ( hit_square.collider == null ) return;
			if ( hit_square.collider.gameObject.tag != "Square" ) return;

			_now_square = hit_square.collider.gameObject.GetComponent< Square >( );
			_card = _now_square.On_Card;
			if ( _card == null ) return;
			Debug.Log( hit_square.collider.gameObject.ToString( ) );
			//--------------------------------------------------------------------------

			//カードの詳細画像の表示(生成)---------------------------------------------------------
			_details = Instantiate( _card_details_image, transform.position, Quaternion.identity );
			_details.GetComponent< Image >( ).sprite = _card.Card_Sprite_Renderer.sprite;
			_details.transform.parent = _canvas.transform;
			RectTransform details_pos = _details.GetComponent< RectTransform >( );
			details_pos.localPosition = new Vector3( -210, 0, 0 );
			//-------------------------------------------------------------------------------------

			if ( _card.gameObject.tag == "Player1" ) {
				_return_button.gameObject.SetActive( true );
				_move_button.gameObject.SetActive( true );
			}

			if ( _card.gameObject.tag == "Player2" ) { 
				_return_button.gameObject.SetActive( true );
			}

			_status = STATUS.CARD_DETAILS;
		}
	}

	void CardDetailsStatus( ) { 

		//戻るボタンを押したら--------------------------
		if ( _main_scene_operation.ReturnButton( ) ) { 
			_return_button.gameObject.SetActive( false );
			_move_button.gameObject.SetActive( false );
			Destroy( _details );
			_details = null;
			_status = STATUS.IDLE;
			return;
		}
		//----------------------------------------------

		//移動ボタンを押したら--------------------------
		if ( _main_scene_operation.MoveButton( ) ) { 
			_move_button.gameObject.SetActive( false );
			Destroy( _details );
			_details = null;
			_status = STATUS.CARD_MOVE;
			return;
		}
		//-----------------------------------------------
	}
}

//ボタンの参照とって表示を切り替えたりする場所はどこ？
//詳細画像を生成して画像を入れたり動かしたりする場所はどこ？
//レイを飛ばすのはどこ？
//多分マスを連打しまくると計算やばそう
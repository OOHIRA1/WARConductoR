using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManeger : MonoBehaviour {
	[ SerializeField ] MainSceneOperation _main_scene_operation = null;
	[ SerializeField ] Participant _player = null;
	[ SerializeField ] Camera _camera = null;

	//テスト用
	[ SerializeField ] GameObject _card = null;
	[ SerializeField ] Square _now_square = null;
	[ SerializeField ] int _distans = 0;
	[ SerializeField ] Field.DIRECTION[ ] _directions = new Field.DIRECTION[ 1 ];

	void Start( ) {
		_card.transform.position = _now_square.transform.position;

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



		if ( _main_scene_operation.MouseTouch( ) ) {
			Vector3 mouse_pos = Input.mousePosition;
			//mouse_pos.z = 10f;
			Vector3 world_pos = Camera.main.ScreenToWorldPoint( mouse_pos );		//マウスのScreen座標をWorld座標に変換
			Debug.DrawRay( world_pos, new Vector3( 0,0,100 ), Color.red, 999f, false );
			RaycastHit2D hit = Physics2D.Raycast( world_pos, new Vector3( 0,0,100 ), LayerMask.NameToLayer( "Square" ) );	//クリックされた場所から真っすぐにRawを飛ばす
			if ( hit.collider == null ) return;
			if ( hit.collider.gameObject.tag == "Square" ) { 
				Debug.Log( hit.collider.gameObject.ToString( ) );
				Square square = hit.collider.gameObject.GetComponent< Square >( );
				_player.MoveCard( _card, _now_square, _directions, _distans, square );
			}

		}
	}
}

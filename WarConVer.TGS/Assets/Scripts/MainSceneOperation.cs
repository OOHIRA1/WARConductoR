using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneOperation : MonoBehaviour {
	const float RAY_DIR = 100f;
	bool _return_button = false; 
	bool _move_button = false;

	public bool MouseTouch( ) {
		if ( Input.GetMouseButtonDown( 0 ) ) {
			return true;
		} else { 
			return false;	
		}
	}

	//戻るボタン判定--------------------------------
	//戻るボタンが押されたかどうかの判定
	public bool ReturnButton( ) {
		if ( _return_button ) { 
			_return_button = false;
			return true;
		} else { 
			return false;	
		}
	}

	//戻るボタンがクリックされたら呼ぶ関数
	public void ClickReturnButton( ) { 
		_return_button = true;
	}
	//----------------------------------------------

	//動くボタン判定--------------------------------
	//動くボタンが押されたかどうかの判定
	public bool MoveButton( ) {
		if ( _move_button ) { 
			_move_button = false;
			return true;
		} else { 
			return false;	
		}
	}

	//動くボタンがクリックされたら呼ぶ関数
	public void ClickMoveButton( ) { 
		_move_button = true;
	}
	//----------------------------------------------


	public RaycastHit2D RayCastSquare( ) {
		Vector3 mouse_pos = Input.mousePosition;
		//mouse_pos.z = 10f;
		Vector3 world_pos = Camera.main.ScreenToWorldPoint( mouse_pos );		//マウスのScreen座標をWorld座標に変換

		Debug.DrawRay( world_pos, new Vector3( 0,0,RAY_DIR ), Color.red, 999f, false );
		return Physics2D.Raycast( world_pos, new Vector3( 0,0,RAY_DIR ), LayerMask.NameToLayer( "Square" ) );	//クリックされた場所から真っすぐにRawを飛ばす
	}
}
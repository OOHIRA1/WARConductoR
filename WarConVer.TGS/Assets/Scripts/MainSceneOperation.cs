using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneOperation : MonoBehaviour {
	const float RAY_DIR = 100f;
	bool _backButtonClicked 	 = false; 
	bool _moveButtonClicked      = false;
	bool _attackButtonClicked    = false;
	bool _effectButtonClicked    = false;
	bool _effectYesButtonClicked = false;
	bool _turnEndButtonClicked   = false;


	public bool MouseTouch( ) {
		if ( Input.GetMouseButtonDown( 0 ) ) {
			return true;
		} else { 
			return false;	
		}
	}


	public bool MouseConsecutivelyTouch( ) { 
		if ( Input.GetMouseButton( 0 ) ) { 
			return true;	
		} else { 
			return false;	
		}
	}


	public Vector3 getWorldMousePos( ) { 
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint( mousePos );		//マウスのScreen座標をWorld座標に変換
		worldPos.z = 0;
		return worldPos;
	}

	public Vector3 getWorldMousePos( Camera turnPlayerCamera ) { 
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = turnPlayerCamera.ScreenToWorldPoint( mousePos );		//マウスのScreen座標をWorld座標に変換
		worldPos.z = 0;
		return worldPos;
	}


	//戻るボタン判定--------------------------------
	//戻るボタンが押されたかどうかの判定
	public bool BackButtonClicked( ) {
		if ( _backButtonClicked  ) { 
			_backButtonClicked  = false;
			return true;
		} else { 
			return false;
		}
	}

	//戻るボタンがクリックされたら呼ぶ関数
	public void ClickReturnButton( ) { 
		_backButtonClicked  = true;
	}
	//----------------------------------------------


	//動くボタン判定--------------------------------
	//動くボタンが押されたかどうかの判定
	public bool MoveButtonClicked( ) {
		if ( _moveButtonClicked ) { 
			_moveButtonClicked = false;
			return true;
		} else { 
			return false;	
		}
	}

	//動くボタンがクリックされたら呼ぶ関数
	public void ClickMoveButton( ) {
		_moveButtonClicked = true;
	}
	//----------------------------------------------


	//攻撃ボタン判定--------------------------------
	//攻撃ボタンが押されたかどうかの判定
	public bool AttackButtonClicked( ) {
		if ( _attackButtonClicked  ) { 
			_attackButtonClicked  = false;
			return true;
		} else { 
			return false;	
		}
	}

	//動くボタンがクリックされたら呼ぶ関数
	public void ClickDirectAttackButton( ) {
		_attackButtonClicked  = true;
	}
	//----------------------------------------------


	//効果ボタン判定--------------------------------
	//効果ボタンが押されたかどうかの判定
	public bool EffectButtonClicked( ) {
		if ( _effectButtonClicked  ) { 
			_effectButtonClicked  = false;
			return true;
		} else { 
			return false;	
		}
	}

	//効果ボタンがクリックされたら呼ぶ関数
	public void ClickEffectButton( ) {
		_effectButtonClicked  = true;
	}
	//----------------------------------------------


	//効果了承ボタン判定--------------------------------
	//効果了承ボタンが押されたかどうかの判定
	public bool EffectYesButtonClicked( ) {
		if ( _effectYesButtonClicked ) { 
			_effectYesButtonClicked = false;
			return true;
		} else { 
			return false;	
		}
	}

	//効果了承ボタンがクリックされたら呼ぶ関数
	public void ClickEffectYesButton( ) {
		_effectYesButtonClicked = true;
	}
	//----------------------------------------------


	//ターンエンドボタン判定--------------------------------
	//ターンエンドボタンが押されたかどうかの判定
	public bool TurnEndButtonClicked( ) {
		if ( _turnEndButtonClicked ) { 
			_turnEndButtonClicked = false;
			return true;
		} else { 
			return false;	
		}
	}

	//ターンエンドボタンがクリックされたら呼ぶ関数
	public void ClickTurnEndButto( ) {
		_turnEndButtonClicked = true;
	}
	//----------------------------------------------

}
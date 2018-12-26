using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneOperation : MonoBehaviour {
	const float RAY_DIR = 100f;
	bool _return_button		   = false; 
	bool _move_button		   = false;
	bool _direct_attack_button = false;
	bool _effect_button		   = false;
	bool _effect_yes_button	   = false;

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


	//攻撃ボタン判定--------------------------------
	//攻撃ボタンが押されたかどうかの判定
	public bool DirectAttackButton( ) {
		if ( _direct_attack_button ) { 
			_direct_attack_button = false;
			return true;
		} else { 
			return false;	
		}
	}

	//動くボタンがクリックされたら呼ぶ関数
	public void ClickDirectAttackButton( ) {
		_direct_attack_button = true;
	}
	//----------------------------------------------


	//効果ボタン判定--------------------------------
	//効果ボタンが押されたかどうかの判定
	public bool EffectButton( ) {
		if ( _effect_button ) { 
			_effect_button = false;
			return true;
		} else { 
			return false;	
		}
	}

	//効果ボタンがクリックされたら呼ぶ関数
	public void ClickEffectButton( ) {
		_effect_button = true;
	}
	//----------------------------------------------

	//効果了承ボタン判定--------------------------------
	//効果了承ボタンが押されたかどうかの判定
	public bool EffectYesButton( ) {
		if ( _effect_yes_button ) { 
			_effect_yes_button = false;
			return true;
		} else { 
			return false;	
		}
	}

	//効果了承ボタンがクリックされたら呼ぶ関数
	public void ClickEffectYesButton( ) {
		_effect_yes_button = true;
	}
	//----------------------------------------------

}
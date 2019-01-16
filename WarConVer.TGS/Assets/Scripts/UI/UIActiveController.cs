using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//==UI系のアクティブ化を管理するクラス
//
//==使用方法：UI系のアクティブ化を行うオブジェクトにアタッチ
public class UIActiveController : MonoBehaviour {
	public enum UI_TYPE {
		MOVE_BUTTON,
		EFFECT_BUTTON,
		BACK_BUTTON,
		ATTACK_BUTTON,
		TURN_END_BUTTON,
		MULLIGAN_PANEL
	}

	[ SerializeField ] Button _moveButton = null;
	[ SerializeField ] Button _effectButton = null;
	[ SerializeField ] Button _backButton = null;
	[ SerializeField ] Button _attackButton = null;
	[ SerializeField ] Button _turnEndButton = null;
	[ SerializeField ] GameObject _mulliganPanel = null;


	//========================================================
	//public関数

	//--指定したボタンのアクティブかどうかを変更する関数
	public void ChangeActive( UI_TYPE userInterface, bool isActive ) {
		switch (userInterface) {
		case UI_TYPE.MOVE_BUTTON:
			_moveButton.interactable = isActive;
			_moveButton.gameObject.SetActive (isActive);
			break;
		case UI_TYPE.EFFECT_BUTTON:
			_effectButton.interactable = isActive;
			_effectButton.gameObject.SetActive (isActive);
			break;
		case UI_TYPE.BACK_BUTTON:
			_backButton.interactable = isActive;
			_backButton.gameObject.SetActive (isActive);
			break;
		case UI_TYPE.ATTACK_BUTTON:
			_attackButton.interactable = isActive;
			_attackButton.gameObject.SetActive (isActive);
			break;
		case UI_TYPE.TURN_END_BUTTON:
			_turnEndButton.interactable = isActive;
			_turnEndButton.gameObject.SetActive (isActive);
			break;
		case UI_TYPE.MULLIGAN_PANEL:
			//_mulliganPanel内のボタンのアクティブ化変更処理-----------------------------
			Button[ ] yesNoButtons = _mulliganPanel.GetComponentsInChildren<Button> ();
			for (int i = 0; i < yesNoButtons.Length; i++) {
				yesNoButtons [ i ].interactable = isActive;
			}
			//-------------------------------------------------------------------------
			_mulliganPanel.SetActive (isActive);
			break;
		default:
			break;
		}
	}
	//========================================================
	//========================================================
}

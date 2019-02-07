using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//==デッキ選択シーン管理クラス
//
//==使用方法：常にアクティブなゲームオブジェクトにアタッチ
public class DeckSelectSceneManager : MonoBehaviour {
	public enum DECK_TYPE {	//デッキの種類(配列の添字番号に使用)
		WHITE,
		BLACK
	}

	[ SerializeField ] SceneTransition _sceneTransition = null;
	[ SerializeField ] AudioSource _bgmSounder = null;
	bool _isWhiteDeckSelectButtonClicked;					//白デッキ選択ボタンを押したかどうかのフラグ
	bool _isBlackDeckSelectButtonClicked;					//黒デッキ選択ボタンを押したかどうかのフラグ

	[ SerializeField ] float _fadeOutTime = 3f;		//ボタンをタップしてからBGMが消えるまでの時間[単位：秒]
	[ SerializeField ] AutoDestroyEffect _tapEffect = null;
	[ SerializeField ] GameObject[ ] _deckSelectedButtons = null;	//デッキ選択ボタン配列
	[ SerializeField ] Deck[ ] _decks = null;
	[ SerializeField ] SelectedDeckData _selectedDeckData = null;
	[ SerializeField ] GameObject _confirmPanel = null;	//確認パネル
	[ SerializeField ] Image _confirmLogo = null;		//確認パネルのロゴ
	[ SerializeField ] Sprite[ ] _confirmLogoSprites = null;	//確認パネルで使用するSprite
	[ SerializeField ] GameObject _enterButton = null;
	[ SerializeField ] GameObject _cancelButton = null;
	bool _isEnterButtonClicked;		//確認パネル内「決定」ボタンを押したかどうかのフラグ
	bool _isCancelButtonClicked;	//確認パネル内「キャンセル」ボタンを押したかどうかのフラグ


	// Use this for initialization
	void Start () {
		_isWhiteDeckSelectButtonClicked = false;
		_isBlackDeckSelectButtonClicked = false;
		_isEnterButtonClicked = false;
		_isCancelButtonClicked = false;
		GameObject[] selectedDeckDataObjs = GameObject.FindGameObjectsWithTag ( "DeckData" );
		if (selectedDeckDataObjs.Length >= 2) {//2つ以上存在した場合
			for (int i = 0; i < selectedDeckDataObjs.Length; i++) {
				if (selectedDeckDataObjs [i].scene.name == "DontDestroyOnLoad") {
					_selectedDeckData = selectedDeckDataObjs [i].GetComponent< SelectedDeckData >( );
				}
			}
		} else {
			_selectedDeckData = selectedDeckDataObjs [0].GetComponent< SelectedDeckData >( );
		}
	}
	
	// Update is called once per frame
	void Update () {
		//タップエフェクト処理-------------------------------------------------------------------
		if( Input.GetMouseButtonDown(0) ) {
			Vector3 effectPos = Input.mousePosition;
			effectPos = Camera.main.ScreenToWorldPoint ( effectPos );
			effectPos.z = Camera.main.transform.position.z + 1f;//カメラに近い位置に生成したいため
			Instantiate<AutoDestroyEffect>( _tapEffect, effectPos, Quaternion.identity );
		}
		//--------------------------------------------------------------------------------------

		//デッキ選択ボタンクリック後処理----------------------------------------------------------
		if (_isWhiteDeckSelectButtonClicked) {//白デッキ選択
			for (int i = 0; i < _deckSelectedButtons.Length; i++) {
				_deckSelectedButtons [i].SetActive (false);
			}
			_selectedDeckData.USE_DECK_CARD_IDS = _decks[ (int)DECK_TYPE.WHITE ].CARD_IDS;
			_confirmLogo.sprite = _confirmLogoSprites [ (int)DECK_TYPE.WHITE ];
			_confirmPanel.SetActive (true);
		}
		if (_isBlackDeckSelectButtonClicked) {//黒デッキ選択
			for (int i = 0; i < _deckSelectedButtons.Length; i++) {
				_deckSelectedButtons [i].SetActive (false);
			}
			_selectedDeckData.USE_DECK_CARD_IDS = _decks[ (int)DECK_TYPE.BLACK ].CARD_IDS;
			_confirmLogo.sprite = _confirmLogoSprites [(int)DECK_TYPE.BLACK];
			_confirmPanel.SetActive (true);

		}
		//--------------------------------------------------------------------------------------


		//確認パネル内ボタンクリック後処理--------------------------------------------------------
		if ( _isEnterButtonClicked ) {
			//ボタンを反応しなくする処理-------------
			if (_enterButton.activeInHierarchy) {
				_enterButton.SetActive (false);
			}
			if (_cancelButton.activeInHierarchy) {
				_cancelButton.SetActive (false);
			}
			//--------------------------------------

			_bgmSounder.volume -= Time.deltaTime / _fadeOutTime;
			if (_bgmSounder.volume <= 0) {
				_sceneTransition.Transition ("Main");//メインシーンへ遷移
			}

		}
		if (_isCancelButtonClicked) {
			_confirmPanel.SetActive (false);
			for (int i = 0; i < _deckSelectedButtons.Length; i++) {
				_deckSelectedButtons [i].SetActive (true);
			}
			_isWhiteDeckSelectButtonClicked = false;
			_isBlackDeckSelectButtonClicked = false;
			_isCancelButtonClicked = false;
		}
		//--------------------------------------------------------------------------------------

	}


	//===================================================
	//public関数
	public void WhiteDeckSelectedButtonClicked( ) {
		_isWhiteDeckSelectButtonClicked = true;
	}


	public void BlackDeckSelectedButtonClicked( ) {
		_isBlackDeckSelectButtonClicked = true;
	}


	public void EnterButtonClicked( ) {
		_isEnterButtonClicked = true;
	}


	public void CancelButtonClicked( ) {
		_isCancelButtonClicked = true;
	}
	//===================================================
	//===================================================
}

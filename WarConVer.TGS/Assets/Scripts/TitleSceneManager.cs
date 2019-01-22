using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

//==タイトルシーン管理クラス
//
//==使用方法：常にアクティブなゲームオブジェクトにアタッチ
public class TitleSceneManager : MonoBehaviour {
	[ SerializeField ] SceneTransition _sceneTransition = null;
	[ SerializeField ] AudioSource _titleSounder = null;
	bool _isTapStartButtonClicked;					//[TapStart]ボタンを押したかどうかのフラグ
	[ SerializeField ] float _fadeOutTime = 3f;		//ボタンをタップしてからBGMが消えるまでの時間[単位：秒]
	[ SerializeField ] AutoDestroyEffect _tapEffect = null;
	[ SerializeField ] GameObject _tapSartButton = null;

	// Use this for initialization
	void Start () {
		_isTapStartButtonClicked = false;
	}
	
	// Update is called once per frame
	void Update () {

		//[TapStart]ボタンを押したときの処理-----------------------------
		if (_isTapStartButtonClicked) {
			if (_tapSartButton.activeInHierarchy) {
				_tapSartButton.SetActive (false);//ボタンを反応しなくする
			}

			_titleSounder.volume -= Time.deltaTime / _fadeOutTime;
			if (_titleSounder.volume <= 0) {
				_titleSounder.Stop ();
				_sceneTransition.Transition ( "Main" );//メインシーンへ遷移
			}
		}
		//--------------------------------------------------------------

		//タップエフェクト処理-------------------------------------------------------------------
		if( Input.GetMouseButtonDown(0) ) {
			Vector3 effectPos = Input.mousePosition;
			effectPos = Camera.main.ScreenToWorldPoint ( effectPos );
			effectPos.z = Camera.main.transform.position.z + 1f;//カメラに近い位置に生成したいため
			Instantiate<AutoDestroyEffect>( _tapEffect, effectPos, Quaternion.identity );
		}
		//--------------------------------------------------------------------------------------
	}


	//===================================================
	//public関数
	public void TapStartButtonClicked( ) {
		_isTapStartButtonClicked = true;
	}
	//===================================================
	//===================================================

}

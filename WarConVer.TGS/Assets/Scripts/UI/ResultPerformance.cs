﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//==勝敗演出を行う機能クラス
//
//==使用方法：常にアクティブなゲームオブジェクトにアタッチ
public class ResultPerformance : MonoBehaviour {
	public enum RESULT {	//_resultLogoSprite,配列の添字番号意味付けに使用
		WIN = 0,
		LOSE
	}

	[ SerializeField ] GameObject 	_resultPanel = null;
	[ SerializeField ] Image 	  	_resultLogo = null;
	[ SerializeField ] Sprite[ ]  	_resultLogoSprites = null;	//勝敗演出で使うSprite
	[ SerializeField ] AudioClip[ ] _seClips = null;			//勝敗演出で使うSE
	[ SerializeField ] AudioSource 	_bgmSounder = null;			//BGMを鳴らすAudioSource
	[ SerializeField ] float 	  	_fadeOutBGMTime = 2f;		//BGMがフェードアウトするまでの時間[単位：秒]
	AudioSource 				  	_resultSESounder;			//勝敗SEを鳴らすAudioSource
	bool 						  	_isStartPerforming;			//勝敗演出を開始したかどうかのフラグ


	// Use this for initialization
	void Start () {
		_resultSESounder = GetComponent<AudioSource> ();
		_isStartPerforming = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//--リザルト演出をする関数(コルーチン)
	public IEnumerator Perform( bool loseFlag ) {
		if (_isStartPerforming) yield break;

		_isStartPerforming = true;	//勝敗演出開始
		//BGMフェードアウト処理--------------------------------------------
		while (_bgmSounder.isPlaying) {
			if (_bgmSounder.volume > 0) {
				_bgmSounder.volume -= Time.deltaTime / _fadeOutBGMTime;
			} else {
				_bgmSounder.volume = 0;
				_bgmSounder.Stop ();
			}
			yield return null;	//BGMが鳴りやむまで待機
		}
		//----------------------------------------------------------------

		//リザルトUI表示・SE再生処理---------------------------------------
		if (loseFlag) {
			_resultLogo.sprite = _resultLogoSprites[ (int)RESULT.LOSE ];
			_resultSESounder.clip = _seClips[ (int)RESULT.LOSE ];
		} else {
			_resultLogo.sprite = _resultLogoSprites[ (int)RESULT.WIN ];
			_resultSESounder.clip = _seClips[ (int)RESULT.WIN ];
		}
		_resultPanel.SetActive ( true );
		_resultSESounder.Play ();
		//-----------------------------------------------------------------

	}


	//=============================================================================
	//public関数

	//--リザルト演出をする関数
	public void StartPerformCoroutine( bool loseFlag ) {
		StartCoroutine ( Perform( loseFlag ) );
	}
	//=============================================================================
	//=============================================================================

}

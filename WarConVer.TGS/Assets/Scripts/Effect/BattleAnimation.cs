using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==戦闘アニメーション機能クラス
//
//==使用方法：戦闘アニメーションを扱うGameObjectにアタッチ
public class BattleAnimation : MonoBehaviour {
	[ SerializeField ] Animator _animator = null;
	[ SerializeField ] AudioSource _audioSource = null;
	[ SerializeField ] bool _isAnimationFinished = false;

	//========================================
	//アクセッサ
	public bool Is_Animation_Finished{
		get { return _isAnimationFinished; }
	}
	//========================================
	//========================================


	// Update is called once per frame
	void Update () {

		int baseLayerIndex = _animator.GetLayerIndex ("Base Layer");
		AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo (baseLayerIndex);
		if (stateInfo.IsTag("battle") && stateInfo.normalizedTime > 1f) {//戦闘アニメーションが終了していたら
			_isAnimationFinished = true;
		}
	}


	//=======================================================
	//public関数

	//--左のカードが勝つアニメーションを再生する関数
	public void StartLeftWinAnim() {
		_animator.SetBool ("leftWinFlag", true);
		_audioSource.Play ();
	}

	//--右のカードが勝つアニメーションを再生する関数
	public void StartRightWinAnim() {
		_animator.SetBool ("rightWinFlag", true);
		_audioSource.Play ();
	}
	//=======================================================
	//=======================================================


}

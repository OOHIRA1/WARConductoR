using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==体力となる盾のアニメション機能
//
//==使用方法：体力となる盾の親オブジェクトにアタッチ
public class LifeSpaceAnimation : MonoBehaviour {
	//==単振動の情報を表す構造体======================================================
	[System.Serializable]
	public struct VibrationInfo {
		public float _amplitude;					//振幅
		public float _animationLengthForSeconds;	//アニメション長さ[単位：秒]
		public int _countOfVibration;				//振動回数
	};
	//===============================================================================

	[ SerializeField ] Transform		 _shieldTransform = null;						//盾のトランスフォーム
	[ SerializeField ] SpriteRenderer[ ] _shieldPieceSpriteRenderer = null;			//盾を分割したモノのSpriteRenderer
	[ SerializeField ] bool				 _isAnimationFinished = false;			//アニメーションが終わったかどうかを示すフラグ

	[ SerializeField ] VibrationInfo _vibrationInfo = new VibrationInfo();
	[ SerializeField ] float _waitTime = 2f;									//振動アニメーション後からアニメーション終了フラグが立つまでの時間[単位：秒]


	//===========================================
	//アクセッサ
	public bool Is_Animation_Finished {
		get { return _isAnimationFinished; }
		set { _isAnimationFinished = value; }
	}
	//===========================================
	//===========================================


	// Use this for initialization
	void Start () {
//		StartCoroutine (ShieldAnimation());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//--盾のアニメションをする関数(コルーチン)
	public IEnumerator ShieldAnimation () {
		float time = 0f;							//アニメーション経過時間
		Vector3 pos = _shieldTransform.position;	//盾の座標
		float angularVelocity;						//角速度
		int num = 0;
		while (time < _vibrationInfo._animationLengthForSeconds) {
			angularVelocity = 2 * Mathf.PI * _vibrationInfo._countOfVibration / _vibrationInfo._animationLengthForSeconds;		//角速度ω = θ / t で、θ = 2πn, t = t_max より ( ※n:振動回数, t_max:アニメーションの長さ )
			pos.y = _vibrationInfo._amplitude * Mathf.Sin ( angularVelocity * time );	//単振動の公式 y = Asin(ωt) より( ※A:振幅 )
			_shieldTransform.position = pos;

			time += Time.deltaTime;

			num = (int)( angularVelocity * time / (2 * Mathf.PI) );

			yield return null;
		}
		Debug.Log (num);

		for (int i = 0; i < _shieldPieceSpriteRenderer.Length; i++) {
			if (_shieldPieceSpriteRenderer [i].gameObject.activeInHierarchy) {
				_shieldPieceSpriteRenderer [i].gameObject.SetActive (false);
				break;
			}
		}

		yield return new WaitForSeconds (_waitTime);

		_isAnimationFinished = true;
	}



}

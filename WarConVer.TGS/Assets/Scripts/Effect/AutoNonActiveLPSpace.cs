using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==ライフスペースクラス
//
//==使用方法：ライフスペースとなるGameObjectにアタッチ
[RequireComponent(typeof(LifeSpaceAnimation))]
public class AutoNonActiveLPSpace : MonoBehaviour {
	[ SerializeField ] LifeSpaceAnimation _lifeSpaceAnim = null;
	[ SerializeField ] AutoDestroyEffect _directAttackEffect = null;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_lifeSpaceAnim.Is_Animation_Finished) {
			_lifeSpaceAnim.Is_Animation_Finished = false;
			this.gameObject.SetActive (false);
		}
	}


	//==========================================================================
	//public関数

	//--ダイレクトアタックアニメ-ションをする関数
	public void StartDirectAttackAnimation() {
		this.gameObject.SetActive (true);
		StartCoroutine (_lifeSpaceAnim.ShieldAnimation ());
		//エフェクト処理---------------------------------------------------------------------
		Vector3 effectPos = this.gameObject.transform.position;
		effectPos.z = Camera.main.transform.position.z + 1f;//カメラに近い位置に生成したいため
		Instantiate<AutoDestroyEffect>( _directAttackEffect, effectPos, Quaternion.identity );
		//----------------------------------------------------------------------------------


	}
	//==========================================================================
	//==========================================================================

}

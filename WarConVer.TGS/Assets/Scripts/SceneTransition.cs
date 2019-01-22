using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//==シーン遷移機能クラス
//
//==使用方法：シーン遷移時にアクティブなゲームオブジェクトにアタッチ
public class SceneTransition : MonoBehaviour {

	//==================================================================
	//public関数

	//--sceneName名のシーンに遷移をする関数
	public void Transition( string sceneName ) {
		SceneManager.LoadScene ( sceneName );
	}
	//==================================================================
	//==================================================================
}

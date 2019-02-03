using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==ユーザーが選択したデッキを保存するクラス
//
//==DonotDestroyOnLoadなオブジェクトにアタッチ
public class SelectedDeckData : MonoBehaviour {
	[ SerializeField ] Deck _useDeck;	//ユーザーが使用するデッキ


	//===============================================================
	//アクセッサ
	public Deck USE_DECK {
		get { return _useDeck; }
		set { _useDeck = value; }
	}
	//===============================================================
	//===============================================================

	// Use this for initialization
	void Start () {
		//2つ以上存在しない処理-------------------------------------------------------------
		GameObject[ ] selectDeckDatas = GameObject.FindGameObjectsWithTag ( "DeckData" );
		if ( selectDeckDatas.Length >= 2 ) {
			for (int i = 0; i < selectDeckDatas.Length; i++) {
				if (selectDeckDatas [i].scene.name != "DontDestroyOnLoad") {
					Destroy (selectDeckDatas [i]);
				}
			}
		} else {
			DontDestroyOnLoad ( this.gameObject );
		}
		//---------------------------------------------------------------------------------
	}
	


}
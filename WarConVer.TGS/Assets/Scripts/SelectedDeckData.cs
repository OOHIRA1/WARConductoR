using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==ユーザーが選択したデッキを保存するクラス
//
//==DonotDestroyOnLoadなオブジェクトにアタッチ
public class SelectedDeckData : MonoBehaviour {
	[ SerializeField ] List<int> _useDeckCardIDs;	//ユーザーが使用するデッキ内ののカードIDリスト


	//===============================================================
	//アクセッサ
	public List<int> USE_DECK_CARD_IDS {
		get { return _useDeckCardIDs; }
		set { _useDeckCardIDs = value; }
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
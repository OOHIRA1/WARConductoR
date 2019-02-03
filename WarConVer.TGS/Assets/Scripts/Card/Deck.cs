using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==デッキ機能クラス
//
//==使用方法：デッキのGameObjectにアタッチ
public class Deck : MonoBehaviour {
	[SerializeField] List<int> _cardIDs = null;	//デッキにあるカードID
	[SerializeField] int _deckNum = 0;			//デッキの枚数
	const int _MAX_DECK_NUM = 20;				//デッキ枚数の最大値


	//===============================================================
	//アクセッサ
	public int deckNum {
		get { return _deckNum; }
	}


	public List<int> CARD_IDS {
		get { return _cardIDs; }
	}
	//===============================================================
	//===============================================================


	// Use this for initialization
	void Start () {
		_deckNum = _MAX_DECK_NUM;
	}
	

	//======================================================================================================================
	//public関数

	//--デッキをシャッフルする関数
	public void Shuffle( ) {
		for ( int i = 0; i < _cardIDs.Count; i++ ) {
			int temp = _cardIDs[ i ];
			int random = Random.Range( 0, _cardIDs.Count );
			_cardIDs[ i ] = _cardIDs[ random ];
			_cardIDs[ random ] = temp;
		}
	}


	//--デッキからカードをドローする関数
	public CardMain Draw( ) {
		CardMain card = null;
		//ドロー処理--------------------------------------------------------------------------------------------------------
		if (_cardIDs.Count > 0) {
			GameObject prefab = (GameObject)Resources.Load ("Prefab/Card");
			GameObject cardObj = (GameObject)Instantiate (prefab, Vector3.zero, Quaternion.identity);//原点にInstantiateする
			card = cardObj.GetComponent<CardMain> ();
			card.loadID = _cardIDs [ 0 ];//デッキトップのカードIDを読み込むように設定する
			_cardIDs.RemoveAt (0);
			_deckNum = _cardIDs.Count;//デッキ枚数の更新
		} else {
			Debug.Log ("デッキのカードがありません！！");
		}
		//-----------------------------------------------------------------------------------------------------------------
		return card;
	}


	//--デッキにカードを戻す関数
	public void ReturnCard( int cardID ) {
		_cardIDs.Add( cardID );
	}


	//--デッキをセットする関数
	public void SetDeck( List<int> cardIDs ) {
		_cardIDs = cardIDs;
	}
	//=======================================================================================================================
	//=======================================================================================================================
}

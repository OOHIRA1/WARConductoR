using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==カード機能クラス
//
//==使用方法：カードのGameObjectにアタッチ
public class CardMain : MonoBehaviour {
	[SerializeField] int _loadID = 0;	//読み込むカードID
	[SerializeField] SpriteRenderer _cardSpriteRenderer = null;	//カードのSpriteRenderer
	[SerializeField] CardDataLoader _cardDataLoader = null;
	[SerializeField] CardData _cardData = null;
	[SerializeField] int _actionCount = 0;		//移動回数

	// Use this for initialization
	void Start () {
		_cardSpriteRenderer = GetComponent<SpriteRenderer> ();
		_cardDataLoader = GameObject.Find ("CardDataLoader").GetComponent<CardDataLoader>();
		Debug.Log (_loadID);
		Debug.Log (_cardData);
		Debug.Log (_actionCount);
		Debug.Log (_cardSpriteRenderer);
		Debug.Log (_cardDataLoader);
	}
}

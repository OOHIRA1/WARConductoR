using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==カード機能クラス
//
//==使用方法：カードのGameObjectにアタッチ
public class CardMain : MonoBehaviour {
	[SerializeField] int _loadID = 0;							//読み込むカードID
	[SerializeField] SpriteRenderer _cardSpriteRenderer = null;	//カードのSpriteRenderer
	[SerializeField] CardDataLoader _cardDataLoader = null;
	[SerializeField] CardData _cardData = null;
	[SerializeField] int _actionCount = 0;						//移動回数
	const int _MAX_ACTION_COUNT = 3;							//移動回数の最大値

	// Use this for initialization
	void Start () {
		_cardSpriteRenderer = GetComponent<SpriteRenderer> ();
		_cardSpriteRenderer.sprite = (Sprite)Resources.Load<Sprite> ("Card/" + _loadID);//カード画像の読み込み
		_cardDataLoader = GameObject.Find ("CardDataLoader").GetComponent<CardDataLoader>();
		Load ();//カードデータの読み込み
		Debug.Log (_cardData);
		Debug.Log (_actionCount);
	}


	//--カードデータを読み込む関数
	void Load() {
		_cardData = _cardDataLoader.GetCardDataFromID ( _loadID );
	}
}

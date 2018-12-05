using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//==カードデータをCSVから読み込む機能
//
//==使用方法：カードデータを読み込むGameObjectにアタッチ
public class CardDataLoader : MonoBehaviour {
	[SerializeField] List<CardData> _cardDatas = null;	//カードデータ構造体リスト
	[SerializeField] List<string[]> _csvStringData = new List<string[]>();
	[SerializeField] TextAsset _csvData = null;

	void Awake() {
		_csvData = (TextAsset)Resources.Load ("CSV/black_card_data");
		Debug.Log (_csvData.text);
		StringReader stringReader = new StringReader (_csvData.text);
		while (stringReader.Peek () > -1) {
			//','毎に区切って配列へ格納
			string line = stringReader.ReadLine();
			_csvStringData.Add (line.Split(','));
		}
		for (int i = 0; i < _csvStringData.Count; i++) {
			if ( i > 0 ) {//i==0は各項目名なので除く
				CardData cardData = new CardData();
				cardData._id 					= int.Parse(_csvStringData [i] [0]);
				cardData._attack 				= int.Parse(_csvStringData [i] [1]);
				cardData._toughness 			= int.Parse(_csvStringData [i] [2]);
				cardData._directionOfTravel 	= int.Parse(_csvStringData [i] [3]);
				cardData._effect 				= int.Parse(_csvStringData [i] [4]);
				//_cardDatas [i - 1]._necessaryMP = int.Parse(_csvStringData [i] [5]);
				cardData._necessaryAP 			= int.Parse(_csvStringData [i] [5]);
				cardData._necessaryAPForEffect 	= int.Parse(_csvStringData [i] [6]);
				_cardDatas.Add(cardData);
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

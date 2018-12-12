using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//==カードデータをCSVから読み込む機能
//
//==使用方法：カードデータを読み込むGameObjectにアタッチ
public class CardDataLoader : MonoBehaviour {
	[SerializeField] string[] _csvFimeNames = null;		//CSVファイル名の配列
	[SerializeField] List<CardData> _cardDatas = null;	//カードデータ構造体リスト

	void Awake() {
		//CSVからカードデータを読み込む処理------------------
		for (int i = 0; i < _csvFimeNames.Length; i++) {
			LoadFromCSV ( _csvFimeNames[i] );
		}
		//-------------------------------------------------
	}


	//--CSVからカードデータを読み込む関数
	void LoadFromCSV( string csvFimeName ) {
		TextAsset csvTextAsset = (TextAsset)Resources.Load ("CSV/" + csvFimeName);	//CSVファイルをTextAssetとして読み込む
		StringReader stringReader = new StringReader (csvTextAsset.text);			//csvTextAssetのテキストを読むStringReaderを生成
		List<string[]> csvStringArrayList = new List<string[]> ();					//CSVの各値を列ごとにstring配列で格納するための変数
		//カンマごとに区切って配列へ格納-------------------
		while (stringReader.Peek () > -1) {
			string line = stringReader.ReadLine();
			csvStringArrayList.Add (line.Split(','));
		}
		//------------------------------------------------
		//CSVのデータを_cardDatasへ格納-------------------------------------------------------
		for (int i = 0; i < csvStringArrayList.Count; i++) {
			if ( i > 0 ) {//i==0は各項目名なので除く
				CardData cardData = new CardData();
				cardData._id 					= int.Parse(csvStringArrayList [i] [0]);
				cardData._attack 				= int.Parse(csvStringArrayList [i] [1]);
				cardData._toughness 			= int.Parse(csvStringArrayList [i] [2]);
				cardData._directionOfTravel 	= int.Parse(csvStringArrayList [i] [3]);
				cardData._effect 				= int.Parse(csvStringArrayList [i] [4]);
				cardData._necessaryMP 			= int.Parse(csvStringArrayList [i] [5]);
				cardData._necessaryAP 			= int.Parse(csvStringArrayList [i] [5]);
				cardData._necessaryAPForEffect 	= int.Parse(csvStringArrayList [i] [6]);
				_cardDatas.Add(cardData);
			}
		}
		//------------------------------------------------------------------------------------
	}


	//==================================================================================================
	//public関数

	//--カードIDがidのカードデータを返す関数
	public CardData GetCardDataFromID( int id ) {
		CardData value = null;
		for (int i = 0; i < _cardDatas.Count; i++) {
			if (_cardDatas [i]._id == id) {
				value = _cardDatas [i];
				break;
			}
		}
		return value;
	}

	//==================================================================================================
	//==================================================================================================
}

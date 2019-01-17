using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//==カードデータをCSVから読み込む機能
//
//==使用方法：カードデータを読み込むGameObjectにアタッチ
public class CardDataLoader : MonoBehaviour {
	[SerializeField] string[] _csvFileNames = null;		//CSVファイル名の配列
	[SerializeField] List<CardData> _cardDatas = null;	//カードデータ構造体リスト

	void Awake() {
		//CSVからカードデータを読み込む処理------------------
		for (int i = 0; i < _csvFileNames.Length; i++) {
			LoadFromCSV ( _csvFileNames[i] );
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
				cardData._maxToughness 			= int.Parse(csvStringArrayList [i] [2]);
				cardData._toughness 			= cardData._maxToughness;
				//CSVデータからList<Field.DIRECTION>を作る--------------------------------------------
				int directionOfTravelData 		= int.Parse(csvStringArrayList [i] [3]);
				List<Field.DIRECTION> directionOfTravelList = new List<Field.DIRECTION> ();
				while (directionOfTravelData != 0) {
					Field.DIRECTION fieldDirection = (Field.DIRECTION)( directionOfTravelData % 10 );
					directionOfTravelList.Add ( fieldDirection );
					directionOfTravelData /= 10;
				}
				//-----------------------------------------------------------------------------------
				cardData._directionOfTravel 	= directionOfTravelList;
				cardData._effect_type 			= (CardData.EFFECT_TYPE)int.Parse(csvStringArrayList [i] [4]);
				cardData._effect_value 			= int.Parse(csvStringArrayList [i] [5]);
				//CSVデータからList<Field.DIRECTION>を作る--------------------------------------------
				int effectDirectionData 		= int.Parse(csvStringArrayList [i] [6]);
				List<Field.DIRECTION> effectDirecionList = new List<Field.DIRECTION> ();
				while (effectDirectionData != 0) {
					Field.DIRECTION fieldDirection = (Field.DIRECTION)( effectDirectionData % 10 );
					effectDirecionList.Add ( fieldDirection );
					effectDirectionData /= 10;
				}
				//------------------------------------------------------------------------------------
				cardData._effect_direction 		= effectDirecionList;
				cardData._effect_distance 		= int.Parse(csvStringArrayList [i] [7]);
				cardData._necessaryMP 			= int.Parse(csvStringArrayList [i] [8]);
				cardData._necessaryAP 			= int.Parse(csvStringArrayList [i] [9]);
				cardData._necessaryAPForEffect 	= int.Parse(csvStringArrayList [i] [10]);
				_cardDatas.Add(cardData);
			}
		}
		//------------------------------------------------------------------------------------
	}


	//==================================================================================================
	//public関数

	//--カードIDがidのカードデータを返す関数
	public CardData GetCardDataFromID( int id ) {
		CardData value = new CardData();
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

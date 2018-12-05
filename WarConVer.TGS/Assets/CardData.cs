using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==カードデータクラス(構造体として扱う)
[System.Serializable]
public class CardData {
	public int _id;						//カードID
	public int _attack;					//攻撃力
	public int _toughness;				//体力
	public int _directionOfTravel;		//移動方向
	public int _effect;					//効果
	public int _necessaryMP;			//召喚で必要なMP
	public int _necessaryAP;			//移動で必要なAP
	public int _necessaryAPForEffect;	//効果で必要なAP
}

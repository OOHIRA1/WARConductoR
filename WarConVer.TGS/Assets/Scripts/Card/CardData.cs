using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==カードデータクラス(構造体として扱う)
[System.Serializable]
public struct CardData {
	public enum EFFECT_TYPE {
		NO_EFFECT = 0,
		ATTACK,
		RECOVERY,
		MOVE,
	}

	public int 						_id;					//カードID
	public int 						_attack;				//攻撃力
	public int 						_maxToughness;			//体力最大値(全回復させるとき等に必要)
	public int 						_toughness;				//体力
	public List<Field.DIRECTION> 	_directionOfTravel;		//移動方向
	public EFFECT_TYPE 				_effect_type;			//効果ID
	public int 						_effect_value;			//効果値(攻撃力・回復量・移動量)
	public List<Field.DIRECTION> 	_effect_direction;		//効果方向(攻撃方向・回復方向・移動方向)
	public int 						_effect_distance;		//効果距離(攻撃距離・回復距離・移動距離)
	public int						_necessaryMP;			//召喚で必要なMP
	public int 						_necessaryAP;			//移動で必要なAP
	public int 						_necessaryAPForEffect;	//効果で必要なAP


	//======================================================================================
	//コンストラクタ
//	public CardData() {
//		_directionOfTravel = new List<Field.DIRECTION> ();
//		_effect_direction  = new List<Field.DIRECTION> ();
//	}
	//======================================================================================
	//======================================================================================
}
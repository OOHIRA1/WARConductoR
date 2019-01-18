using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantStorehouse {
	//タグ
	public static readonly string TAG_PLAYER1 = "Player1";
	public static readonly string TAG_PLAYER2 = "Player2";
	public static readonly string TAG_SQUARE  = "Square";

	//レイヤー
	public static readonly string LAYER_SQUARE    = "Square";
	public static readonly string LAYER_HAND_CARD = "HandCard";

	//共通定数
	public static readonly int SQUARE_ROW_NUM   = 4;
	public static readonly int FIRST_ROW_INDEX  = 0;
	public static readonly int SECOND_ROW_INDEX = 1;
	public static readonly int THIRD_ROW_INDEX  = 2;
	public static readonly int FOURTH_ROW_INDEX = 3;
	public static readonly int FIFTH_ROW_INDEX  = 4;
	public static readonly int ERROR			= -1;
}

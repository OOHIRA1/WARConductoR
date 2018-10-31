using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

    const int WAIT_FRAME = 10;
    const int MOVE_FRONT = 4;
    const int BACK_FRONT = -4;
    const int FRONT_RIGTH = 3;
    const int FRONT_LEFT = 5;
    const int BACK_RIGTH = -5;
    const int BACK_LEFT = -3;
    const int MOVE_RIGTH = -1;
    const int MOVE_LEFT = 1;

    private enum MOVE_TYPE
    {
        Front,
        Back,
        Left,
        Rigth,
        Back_Left,
        Back_Rigth,
        Front_Rigth,
        Front_Left,
        MAX_MOVE_TYPE
    }

    //このクラスでunitの動きを管理する、UnitMgrから呼び出され、UnitMgrに値を返す。
    [SerializeField] UnitMgr unitMgr;
    [SerializeField] int unit_Name;
    //答え、行先
    [SerializeField] int ans_Num;

    private MOVE_TYPE move_num = MOVE_TYPE.Front;


    // Use this for initialization
	void Start () {
	
	}
	//呼び出されたあと、移動先を計算
    public int Move_Calculation(int move_Num, int unit_Num)
    {
        switch (move_Num)
        {
            //前進
            case 0:
                ans_Num = unit_Num + MOVE_FRONT;
                break;
            //後退
            case 1:
                move_num = MOVE_TYPE.Back;
                ans_Num = unit_Num + BACK_FRONT;
                move_num = MOVE_TYPE.Front;
                break;
            //左
            case 2:
                move_num = MOVE_TYPE.Left;
                ans_Num = unit_Num + MOVE_LEFT;
                move_num = MOVE_TYPE.Front;
                break;
            //右
            case 3:
                move_num = MOVE_TYPE.Rigth;
                ans_Num = unit_Num + MOVE_RIGTH;
                move_num = MOVE_TYPE.Front;
                break;
            //左後ろ
            case 4:
                move_num = MOVE_TYPE.Back_Left;
                ans_Num = unit_Num + BACK_LEFT;
                move_num = MOVE_TYPE.Front;
                break;
            //右後ろ
            case 5:
                move_num = MOVE_TYPE.Back_Rigth;
                ans_Num = unit_Num + BACK_RIGTH;
                move_num = MOVE_TYPE.Front;
                break;
            //右前
            case 6:
                move_num = MOVE_TYPE.Front_Rigth;
                ans_Num = unit_Num + FRONT_RIGTH;
                move_num = MOVE_TYPE.Front;
                break;
            //左前
            case 7:
                move_num = MOVE_TYPE.Front_Left;
                ans_Num = unit_Num + FRONT_LEFT;
                move_num = MOVE_TYPE.Front;
                break;
            default:
                break;

        }
        return ans_Num;
    }
    
}

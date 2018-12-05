using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	[SerializeField] UnitMgr unitMgr = null;
//    private int unit_Num;

//    public int set_unit_Num
//    {
//        set { unit_Num = value; }
//    }
	// Use this for initialization
	void Start () {
	}
	
    public void OnClickMove()
    {
        unitMgr.Tap_Acution_Move();
    }
}

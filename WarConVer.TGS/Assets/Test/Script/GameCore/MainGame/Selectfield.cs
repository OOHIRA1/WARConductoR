using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selectfield : MonoBehaviour {

	[SerializeField] UnitMgr unitMgr = null;
	[SerializeField] UnitController unitController = null;
	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Image>().color = Vector4.one * 0.6f;
	}
	
    public void Send_Unit_Num()
    {
        unitMgr._num = int.Parse(this.name);
        unitMgr.Move_Select();
    }
    public int SelectField(int Select_Move)
    {
        return unitController.Move_Calculation(Select_Move, int.Parse(this.name));

    }
	// Update is called once per frame
	void Update () {
		
	}
}

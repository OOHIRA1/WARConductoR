using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//==プレイヤークラス
public class Player : MonoBehaviour
{
    const int MAX_PLAYER_AP = 6;

//    [SerializeField] GameObject _gameMgr;
//    [SerializeField] GameMgr gameMgr;
//    [SerializeField] GameObject[] AP = { };
//    [SerializeField] GameObject[] LIFE = { };
//    [SerializeField] Material crystal;
    [SerializeField] int _mana = 0;				//マナとは？？？
    [SerializeField] int _life = 0;				//プレイヤーのライフ
    [SerializeField] int _turn = 0;				//何ターン目かのカウンター？？？
    [SerializeField] int _handNum = 0;			//手札の数
    [SerializeField] int _actionPoint = 0;		//APのこと？？？
    [SerializeField] int _effectPoint = 0;		//???
    [SerializeField] int _cemetery = 0;			//墓地
    [SerializeField] int _deck = 0;				//デッキの枚数
    [SerializeField] bool _dead = false;		//死んだかどうかのフラグ
    [SerializeField] bool _playerTurn = false;	//自分のターンかどうかのフラグ？？？
    [SerializeField] Text _text;				//???


	//------------------------------------------------------
	//アクセッサ
    public int Mana
    {
        set { this._mana = value; }
        get { return this._mana;  }
    }
    public int Life
    {
        set { this._life = value; }
        get { return this._life; }
    }
    public int Trun
    {
        set { this._turn = value; }
        get { return this._turn; }
    }
    public int HandNum
    {
        set { this._handNum = value; }
        get { return this._handNum; }
    }
    public int ActionPoint
    {
        set { this._actionPoint = value; }
        get { return this._actionPoint; }
    }
    public int EffectPoint
    {
        set { this._effectPoint = value; }
        get { return this._effectPoint; }
    }
    public int Cemetery
    {
        set { this._cemetery = value; }
        get { return this._cemetery; }
    }
    public int Deck
    {
        set { this._deck = value; }
        get { return this._deck; }
    }
    public bool Dead
    {
        set { this._dead = value; }
        get { return this._dead; }
    }
    public bool PlayerTrun
    {
        set { this._playerTurn = value; }
        get { return this._playerTurn; }
    }
    public Text Text
    {
        set { this._text = value; }
        get { return this._text; }
    }
	//-----------------------------------------------------
	//-----------------------------------------------------



    // Use this for initialization
    void Start( )
    {
        _mana = 100;
        _deck = 20;
        _actionPoint = 6;
        _life = 5;
        //_text.text = _mana.ToString();
        
    }

	//-----------------------------------------------------------------------------------------------------
	//public関数

	//--APクリスタルを更新する関数
    public void Update_Crystal_AP()
    {
        int counter = 0;
        for (int i = ActionPoint; i < MAX_PLAYER_AP; i++)
        {
           // AP[counter].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/off_Crystal");
            counter++;
            print(counter);
        }
    }
    //--------------------------------------------------------------------------------------------------------
	//--------------------------------------------------------------------------------------------------------
}

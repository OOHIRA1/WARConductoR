using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UnitMgr : MonoBehaviour {

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

    [SerializeField] CardList _cardList;
	[SerializeField] Unit[] unit = null;
	[SerializeField] UnitController unitController = null;
	[SerializeField] Player _player = null;
    [SerializeField] GameMgr gameMgr;
	[SerializeField] GameObject[] Unit = null;
    [SerializeField] int _Power;
    [SerializeField] int _Com;
    [SerializeField] int _Cost;
    [SerializeField] int _Move;
    [SerializeField] int _Move_Point;
    [SerializeField] int _AP;
    [HideInInspector]public int Unit_Num;
    [SerializeField] int _Action_Num;
    [SerializeField] string _Image_Name;
	[SerializeField] Button[] Action_Move = null;
    [SerializeField] GameObject[] Fieldselect;
    [SerializeField] Image iconImage;
//    [SerializeField] int tmp = 0;
    [SerializeField] int Select_Move = 0;

    private int move_Field;
    private int unit_name;
    private bool is_running = false;
    private int[] move_calculation;
    private int now_unit__num;

    public int _pow
    {
        set { _Power = value; }
        get { return _Power; }
    }
    public int _com
    {
        set { _Com = value; }
        get { return _Com; }
    }
    public int _move
    {
        set { _Move = value; }
        get { return _Move; }
    }
    public int _move_point
    {
        set { _Move_Point = value; }
        get { return _Move_Point; }
    }
    public int _ap
    {
        set { _AP = value; }
        get { return _AP; }
    }
    public int _num
    {
        set { Unit_Num = value; }
        get { return Unit_Num; }
    }
    public string Image_Name
    {
        set { _Image_Name = value; }
        get { return _Image_Name; }
    }
    public Image IconImage
    {
        set { iconImage = value; }
        get { return iconImage; }
    }
    public int _cost
    {
        set { _Cost = value; }
        get { return _Cost; }
    }
    
    public int Unit_Name
    {
        set { unit_name = value; }
        get { return unit_name; }
    }

    public bool is_Running
    {
        set { is_running = value; }
        get { return is_running; }
    }

    public int Now_Unit_Num
    {
        set { now_unit__num = value; }
        get { return now_unit__num; }
    }

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start() {
        move_calculation = new int[9];
    }

    public void Data_Move_Unit()
    {
        unit[Unit_Num ]._combat = _com;
        unit[Unit_Num ]._power = _pow;
        unit[Unit_Num ]._move = _move;
        unit[Unit_Num ]._move_Point = _move_point;
        unit[Unit_Num ]._ap = _ap;
        unit[Unit_Num ]._imageName = _Image_Name;
        unit[Unit_Num ].Scripts_Attach();
    }

    public void Tap_Acution_Move()
    {
        if ( _player.ActionPoint == 0 || unit[Unit_Num]._Action_Count >= 3)
        {
            print("YouDon'tMove!!");
            return;
        }
        else
        {
          /*  for(int i = 0; i < 20; i++)
            {
                Fieldselect[i].SetActive(true);
            }*/
            switch (_Move)
            {
                case 0:
                    Select_Field();
                    break;
                case 1:
                    Select_Field();
                  // Move_Rigth();
                  //  Back_Left();
                  //  Back_Rigth();
                  //  Front_Rigth();
                  //  Front_Left();
                    break;          
                default:
                    break;
            }
        }
    }

    void Select_Field()
    {
        switch (_Move)
        {
            case 0:
                Action_Move[(int)MOVE_TYPE.Front] = unit[Unit_Num  + MOVE_FRONT].GetComponent<Button>();
                unit[Unit_Num + MOVE_FRONT].GetComponent<Image>().color = Color.red;
                Select_Move = (int)MOVE_TYPE.Front;
                Move_Select();
                break;
            case 1:
                
                Action_Move[(int)MOVE_TYPE.Front] = unit[Unit_Num  + MOVE_FRONT].GetComponent<Button>();
                unit[Unit_Num  + MOVE_FRONT].iconImage.color = Color.red * 0.6f;
                move_calculation[(int)MOVE_TYPE.Front] = Unit_Num + MOVE_FRONT;

                Action_Move[(int)MOVE_TYPE.Rigth] = unit[Unit_Num  + MOVE_RIGTH].GetComponent<Button>();
                unit[Unit_Num  + MOVE_RIGTH].iconImage.color = Color.red * 0.6f;
                move_calculation[(int)MOVE_TYPE.Rigth] = Unit_Num + MOVE_RIGTH;

                if (Unit_Num == move_calculation[(int)MOVE_TYPE.Front] )
                {
                    print(Unit_Num + MOVE_FRONT);
                    Select_Move = (int)MOVE_TYPE.Front;
                    move_Field = unitController.Move_Calculation(Select_Move, Unit_Num);

                }
                if (Unit_Num == move_calculation[(int)MOVE_TYPE.Rigth])
                {
                    print(Unit_Num + MOVE_RIGTH);
                    Select_Move = (int)MOVE_TYPE.Rigth;
                    move_Field = unitController.Move_Calculation(Select_Move, Unit_Num);
                    unit[Unit_Num + MOVE_FRONT].iconImage.color = Color.white;


                }

                break;
            default:
                break;
        }
    }
  

    public void Move_Select()
    {
        /*  for (int i = 0; i < 20; i++)
          {
              Fieldselect[i].SetActive(false);
          }*/
        move_Field = unit[Unit_Num].SelectField(Select_Move);
        Tap_Acution_Move_Next();
    }

    public void Tap_Acution_Move_Next()
    {
        Action_Move[Select_Move].onClick.AddListener(_Action_Move);
        Unit[move_Field].GetComponent<Image>().color = Color.white;
        _player.ActionPoint -= _Move_Point;
        /* if (_player.ActionPoint < 0)
         {
             return;
         }
         else
         {
           //  _player.Update_Crystal_AP();
         }
         */
    }
    //後
    void Move_Back()
    {
        if (int.Parse(unit[Unit_Num  + BACK_FRONT].name) < 0 )
        {
            return;
        }
        Action_Move[(int)(MOVE_TYPE.Back)] = unit[Unit_Num  + BACK_FRONT].GetComponent<Button>();
        unit[Unit_Num  + BACK_FRONT].iconImage.color = Color.red * 0.6f;
        Action_Move[(int)(MOVE_TYPE.Back)].onClick.AddListener(() => Select_Move = (int)(MOVE_TYPE.Back));

    }
    //右
    void Move_Rigth()
    {
        if (Unit_Num == 0 && Unit_Num == 4 && Unit_Num == 8 && Unit_Num == 12 && Unit_Num == 16)
        {
            return;
        }
        Action_Move[3] = unit[Unit_Num  + MOVE_RIGTH].GetComponent<Button>();
        unit[Unit_Num  + MOVE_RIGTH].iconImage.color = Color.red * 0.6f;
        Action_Move[3].onClick.AddListener(() => Select_Move = 3);
    }
    //左
    void Move_Left()
    {
        if (Unit_Num == 4 && Unit_Num == 8 && Unit_Num == 12 && Unit_Num == 16 && Unit_Num == 20)
        {
            return;
        }
        Action_Move[2] = unit[Unit_Num  + MOVE_LEFT].GetComponent<Button>();
        unit[Unit_Num  + MOVE_LEFT].iconImage.color = Color.red * 0.6f;
        Action_Move[2].onClick.AddListener(() => Select_Move = 2);
    }
    //右前
    void Front_Rigth()
    {
        if (Unit_Num == 1 && Unit_Num ==  5 && Unit_Num == 9 && Unit_Num == 13 && Unit_Num == 17
            && Unit_Num  + FRONT_RIGTH > 20 )
        {
            return;
        }
        Action_Move[6] = unit[Unit_Num  + FRONT_RIGTH].GetComponent<Button>();
        unit[Unit_Num  + FRONT_RIGTH].iconImage.color = Color.red * 0.6f;
        Action_Move[6].onClick.AddListener(() => Select_Move = 6);

    }
    //左前
    void Front_Left()
    {
        if (Unit_Num == 4 && Unit_Num == 8 && Unit_Num == 12 && Unit_Num == 16 && Unit_Num == 20
            && Unit_Num  + FRONT_LEFT > 20 )
        {
            return;
        }
        Action_Move[7] = unit[Unit_Num  + FRONT_LEFT].GetComponent<Button>();
        unit[Unit_Num  + FRONT_LEFT].iconImage.color = Color.red * 0.6f;
        Action_Move[7].onClick.AddListener(() => Select_Move = 7);

    }
    //右後
    void Back_Rigth()
    {
        if (Unit_Num == 1 && Unit_Num == 5 && Unit_Num == 9 && Unit_Num == 13 && Unit_Num == 17
            && Unit_Num  + BACK_RIGTH < 0)
        {
            return;
        }
        Action_Move[4] = unit[Unit_Num  + BACK_RIGTH].GetComponent<Button>();
        unit[Unit_Num  + BACK_RIGTH].iconImage.color = Color.red * 0.6f;
        Action_Move[4].onClick.AddListener(() => Select_Move = 4);

    }
    //左後
    void Back_Left()
    {
        if (Unit_Num == 4 && Unit_Num  == 8 && Unit_Num == 12 && Unit_Num == 16 && Unit_Num == 20
            && Unit_Num  + BACK_LEFT < 0)
        {
            return;
        }
        Action_Move[5] = unit[Unit_Num  + BACK_LEFT].GetComponent<Button>();
        unit[Unit_Num  + BACK_LEFT].iconImage.color = Color.red * 0.6f;
        Action_Move[5].onClick.AddListener(() => Select_Move = 5);

    }

    void Front_Update()
    {

        Unit[move_Field].GetComponent<Image>().sprite = iconImage.sprite;
        unit[ move_Field]._power = _pow;
        unit[ move_Field]._combat = _com;
        unit[ move_Field]._move = _move;
        unit[ move_Field]._move_Point = _Move_Point;
        unit[ move_Field]._ap = _ap;
        unit[ move_Field]._Action_Count = unit[Unit_Num]._Action_Count;
        Action_Move[Select_Move].onClick.RemoveListener(_Action_Move);
        //null_unit();
      /*  print(move_Field);
        unit[Unit_Num].Null();
        Unit[ move_Field].GetComponent<Image>().sprite = iconImage.sprite;
        Unit[ move_Field].GetComponent<Image>().color = Color.white;

        unit[ move_Field]._comImage.SetActive(true);
        unit[ move_Field]._powImage.SetActive(true);

        unit[ move_Field]._powText.text = unit[Unit_Num]._power.ToString();
        unit[ move_Field]._comText.text = unit[Unit_Num]._combat.ToString();
        //unit[ move_Field].Scripts_Attach();

        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        */
    }
    //後進処理
    void Back_Update()
    {
        unit[Unit_Num  + BACK_FRONT].iconImage.sprite = IconImage.sprite;
        unit[Unit_Num  + BACK_FRONT]._imageName = Image_Name;
        unit[Unit_Num  + BACK_FRONT]._comImage.SetActive(true);
        unit[Unit_Num  + BACK_FRONT]._powImage.SetActive(true);
        unit[Unit_Num  + BACK_FRONT]._power = _pow;
        unit[Unit_Num  + BACK_FRONT]._combat = _com;
        unit[Unit_Num  + BACK_FRONT]._move = _move;
        unit[Unit_Num  + BACK_FRONT]._move_Point = _Move_Point;
        unit[Unit_Num  + BACK_FRONT]._ap = _ap;
        unit[Unit_Num +  BACK_FRONT]._Action_Count = unit[Unit_Num]._Action_Count;
        unit[Unit_Num  + BACK_FRONT]._powText.text = unit[Unit_Num  + MOVE_FRONT]._power.ToString();
        unit[Unit_Num  + BACK_FRONT]._comText.text = unit[Unit_Num  + MOVE_FRONT]._combat.ToString();
        unit[Unit_Num  + BACK_FRONT].iconImage.color = Color.white;
        unit[Unit_Num +  BACK_FRONT].Scripts_Attach();


        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        unit[Unit_Num ].Null();
    }
    //右移動処理
    void Rigth_Update()
    {
        unit[Unit_Num  + MOVE_RIGTH].iconImage.sprite = IconImage.sprite;
        unit[Unit_Num  + MOVE_RIGTH]._imageName = Image_Name;
        unit[Unit_Num  + MOVE_RIGTH]._comImage.SetActive(true);
        unit[Unit_Num  + MOVE_RIGTH]._powImage.SetActive(true);
        unit[Unit_Num  + MOVE_RIGTH]._power = _pow;
        unit[Unit_Num  + MOVE_RIGTH]._combat = _com;
        unit[Unit_Num  + MOVE_RIGTH]._move = _move;
        unit[Unit_Num  + MOVE_RIGTH]._move_Point = _Move_Point;
        unit[Unit_Num  + MOVE_RIGTH]._ap = _ap;
        unit[Unit_Num  + MOVE_RIGTH]._powText.text = unit[Unit_Num  + MOVE_FRONT]._power.ToString();
        unit[Unit_Num  + MOVE_RIGTH]._comText.text = unit[Unit_Num  + MOVE_FRONT]._combat.ToString();
        unit[Unit_Num  + MOVE_RIGTH].iconImage.color = Color.white;

        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        unit[Unit_Num ].Null();
        Action_Move[Select_Move].onClick.RemoveListener(_Action_Move);

    }
    //左移動処理
    void Left_Update()
    {
        unit[Unit_Num  + MOVE_LEFT].iconImage.sprite = IconImage.sprite;
        unit[Unit_Num  + MOVE_LEFT]._imageName = Image_Name;
        unit[Unit_Num  + MOVE_LEFT]._comImage.SetActive(true);
        unit[Unit_Num  + MOVE_LEFT]._powImage.SetActive(true);
        unit[Unit_Num  + MOVE_LEFT]._power = _pow;
        unit[Unit_Num  + MOVE_LEFT]._combat = _com;
        unit[Unit_Num  + MOVE_LEFT]._move = _move;
        unit[Unit_Num  + MOVE_LEFT]._move_Point = _Move_Point;
        unit[Unit_Num  + MOVE_LEFT]._ap = _ap;
        unit[Unit_Num  + MOVE_LEFT]._powText.text = unit[Unit_Num  + MOVE_FRONT]._power.ToString();
        unit[Unit_Num  + MOVE_LEFT]._comText.text = unit[Unit_Num  + MOVE_FRONT]._combat.ToString();
        unit[Unit_Num  + MOVE_LEFT].iconImage.color = Color.white;

        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        unit[Unit_Num ].Null();
    }
    //右斜め前移動処理
    void Front_Rigth_Update()
    {
        unit[Unit_Num  + FRONT_RIGTH].iconImage.sprite = IconImage.sprite;
        unit[Unit_Num  + FRONT_RIGTH]._imageName = Image_Name;
        unit[Unit_Num  + FRONT_RIGTH]._comImage.SetActive(true);
        unit[Unit_Num  + FRONT_RIGTH]._powImage.SetActive(true);
        unit[Unit_Num  + FRONT_RIGTH]._power = _pow;
        unit[Unit_Num  + FRONT_RIGTH]._combat = _com;
        unit[Unit_Num  + FRONT_RIGTH]._move = _move;
        unit[Unit_Num  + FRONT_RIGTH]._move_Point = _Move_Point;
        unit[Unit_Num  + FRONT_RIGTH]._ap = _ap;
        unit[Unit_Num  + FRONT_RIGTH]._powText.text = unit[Unit_Num  + MOVE_FRONT]._power.ToString();
        unit[Unit_Num  + FRONT_RIGTH]._comText.text = unit[Unit_Num  + MOVE_FRONT]._combat.ToString();
        unit[Unit_Num  + FRONT_RIGTH].iconImage.color = Color.white;

        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        unit[Unit_Num ].Null();
    }
    //左斜め前移動処理
    void Front_Left_Update()
    {
        unit[Unit_Num  + FRONT_LEFT].iconImage.sprite = IconImage.sprite;
        unit[Unit_Num  + FRONT_LEFT]._imageName = Image_Name;
        unit[Unit_Num  + FRONT_LEFT]._comImage.SetActive(true);
        unit[Unit_Num  + FRONT_LEFT]._powImage.SetActive(true);
        unit[Unit_Num  + FRONT_LEFT]._power = _pow;
        unit[Unit_Num  + FRONT_LEFT]._combat = _com;
        unit[Unit_Num  + FRONT_LEFT]._move = _move;
        unit[Unit_Num  + FRONT_LEFT]._move_Point = _Move_Point;
        unit[Unit_Num  + FRONT_LEFT]._ap = _ap;
        unit[Unit_Num  + FRONT_LEFT]._powText.text = unit[Unit_Num  + MOVE_FRONT]._power.ToString();
        unit[Unit_Num  + FRONT_LEFT]._comText.text = unit[Unit_Num  + MOVE_FRONT]._combat.ToString();
        unit[Unit_Num  + FRONT_LEFT].iconImage.color = Color.white;

        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        unit[Unit_Num ].Null();
    }
    //右斜め後ろ移動処理
    void Back_Rigth_Update()
    {
        unit[Unit_Num  + BACK_RIGTH].iconImage.sprite = IconImage.sprite;
        unit[Unit_Num  + BACK_RIGTH]._imageName = Image_Name;
        unit[Unit_Num  + BACK_RIGTH]._comImage.SetActive(true);
        unit[Unit_Num  + BACK_RIGTH]._powImage.SetActive(true);
        unit[Unit_Num  + BACK_RIGTH]._power = _pow;
        unit[Unit_Num  + BACK_RIGTH]._combat = _com;
        unit[Unit_Num  + BACK_RIGTH]._move = _move;
        unit[Unit_Num  + BACK_RIGTH]._move_Point = _Move_Point;
        unit[Unit_Num  + BACK_RIGTH]._ap = _ap;
        unit[Unit_Num  + BACK_RIGTH]._powText.text = unit[Unit_Num  + MOVE_FRONT]._power.ToString();
        unit[Unit_Num  + BACK_RIGTH]._comText.text = unit[Unit_Num  + MOVE_FRONT]._combat.ToString();
        unit[Unit_Num  + BACK_RIGTH].iconImage.color = Color.white;

        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        unit[Unit_Num ].Null();
    }
    //左斜め後ろ移動処理
    void Back_Left_Update()
    {
        unit[Unit_Num  + BACK_LEFT].iconImage.sprite = IconImage.sprite;
        unit[Unit_Num  + BACK_LEFT]._imageName = Image_Name;
        unit[Unit_Num  + BACK_LEFT]._comImage.SetActive(true);
        unit[Unit_Num  + BACK_LEFT]._powImage.SetActive(true);
        unit[Unit_Num  + BACK_LEFT]._power = _pow;
        unit[Unit_Num  + BACK_LEFT]._combat = _com;
        unit[Unit_Num  + BACK_LEFT]._move = _move;
        unit[Unit_Num  + BACK_LEFT]._move_Point = _Move_Point;
        unit[Unit_Num  + BACK_LEFT]._ap = _ap;
        unit[Unit_Num  + BACK_LEFT]._powText.text = unit[Unit_Num  + MOVE_FRONT]._power.ToString();
        unit[Unit_Num  + BACK_LEFT]._comText.text = unit[Unit_Num  + MOVE_FRONT]._combat.ToString();
        unit[Unit_Num  + BACK_LEFT].iconImage.color = Color.white;

        unit[Unit_Num ]._comImage.SetActive(false);
        unit[Unit_Num ]._powImage.SetActive(false);
        unit[Unit_Num ]._powText.text = null;
        unit[Unit_Num ]._comText.text = null;
        unit[Unit_Num ].Null();
    }

    void null_unit()
    {
        unit[Unit_Num]._power = 0;
        unit[Unit_Num]._combat = 0;
        unit[Unit_Num]._move_Point = 0;
        unit[Unit_Num]._ap = 0;
        unit[Unit_Num].iconImage.sprite = null;
    }

    void _Action_Move()
    {
        switch (_Move)
        {
            case 0:
                Front_Update();
                break;
            case 1:
                switch (move_Field)
                {
                    case 0:
                        Front_Update();
                        break;
                    case 1:
                        Back_Update();
                        break;
                    case 2:
                        Left_Update();
                        break;
                    case 3:
                        Rigth_Update();
                        break;
                    case 4:
                        Back_Left();
                        break;
                    case 5:
                        Back_Rigth();
                        break;
                    case 6:
                        Front_Rigth();
                        break;
                    case 7:
                        Front_Left();
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
//        tmp = Unit_Num;

    }
}

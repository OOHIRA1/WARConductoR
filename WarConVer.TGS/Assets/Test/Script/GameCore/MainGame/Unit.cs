using UnityEngine;
using UnityEngine.UI;


//==フィールドに召喚されたユニット
public class Unit : MonoBehaviour
{

	[SerializeField] GameMgr _get_gameMgr = null;
	[SerializeField] UnitMgr _unitMgr = null;
//    [SerializeField] Sprite nowSprite;
    [SerializeField] Image _unitImage;
    [SerializeField] Text powText;
    [SerializeField] Text comText;
    [SerializeField] GameObject powImage;
    [SerializeField] GameObject comImage;
	[SerializeField] GameObject _tapImage = null;
    public GameObject OreO;
    [SerializeField] private int combat;
    [SerializeField] private int power;
    [SerializeField] private int move;
    [SerializeField] private int move_point;
    [SerializeField] private int ap;
//    [SerializeField] private int _unitName;
    [SerializeField] private string imageName;
    [SerializeField] private int _action_Count;
	[SerializeField] UnitController unitController = null;

    private Button GetButton;
    private string eventName;
//    private bool add_Tup_Ation = false;
//    private Move move_class;

    public int _combat
    {
        set { combat = value; }
        get { return combat; }
    }

    public int _power
    {
        set { power = value; }
        get { return power; }
    }

    public int _move
    {
        set { move = value; }
        get { return move; }
    }

    public int _ap
    {
        set { ap = value; }
        get { return ap; }
    }

    public int _move_Point
    {
        set { move_point = value; }
        get { return move_point; }
    }
    public int _Action_Count
    {
        set { _action_Count = value; }
        get { return _action_Count;  }
    }

    public string _imageName
    {
        set { imageName = value; }
        get { return imageName; }
    }
    public Image iconImage
    {
        set { _unitImage = value; }
        get { return _unitImage; }
    }

    public GameObject _comImage
    {
        set { comImage = value; }
        get { return comImage;  }
    }

    public GameObject _powImage
    {
        set { powImage = value; }
        get { return powImage; }
    }

    public Text _powText
    {
        set { powText = value; }
        get { return powText; }
    }

    public Text _comText
    {
        set { comText = value; }
        get { return comText; }
    }
    // Use this for initialization
    void Start()
    {
//        move_class = GetComponent<Move>();
//        _unitName = int.Parse(this.name);
        GetButton = this.GetComponent<Button>();
        GetButton.onClick.AddListener(TupAction);
       // eventTrigger = GetButton.gameObject.AddComponent<EventTrigger>();
       // entry = new EventTrigger.Entry( );
       // entry.eventID = EventTriggerType.PointerEnter;
       // Change_Event();
    }
    //召喚されたときにImageをアッタチ
    public void Scripts_Attach()
    {
        for (int i = 0; i < (int)CardList.Card.MAX_CARD_NUM; i++)
        {
            if (_get_gameMgr.Get_Card_Sprite[i] == iconImage.sprite)
            {
                iconImage.sprite = _get_gameMgr.Get_Card_Sprite[i];
            }
        }
         powText.text = power.ToString();
         comText.text = combat.ToString();
         _powImage.SetActive(true);
         _comImage.SetActive(true);
    }

    //動くとカウント追加
    public void Add_OnClick_Action()
    {
        _action_Count++;
    }

    public int SendName()
    {
        return int.Parse(this.gameObject.name);
    }
    //TupActionタップすると画像が大きくなり効果や移動などの選択ができる
    public void TupAction()
    {
        _unitMgr.Unit_Num = int.Parse(this.name);
        if (_unitImage.sprite == null){
            return;
        }
        else
        {
            _unitMgr.IconImage.sprite = _unitImage.sprite;
        }
        _tapImage.SetActive(true);
        _tapImage.GetComponent<Image>().sprite = _unitImage.sprite;
        _unitMgr._pow = power;
        _unitMgr._com = combat;
        _unitMgr._move = move;
        _unitMgr._move_point = move_point;
        _unitMgr._ap = _ap;
      
    }
   
    public void TupactionClouse()
    {
        _tapImage.SetActive(false);
    }
    public int SelectField(int Select_Move)
    {
        return unitController.Move_Calculation(Select_Move, int.Parse(this.name));

    }
    public void Null()
    {
        power = 0;
        combat = 0;
        move = 0;
        move_point = 0;
        ap = 0;
        this.GetComponent<Image>().sprite = null;
//        nowSprite = null;
    }
}

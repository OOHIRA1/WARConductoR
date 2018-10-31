using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Card : MonoBehaviour/*,IBeginDragHandler, IEndDragHandler, IDragHandler*/
{

    [SerializeField] GameObject card;
    [SerializeField] GameMgr gameMgr;
    [SerializeField] Image image;
    [SerializeField] Sprite Getsprite;
    [SerializeField] Graphic GetGraphic;
    [SerializeField] GameObject GetImage;
    [SerializeField] int scene;
   // [SerializeField] SceneMgr _sceneMgr;
    [SerializeField] Vector3 _vector3;
    [SerializeField] Vector3 _offset_Vector3;
    [SerializeField] Vector3 _tmp_Vector3;
    [SerializeField] int _combat;
    [SerializeField] int _power;
    [SerializeField] int _cost;
    [SerializeField] int _move;
    [SerializeField] int _move_poiint;
    [SerializeField] int _actionPoint;
    [SerializeField] string _graphic;
    [SerializeField] Sprite GetSprite;
    [SerializeField] SpriteRenderer Card_spriteRenderer;
    [SerializeField] int _filed_Name;
    [SerializeField] bool cansamon = false;
    public int Combat
    {
        set { _combat = value; }
        get { return _combat; }
    }
    public int Power
    {
        set { _power = value; }
        get { return _power; }
    }
    public int Cost
    {
        set { _cost = value; }
        get { return _cost; }
    }
    public int Move
    {
        set { _move = value; }
        get { return _move; }
    }
    public int Move_Point
    {
        set { _move_poiint = value; }
        get { return _move_poiint; }
    }
    public int ActionPoint
    {
        set { _actionPoint = value; }
        get { return _actionPoint; }
    }
    public string Graphic
    {
        set { _graphic = value; }
        get { return _graphic; }
    }
    public  int Field_Name
    {
        set { _filed_Name = value; }
        get { return _filed_Name; }
    }

    public Sprite getSprite
    {
        set { GetSprite = value; }
        get { return GetSprite; }
    }

    public bool CostCheck
    {
        set { cansamon = value; }
        get { return cansamon; }
    }
    // Use this for initialization
    private void Start()
    {
      
    }
    public void CloneCard(string CardNum)
    {
        for (int i = 0; i < (int)CardList.Card.MAX_CARD_NUM; i++)
        {
            if (gameMgr.Get_Load_Sprite && CardNum == gameMgr.Get_Card_Sprite[i].name)
            {
                Getsprite = gameMgr.Get_Card_Sprite[i];
            }
        }
        card.GetComponent<Image>().sprite = Getsprite;
        image.sprite = Getsprite;
        GetSprite = Getsprite;
        Card_spriteRenderer.sprite = Getsprite;
        
    }

    public bool CanSamon()
    {
       CostCheck = gameMgr.CostCheck(Cost);
       return CostCheck;
    }

    public void Samon()
    {
        if (CostCheck)
        {
            gameMgr.Samon(Graphic, Cost, Combat, Power, Move, Move_Point, ActionPoint, image);
            Null();
        }
        else
        {
            return;
        }
    }
  
    public void Null()
    {
        Graphic = null;
        Cost = 0;
        Combat = 0;
        Power = 0;
        Move = 0;
        Move_Point = 0;
        ActionPoint = 0;
        image = null;
        GetGraphic = null;
        GetSprite = null;
        GetImage = null;
        this.gameObject.SetActive(false);
    }

    private void Move_Hand_Card_Date()
    {

    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TestDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] Image image;
    [SerializeField] GameMgr gameMgr;
    [SerializeField] UnitMgr unitMgr;
  //  [SerializeField] Unit unit;

    private Sprite nowSprite;
    private bool pointerExit = false;

    private void Awake()
    {
        image = this.gameObject.GetComponent<Image>(); 
    }
    // Use this for initialization
    void Start () {
        nowSprite = null;
        image.color = Vector4.one * 0.6f;
	}

    public void OnPointerEnter(PointerEventData pointereventData)
    {
        if (pointereventData.pointerDrag == null || int.Parse(this.name) > 3)
        {
            return;
        }
        if (nowSprite != null)
        {
            return;
        }
        Card Costcheck = pointereventData.pointerDrag.GetComponent<Card>();
        Image droppedImage = pointereventData.pointerDrag.GetComponent<Image>();
        if ( !Costcheck.CostCheck)
        {
            return;
        }
        image.sprite = droppedImage.sprite;
        image.color = Vector4.one  * 0.6f;
    }

    public void OnDrop(PointerEventData pointereventData)
    {

        if (pointereventData.pointerDrag == null || int.Parse(this.name) > 3 )
        {
            return;
        }
        if( nowSprite != false)
        {
            return;
        }
        if( pointereventData.pointerEnter == false || int.Parse(this.name) > 3)
        {
            pointerExit = true;
            gameMgr.CanSamon = false;
        }
        Card Costcheck = pointereventData.pointerDrag.GetComponent<Card>();
        Image droppedImage = pointereventData.pointerDrag.GetComponent<Image>();
        unitMgr.Unit_Num = int.Parse(this.name);
        if (!Costcheck.CostCheck)
        {
            return;
        }
        image.sprite = droppedImage.sprite;
        nowSprite = droppedImage.sprite;
        image.color = Vector4.one;
    }

    public void OnPointerExit(PointerEventData pointereventData)
    {
        if (pointereventData.pointerDrag == null || int.Parse(this.name) > 3)
        {
            return;
        }
        if( nowSprite == null)
        {
            image.color = Vector4.one * 0.6f;
        }
        image.sprite = nowSprite;
        image.color = Vector4.one;
        gameMgr.CanSamon = false;
    }
	

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tutorial_Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
	[SerializeField] GameObject Tap_Image = null;
	[SerializeField] GameObject Move_Buttom = null;
   // [SerializeField] GameObject Fream;
	[SerializeField] GameObject Hand = null;
	[SerializeField] GameObject Hand2 = null;
	[SerializeField] GameObject Tutorial_Select_effect = null;
	[SerializeField] GameObject Effect_Tutorial_Samon_Hand = null;
	[SerializeField] GameObject Tap_Arrow_End = null;
	[SerializeField] Tutorial_TurenEnd Tutorial_TurenEnd = null;
	[SerializeField] GameObject Samon_YUBI = null;
    private Sprite nowSprite;
//    private bool pointerExit = false;
    private int This_Name = 0;
	[SerializeField] GameObject Tutorial = null;
    [SerializeField] Vector3 Hand_Vector;
//    [SerializeField] Vector3 Hand_Vector2;
    [SerializeField] GameObject Effect_Tap;
    [SerializeField] Sprite poseidon = null;
	[SerializeField] Sprite sukeruton = null;
    [SerializeField] Sprite This_Unit_Image;
	[SerializeField] Image Player_Mp = null;
	[SerializeField] Sprite Farst_Saom_Mp = null;
    private void Awake()
    {
        This_Name = int.Parse(this.name);
        image = this.gameObject.GetComponent<Image>();
        Hand_Vector = Hand.gameObject.transform.position;
//        Hand_Vector2 = Hand2.gameObject.transform.position;

    }
    void Start()
    {
        nowSprite = null;

    }
    public void OnPointerEnter(PointerEventData pointereventData)
    {
        if (pointereventData.pointerDrag == null)
        {
            return;
        }
        if (This_Name != 2 || pointereventData.pointerDrag.GetComponent<Image>().sprite != poseidon)
        {
            if (This_Name == 1 && This_Name == 3)
            {
                return;

            }
            if (This_Name == 0 || pointereventData.pointerDrag.GetComponent<Image>().sprite != sukeruton)
            {
                return;

            }
        }
        else
        {
            Image droppedImage = pointereventData.pointerDrag.GetComponent<Image>();
            image.sprite = droppedImage.sprite;
            image.color = Vector4.one * 0.6f;
        }
       
    }

    public void OnDrop(PointerEventData pointereventData)
    {
        Image droppedImage = pointereventData.pointerDrag.GetComponent<Image>();

        if (pointereventData.pointerDrag == null)
        {
            return;
        }
        if (This_Name == 2 && pointereventData.pointerDrag.GetComponent<Image>().sprite == poseidon)
        {
            Player_Mp.sprite = Farst_Saom_Mp;
            image.sprite = droppedImage.sprite;
            nowSprite = droppedImage.sprite;
            Samon_YUBI.SetActive(false);
            Hand.SetActive(false);
            image.color = Vector4.one;
            Invoke("Tutorial_SetActive", 2.0f);
            Tutorial_TurenEnd.drop_unit_num = int.Parse(this.name);
            return;

        }
        if (This_Name == 0 && pointereventData.pointerDrag.GetComponent<Image>().sprite == sukeruton)
        {
            image.sprite = droppedImage.sprite;
            nowSprite = droppedImage.sprite;
            Hand2.SetActive(false);
            image.color = Vector4.one;
            Effect_Tutorial_Samon_Hand.SetActive(false);
            Tutorial_Select_effect.SetActive(true);
            return;
        }

            if ( This_Name == 1 && This_Name == 3 || pointereventData.pointerDrag.GetComponent<Image>().sprite != poseidon)
        {
            Hand.SetActive(true);
            Hand.transform.position = Hand_Vector;
            return;
        }
        if( This_Name == 0 || pointereventData.pointerDrag.GetComponent<Image>().sprite != sukeruton)
        {
            Hand.SetActive(true);
            Hand.transform.position = Hand_Vector;
            return;
        }
        
        Hand.SetActive(true);
        Hand.transform.position = Hand_Vector;
        return;
        
    }

    public void OnPointerExit(PointerEventData pointereventData)
    {
        if (pointereventData.pointerDrag == null)
        {
            return;
        }
        if (This_Name != 2 || pointereventData.pointerDrag.GetComponent<Image>().sprite != poseidon)
        {
            return;

        }
        else
        {
          
        }
        if (This_Name != 0 || pointereventData.pointerDrag.GetComponent<Image>().sprite != sukeruton)
        {
            return;

        }
        else
        {
            image.sprite = nowSprite;
            image.color = Vector4.one * 0.6f;
          
        }
    }

    void Tutorial_SetActive()
    {
        Tutorial.SetActive(true);
        Hand.SetActive(false);

    }

    public void Tap_End_Arrow_SetActive()
    {
        Tap_Arrow_End.SetActive(true);
    }
    public void Tap_Unit()
    {
        if(this.gameObject.GetComponent<Image>().sprite == null)
        {
            return;
        }
        Tap_Image.SetActive(true);
        Move_Buttom.SetActive(true);
    }
}

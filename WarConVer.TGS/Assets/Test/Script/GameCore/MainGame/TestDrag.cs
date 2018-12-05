using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TestDrag : MonoBehaviour, IDragHandler, IBeginDragHandler,IPointerEnterHandler, IEndDragHandler
{


    private GameObject DragCardObject;
    private Card card;
    private Transform canvas_transform;
    private Transform start_position;
	[SerializeField] UnitMgr unitMgr = null;

    private void Awake()
    {
        canvas_transform = transform.parent.parent;
        card = this.gameObject.GetComponent<Card>();
        start_position = this.gameObject.transform;

    }

    // Use this for initialization
    void Start () {
	}

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
    }
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        card.CanSamon();
        if (card.CostCheck)
        {
            CreatDragCard();
            DragCardObject.gameObject.transform.position = pointerEventData.position;
        }
        else
        {
            print("CostOver!!");
            return;
        }
    }
    public void OnDrag(PointerEventData pointerEventData)
    {
        if (card.CostCheck)
        {
            DragCardObject.gameObject.transform.position = pointerEventData.position;
        }
        else
        {
            DragCardObject.gameObject.transform.position = start_position.position;

        }
    }
    public void OnEndDrag(PointerEventData pointerEventData)
    {
       
        if (card.CostCheck)
        {
            card.Samon();
            unitMgr.Data_Move_Unit();
        }
        else
        {
            DragCardObject.gameObject.transform.position = start_position.position;

        }
    }

    private void CreatDragCard()
    {
        DragCardObject = new GameObject("DragCardObject");
        DragCardObject.transform.SetParent(canvas_transform);
        DragCardObject.transform.SetAsLastSibling();
        DragCardObject.transform.localScale = Vector3.one;

        CanvasGroup canvasGroup = DragCardObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Image cardImage = DragCardObject.AddComponent<Image>();
        Image source_image = GetComponent<Image>();

        cardImage.sprite = source_image.sprite;
        cardImage.rectTransform.sizeDelta = source_image.rectTransform.sizeDelta;
        cardImage.color = source_image.color;
        cardImage.material = source_image.material;

        DragCardObject.GetComponent<Image>().color = Vector4.one * 0.6f;

        

    }

}

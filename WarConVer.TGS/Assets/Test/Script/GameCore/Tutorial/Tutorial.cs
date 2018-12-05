using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class Tutorial : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IEndDragHandler
{
	[SerializeField] GameObject Farst_Tutorial = null;
    [SerializeField] GameObject Tutorial_Image;
    private Card card;
	private Transform canvas_transform = null;
    private Transform start_position;

    private void Start()
    {
        Invoke("Farst_Tutorial_SetActive", 2.0f);
    }
    void This_SetActive()
    {
        this.gameObject.SetActive(true);
    }
    public void Farst_Tutorial_SetActive()
    {
        Farst_Tutorial.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {

    }
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        this.gameObject.transform.position = pointerEventData.position;
    }
    public void OnDrag(PointerEventData pointerEventData)
    {
        this.gameObject.transform.position = pointerEventData.position;
        this.gameObject.SetActive(false);

    }
    public void OnEndDrag(PointerEventData pointerEventData)
    {
    }
    
    private void CreatDragCard()
    {
        Tutorial_Image = new GameObject("DragCardObject");
        Tutorial_Image.transform.SetParent(canvas_transform);
        Tutorial_Image.transform.SetAsLastSibling();
        Tutorial_Image.transform.localScale = Vector3.one;

        CanvasGroup canvasGroup = Tutorial_Image.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Image cardImage = Tutorial_Image.AddComponent<Image>();
        Image source_image = GetComponent<Image>();

        cardImage.sprite = source_image.sprite;
        cardImage.rectTransform.sizeDelta = source_image.rectTransform.sizeDelta;
        cardImage.color = source_image.color;
        cardImage.material = source_image.material;

        Tutorial_Image.GetComponent<Image>().color = Vector4.one * 0.6f;



    }
}

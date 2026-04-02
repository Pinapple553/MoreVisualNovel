using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIButtonHover : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;

    public Image BGImage;
    public Color normalColor = Color.black;
    public Color HoverlColor = Color.white;

    public TMP_Text btnText;
    public Color normalTextColor = Color.white;
    public Color HoverTextColor = Color.black;
	public Color selectedTextColor = Color.black;
	public Color PressedTextColor = Color.red;

    void Start()
    {

        if (BGImage != null)
            BGImage.color = normalColor;
    }

    public void Selected(bool selected){
        if (selected)
        {
            
            btnText.color = selectedTextColor;
        }
        else
        {
            if (BGImage != null) BGImage.color = normalColor;
            btnText.color = normalTextColor;
		}

	}
	public void OnPointerEnter(PointerEventData eventData)
    {
        if (BGImage != null) BGImage.color = HoverlColor;
        btnText.color = HoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (BGImage != null) BGImage.color = normalColor;
        btnText.color = normalTextColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        btnText.color = PressedTextColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        btnText.color = normalTextColor;
    }
}
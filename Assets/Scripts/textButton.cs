using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class textButton : MonoBehaviour
{
	[SerializeField] private TMP_Text buttonText;
	[SerializeField] private Button buttonButton;
	[SerializeField] private Image buttonImage;
	[SerializeField] private Color normalTextColor;
	[SerializeField] private Color activeTextColor;
	[SerializeField] private Color disabledTextColor;

	public void ButtonActive(bool active){
		if (active) {
			buttonText.color = activeTextColor;
		}
		else {
			buttonText.color = normalTextColor;
		}
	}
	public void ButtonDisabled(bool disabled)
	{
		if (disabled)
		{
			buttonButton.SetEnabled(false);
			buttonText.color = disabledTextColor;
		}
		else
		{
			buttonButton.SetEnabled(true);
			buttonText.color = normalTextColor;
		}
	}
}

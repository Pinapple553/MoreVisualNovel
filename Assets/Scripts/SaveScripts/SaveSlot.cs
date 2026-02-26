using Ink.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private int page;
    [SerializeField] private int slot;

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text slotnumber;
    [SerializeField] private GameObject optionsContainer;

    [SerializeField] private SaveUIManager uIManager;


    public void SetData(GameData data, int pageNum =1, int slotNum =1)
    {
        page = pageNum;
        slot = slotNum;
        int number = (pageNum-1)*16+slotNum;


        slotnumber.text = "NO."+number.ToString();
        if (data == null)
        {
            image.color = Color.gray;
        }
        else
        {
            image = data.background;
        }
    }


}

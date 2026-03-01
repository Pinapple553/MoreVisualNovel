using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private int page;
    [SerializeField] private int slot;

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text slotText;
    [SerializeField] private GameObject optionsContainer;

    [SerializeField] private SaveLoadUI uIManager; 

    public void OnSlotClicked()
    {
        SaveLoadUI.Instance.OpenSlot(this);
    }
    public void SetData(GameData data, int pageNum =1, int slotNum =1)
    {
        page = pageNum;
        slot = slotNum;
        //int number = (pageNum-1)*16+slotNum;
        //slotText.text = "NO."+number.ToString();
        Sprite sprite = SaveLoadUI.Instance.GetSaveImage(pageNum, slotNum);

        if (sprite != null)
        {
            image.sprite = sprite;
            image.color = Color.white;
        }
        else
        {
            image.color = Color.gray;
        }
    }

    public void ShowOptions(bool show)
    {
        //uIManager.HideAllOptions();
        optionsContainer.SetActive(show);
        //need to make it so you cant select other slots while options are open
    }
    public void HideOptions()
    {
        optionsContainer.SetActive(false);
    }
    public void OnSavePressed()
    {
        SaveLoadManager.Instance.SaveGame(GameManager.Instance.currentStory, page, slot);
        uIManager.OpenSavePage(page); //refresh the save slots to show the new save data
    }
    public void OnLoadPressed()
    {
        if (!SaveLoadManager.Instance.HasSave(page, slot))
            return;

        GameManager.Instance.loadFromSave = true;
        GameManager.Instance.loadPage = page;
        GameManager.Instance.loadSlot = slot;

        SceneManager.LoadScene("GameScene");
    }
    public void OnDeletePressed()
    {
        SaveLoadManager.Instance.DeleteSave(page, slot);
        SaveLoadUI.Instance.OpenSavePage(page);
    }
    public void Refresh()
    {
        // Update preview text, time, etc
    }

}

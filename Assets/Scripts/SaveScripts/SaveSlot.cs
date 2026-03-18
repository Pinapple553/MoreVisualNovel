using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private int page;
    [SerializeField] private int slot;

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text slotNumber;
    [SerializeField] private TMP_Text slotdate;
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    public void SetData(GameData data, int pageNum, int slotNum)
    {
        page = pageNum;
        slot = slotNum;
        int number = (pageNum-1)*16+slotNum;
        slotNumber.text = "NO."+number.ToString();

        if (data != null)
        {
            slotdate.text = data.dateTime;
            image.sprite = Resources.Load<Sprite>($"Backgrounds/{data.background_id}");
        }
        else
        {
            slotdate.text = "Empty";
            image.color = Color.black;
        }
    }
    public void ShowOptions(bool show)
    {
        SaveLoadUI.Instance.HideAllOptions();
        if (GameManager.Instance.currentStory == null)  
        {
            saveButton.interactable = false;
        }
        else
        {
            saveButton.interactable = true;
        }
        if (!SaveLoadManager.Instance.HasSave(page, slot))
        {
            loadButton.interactable = false;
            deleteButton.interactable = false;
        }
        else
        {
            loadButton.interactable = true;
            deleteButton.interactable = true;
        }

        optionsContainer.SetActive(show);
    }
    public void HideOptions()
    {
        optionsContainer.SetActive(false);
    }
    public void OnSavePressed()
    {
        SaveLoadManager.Instance.SaveGame(GameManager.Instance.currentStory, page, slot);
        SaveLoadUI.Instance.OpenSavePage(page); //refresh the save slots to show the new save data
    }
    public void OnLoadPressed()
    {
        if (!SaveLoadManager.Instance.HasSave(page, slot))
        {
            return;
        }
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
    public void OnSlotHover()
    {
        SaveLoadUI.Instance.OpenSlot(this);
    }

    public string GetSaveNumber()
    {
        int number = (page - 1) * 16 + slot;
        return number.ToString();
    }
    public int GetPage()
    {
        return page;
    }
    public int GetSlot()
    {
        return slot;
    }
    public string GetSaveDate()
    {
        GameData data = SaveLoadManager.Instance.GetSaveData(page, slot);
        if (data != null)
        {
            return data.dateTime;
        }
        return "No Save";
    }
    public string GetSaveText()
    {
        GameData data = SaveLoadManager.Instance.GetSaveData(page, slot);
        if (data != null)
        {
            return data.previewText;
        }
        return "No Save";
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

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
        slotNumber.text = GetSaveNumber();

        if (data != null)
        {
            slotdate.text = data.dateTime;
            string path = Application.persistentDataPath + $"/Saves/save_{page}_{slot}.png";
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                image.sprite = Sprite.Create(tex,new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
        }
        else
        {
            slotdate.text = "Empty";
            image.sprite = null;
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
        string text = "GONE";
        if (page == 0)
        {
            text = "Auto."+slot;
        }
        else if( page == -1)
        {
            text = "Quick." + slot;
        }
        else 
        {
            text = "NO."+((page - 1) * 16 + slot);
        }
        return text;
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
    public Sprite GetSaveImage()
    {
        return image.sprite;
    }
}

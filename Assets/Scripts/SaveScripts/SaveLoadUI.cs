using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    public static SaveLoadUI Instance;
    private SaveSlot currentlyOpenSlot;

    [SerializeField] private GameObject saveContainer;
    [SerializeField] private GameObject saveSlotPrefab;

    [SerializeField] private GameObject openSaveContainer;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ShowSaveSlots(1); 
    }
    public void OpenSavePage(int page) //opens the save page for the specified page number
    {
        Debug.Log("Opening save page: " + page);
        ShowSaveSlots(page);
    }


    private void ShowSaveSlots(int page) //shows the save slots for the specified page
    {
        // Clear existing slots
        foreach (Transform child in saveContainer.transform)
        {
            Destroy(child.gameObject);
        }
        // Create new slots
        for (int i = 1; i <= 16; i++)
        {
            GameObject slot = Instantiate(saveSlotPrefab, saveContainer.transform);
            SaveSlot saveSlot = slot.GetComponent<SaveSlot>();
            GameData data = SaveLoadManager.Instance.GetSaveData(page, i); 
            saveSlot.SetData(data, page, i);
        }
    }

    public Sprite GetSaveImage(int page, int slot)
    {
        GameData data = SaveLoadManager.Instance.GetSaveData(page, slot);
        if (data != null)
        {
            return Resources.Load<Sprite>("Backgrounds/" + data.background_id); //loads the background sprite for the save slot preview image
        }
        return null;
    }
    public void OpenSlot(SaveSlot slot)
    {
        if (currentlyOpenSlot != null)
            currentlyOpenSlot.ShowOptions(false);

        currentlyOpenSlot = slot;
        slot.ShowOptions(true);
    }

    public void CloseAll()
    {
        if (currentlyOpenSlot != null)
        {
            currentlyOpenSlot.ShowOptions(false);
            currentlyOpenSlot = null;
        }
    }
    public void HideAllOptions()
    {
        foreach (Transform child in saveContainer.transform)
        {
            SaveSlot slot = child.GetComponent<SaveSlot>();
            if (slot != null)
            {
                slot.HideOptions();
            }
        }

    }
    public void OnBackgroundClicked()
    {
        CloseAll();
    }
}

using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveUIManager : MonoBehaviour
{
    [SerializeField] private GameObject saveContainer;
    [SerializeField] private GameObject saveSlotPrefab;

    [SerializeField] private GameObject openSaveContainer;

    private void Start()
    {
        ShowSaveSlots(1); 
    }
    public void OpenSavePage(int page)
    {
        Debug.Log("Opening save page: " + page);
        ShowSaveSlots(page);
    }


    private void ShowSaveSlots(int page)
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
            GameData data = SaveLoadManager.Instance.GetSaveData(0, i); // Assuming page 0 for now
            saveSlot.SetData(data, page, i);
        }
    }

    public void HideAllOptions()
    {

    }
}

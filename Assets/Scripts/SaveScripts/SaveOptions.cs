using Unity.VisualScripting;
using UnityEngine;
using Ink.Runtime;

public class SaveOptions : MonoBehaviour
{
    [SerializeField] private int page;
    [SerializeField] private int slot;

    [SerializeField] private GameObject optionsContainer;

    [SerializeField] private SaveUIManager uIManager;
    [SerializeField] private StoryManager storyManager;
    public void ShowOptions()
    {
        optionsContainer.SetActive(true);
        //need to make it so you cant select other slots while options are open
    }

    public void HideOptions()
    {
        optionsContainer.SetActive(false);
    }

    public void Save()
    {
        Story currentStory = storyManager.GetCurrentStory();
        if (currentStory != null)
        {
            SaveLoadManager.Instance.SaveGame(currentStory, page, slot);
            uIManager.OpenSavePage(page);
        }
    }

    public void Load()
    {
        Story currentStory = storyManager.GetCurrentStory();
        if (currentStory != null)
        {
            bool success = SaveLoadManager.Instance.LoadGame(currentStory, page, slot);
            if (success)
            {
                // Optionally, you can add some feedback to the player here
                Debug.Log("Game loaded successfully.");
            }
            else
            {
                Debug.LogWarning("Failed to load game.");
            }
        }
    }
}

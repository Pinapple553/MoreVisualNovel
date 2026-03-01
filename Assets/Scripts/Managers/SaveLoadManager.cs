using Ink.Runtime;
using System.IO;
using UnityEngine;
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    [SerializeField] private BackgroundManager backgroundManager;
    private string saveFolder;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        saveFolder = Application.persistentDataPath + "/Saves/";
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder); //if the folder doesnt exist, create it
        }
    }
    private string GetSlotPath(int page, int slot)
    {
        return saveFolder + $"save_{page}_{slot}.json"; //returns save path, example: "save_1_1.json" for page 1, slot 1
    }
    public GameData GetSaveData(int page, int slot)
    {
        string path = GetSlotPath(page, slot);
        if (!File.Exists(path))
        {
            return null;
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json); //creates a GameData object from the json data in the save file
    }
    public void SaveGame(Story story, int page, int slot)
    {
        // Create a GameData object to hold the current state of the story and other relevant information
        GameData data = new GameData();
        data.storyJson = story.state.ToJson();
        data.previewText = story.currentText;
        data.dateTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        data.background_id = backgroundManager.CurrentBackgroundID;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSlotPath(page, slot), json);
    }
    public GameData LoadGameData(int page, int slot)
    {
        string path = GetSlotPath(page, slot);

        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }

    public bool HasSave(int page, int slot)
    {
        return File.Exists(GetSlotPath(page, slot));
    }

    public void DeleteSave(int page, int slot)
    {
        string path = GetSlotPath(page, slot);
        if (File.Exists(path))
            File.Delete(path);
    }
}


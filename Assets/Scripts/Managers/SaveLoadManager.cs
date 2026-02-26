using System.IO;
using UnityEngine;
using Ink.Runtime;
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private string saveFolder;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        saveFolder = Application.persistentDataPath + "/Saves/";

        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);
    }

    private string GetSlotPath(int page, int slot)
    {
        return saveFolder + $"save_{page}_{slot}.json";
    }
    public GameData GetSaveData(int page, int slot)
    {
        string path = GetSlotPath(page, slot);
        if (!File.Exists(path))
            return null;
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }

    public void SaveGame(Story story, int page, int slot)
    {
        GameData data = new GameData();
        data.storyJson = story.state.ToJson();
        data.dateTime = System.DateTime.Now.ToString();
        data.previewText = story.currentText;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSlotPath(page, slot), json);
    }
    public bool LoadGame(Story story, int page, int slot)
    {
        string path = GetSlotPath(page, slot);

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save found.");
            return false;
        }

        string json = File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(json);
        story.state.LoadJson(data.storyJson);
        return true;
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


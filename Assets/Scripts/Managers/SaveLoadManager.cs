using Ink.Runtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private string saveFolder;

    private int slotsPerPage = 16;
    private int quickPage = -1;
    private int autoPage = 0;

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
        data.background_id = GameManager.Instance.currentBackgroundID;

        Texture2D screenshot = GameManager.Instance.GetLatestScreenshot();
        if (screenshot != null)
        {
            byte[] bytes = screenshot.EncodeToPNG();
            string imagePath = Application.persistentDataPath + $"/Saves/save_{page}_{slot}.png";
            File.WriteAllBytes(imagePath, bytes);
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSlotPath(page, slot), json);
    }
    public GameData LoadGameData(int page, int slot)
    {
        string path = GetSlotPath(page, slot);
        if (!File.Exists(path)) {  return null; }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }

    public bool HasSave(int page, int slot)
    {
        return File.Exists(GetSlotPath(page, slot));
    }
    public void DeleteSave(int page, int slot)
    {
        string jsonPath = GetSlotPath(page, slot);
        if (File.Exists(jsonPath)) File.Delete(jsonPath);
        string imagePath = Application.persistentDataPath + $"/Saves/save_{page}_{slot}.png";
        if (File.Exists(imagePath)) File.Delete(imagePath);
    }

    public void QuickSave(Story story)
    {
        int slot = GetNextSlot(quickPage);
        SaveGame(story, quickPage, slot);
    }
    public void QuickLoad()
    {
        int slot = GetLatestSlot(quickPage);
        if (slot == 0) return;

        GameManager.Instance.loadFromSave = true;
        GameManager.Instance.loadPage = quickPage;
        GameManager.Instance.loadSlot = slot;

        GameManager.Instance.OpenScene("GameScene");
    }
    public void AutoSave(Story story)
    {
        int slot = GetNextSlot(autoPage);
        SaveGame(story, autoPage, slot);
    }
    private int GetLatestSlot(int page)
    {
        int latestSlot = 0;
        System.DateTime latestTime = System.DateTime.MinValue;

        for (int i = 1; i <= slotsPerPage; i++)
        {
            if (!HasSave(page, i)) continue;

            GameData data = GetSaveData(page, i);
            if (data != null)
            {
                if (System.DateTime.TryParse(data.dateTime, out var time))
                {
                    if (time > latestTime)
                    {
                        latestTime = time;
                        latestSlot = i;
                    }
                }
            }
        }
        return latestSlot;
    }
    private int GetNextSlot(int page)
    {
        int oldestSlot = 1;
        System.DateTime oldestTime = System.DateTime.MaxValue;
        for (int i = 1; i <= slotsPerPage; i++)
        {
            if (!HasSave(page, i))
            {
                return i;
            }
            GameData data = GetSaveData(page, i);
            if (data != null)
            {
                if (System.DateTime.TryParse(data.dateTime, out var time))
                {
                    if (time < oldestTime)
                    {
                        oldestTime = time;
                        oldestSlot = i;
                    }
                }
            }
        }
        return oldestSlot;
    }
}


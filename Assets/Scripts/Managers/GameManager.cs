using Ink.Runtime;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool loadFromSave = false;
    public int loadPage;
    public int loadSlot;
    public Story currentStory;
    public bool isInGame = false;
    private string lastOpenScene;

    public string currentBackgroundID; 
    private Texture2D latestScreenshot;

    private void Awake()
    {
        //makes sure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OpenScene(string scene) //opens the specified scene and saves the name of the currently open scene so it can be returned to later
    {
        lastOpenScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }
    public void CloseScene() //returns to the last open scene, if there is one, otherwise it returns to the main menu
    {
        if (string.IsNullOrEmpty(lastOpenScene))
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        SceneManager.LoadScene(lastOpenScene);
    }
    public void NewStory(){
		lastOpenScene = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene("GameScene");
		currentStory =
	}
    public void QuitGame() //quits the game, if in the editor it stops play mode instead
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    //saveload thumbnail
    public IEnumerator CoroutineScreenshot()
    {
        yield return new WaitForEndOfFrame();
       
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false); Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0); 
        screenshotTexture.Apply();

        int targetWidth = 200;
        int targetHeight = 180;

        float scale = (float)targetHeight / height;
        int scaledWidth = Mathf.RoundToInt(width *scale);
        Texture2D scaled = ResizeTexture(screenshotTexture, scaledWidth, targetHeight);
        int startX = (scaledWidth - targetWidth) / 2;
        Texture2D finalTex = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);
        finalTex.SetPixels(scaled.GetPixels(startX, 0, targetWidth, targetHeight));
        finalTex.Apply();

        latestScreenshot = finalTex;
    }
    public Texture2D GetLatestScreenshot()
    {
        return latestScreenshot;
    }
    public Texture2D ResizeTexture(Texture2D source, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, false);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
}
using Ink.Runtime;
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

    public void QuitGame() //quits the game, if in the editor it stops play mode instead
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputSystem controls;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject settingsScreen;

    private string lastOpenScene;
    private void OnEnable()
    {
        controls.UI.Enable();
    }
    private void OnDisable()
    {
        controls.UI.Disable();
    }
    private void Awake()
    {
        controls = new InputSystem();
        controls.UI.Pause.performed += ctx =>
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                PauseGame();
            }
        };
    }
    public void OpenScene(string scene)
    {
        lastOpenScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }
    public void CloseScene()
    {
        if (lastOpenScene == null)
        {
            SceneManager.LoadScene("MainMenu");
        }
        SceneManager.LoadScene(lastOpenScene);
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
    public void PauseGame()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
    }
    public void ShowSettings()
    {
        settingsScreen.SetActive(!settingsScreen.activeSelf);
    }
}

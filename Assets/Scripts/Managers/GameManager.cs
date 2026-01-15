using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputSystem controls;
    [SerializeField]
    private GameObject pauseScreen;
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
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadSavesScene()
    {
        SceneManager.LoadScene("LoadSavesScene");
    }
    public void SaveGameScene()
    {
        SceneManager.LoadScene("SaveGameScene");
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void OpenCutscenes()
    {
        SceneManager.LoadScene("Cutscenes");
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private InputSystem controls;
    [SerializeField]
    private GameObject pauseScreen;
    private void Awake()
    {
        controls = new InputSystem();
        controls.UI.Pause.performed += ctx => PauseGame();

    }
    private void OnEnable()
    {
        controls.UI.Enable();
    }
    private void OnDisable()
    {
        controls.UI.Disable();
    }
    void Update()
    {
        //NOTE: delete this quit functionality when a Pause Menu is added!
        // if (Input.GetKey("escape")){
        //         Application.Quit();
        // }

        // Stat tester:
        //if (Input.GetKey("p")){
        //       Debug.Log("Player Stat = " + playerStat1);
        //}
    }

    // void UpdateScore () {
    //        textGameObject.text = "Score: " + score; }

    public void StartGame()
    {
        Debug.Log("Start Game button clicked");
        SceneManager.LoadScene("Scene1");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
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

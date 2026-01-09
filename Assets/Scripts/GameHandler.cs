using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{

    public static int playerStat1;
    // public TMP_Text textGameObject;

    // void Start () { UpdateScore (); }

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

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}
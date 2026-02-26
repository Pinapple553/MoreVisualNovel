using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private AudioEffectsManager audioEffectsManager;
    [SerializeField]
    private StoryManager storyManager;
    [SerializeField]
    private Slider BGMslider;
    [SerializeField]
    private Slider SFXslider;
    [SerializeField]
    private Slider textSpeedSlider;
    [SerializeField]
    private Slider autoTextSpeedSlider;
    [SerializeField]
    private Slider autoDelaySlider;
    [SerializeField]
    private Toggle fullscreenToggle;
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private AudioMixer mainMixer;
    [SerializeField]
    private AudioMixer bgmMixer;
    [SerializeField]
    private AudioMixer sfxMixer;

    private Resolution[] resolutions;
    int currentResolutionIndex;

    private void Start()
    {
        ShowResolutions();
        SetTextSliderSettings();


    }
    public void SetBGMVolume(float volume)
    {
        bgmMixer.SetFloat("bgmVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("sfxVolume", volume);
    }
    public void ToggleFullscrean(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void ShowResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> resolutionTexts = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionTexts.Add(resolutions[i].height +" x " + resolutions[i].width);
            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionTexts);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void TextSpeed(float speed)
    {
        speed = 0.5f / speed;
        storyManager.SetTextSpeed(speed);
        Debug.Log(speed);
    }
    public void AutoTextSpeed(float speed)
    {
        speed = 1f / speed;
        storyManager.SetAutoTextSpeed(speed);
    }
    public void AutoDelay(float delay)
    {
        storyManager.SetAutoDelay(delay);
    }

    private void SetTextSliderSettings()
    {
        textSpeedSlider.value = storyManager.currentTextSpeed*100f;
        autoTextSpeedSlider.value = storyManager.autoTextSpeed*100f;
        textSpeedSlider.value = storyManager.autoDelay;
    }
    
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private AudioEffectsManager audioEffectsManager;
    [SerializeField]
    private Slider BGMslider;
    [SerializeField]
    private Slider SFXslider;
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
        audioEffectsManager.PlayMusic("short_music", true);
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
}


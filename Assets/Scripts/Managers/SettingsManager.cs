using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioEffectsManager audioEffectsManager;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private Slider BGMslider;
    [SerializeField] private Slider SFXslider;
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private Slider autoTextSpeedSlider;
    [SerializeField] private Slider autoDelaySlider;

    [SerializeField] private AudioMixer bgmMixer;
    [SerializeField] private AudioMixer sfxMixer;

    private Resolution[] resolutions;
    private int currentResolutionIndex;
    //for future text write example
    private float tempTextSpeed;
    private float tempAutoTextSpeed;
    private float tempAutoDelay;
    //[SerializeField] private TMP_Text previewText;
    //[SerializeField] private string previewString = "Example text...";

    private const float defultTextSpeed = 0.05f;
    private const float defultAutoTextSpeed = 0.05f;
    private const float defultAutoDelay = 2;
    private const float defultVolume = 0.5f;

    //private Coroutine previewCoroutine;

    private void Start()
    {
        ShowResolutions();
        LoadSettings();
    }
    public void SetBGMVolume(float volume)
    {
        bgmMixer.SetFloat("bgmVolume", Mathf.Log10(volume)*20);
        if(volume ==0)
        {
            bgmMixer.SetFloat("bgmVolume", -80);
        }
    }
    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        if (volume == 0)
        {
            bgmMixer.SetFloat("bgmVolume", -80);
        }
    }
    public void MuteAll()
    {

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
            resolutionTexts.Add(resolutions[i].width +" x " + resolutions[i].height);
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
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
    }
    public void TextSpeed(float speed)
    {
        tempTextSpeed = speed;
        //if (previewCoroutine != null) { StopCoroutine(previewCoroutine); }
        //previewCoroutine = StartCoroutine(PreviewText(speed));
    }
    public void AutoTextSpeed(float speed)
    {
        tempAutoTextSpeed = speed;
    }
    public void AutoDelay(float delay)
    {
        tempAutoDelay = delay;
    }
    private void LoadSettings()
    {
        BGMslider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXslider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        textSpeedSlider.value = PlayerPrefs.GetFloat("textSpeed", 0.05f);
        autoTextSpeedSlider.value = PlayerPrefs.GetFloat("autoTextSpeed", 0.05f);
        autoDelaySlider.value = PlayerPrefs.GetFloat("autoDelay", 2);

        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = resIndex;
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        SetResolution(resIndex);
    }
    public void RevertSettings()
    {
        LoadSettings();
    }
    public void ApplySettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", BGMslider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXslider.value);
        PlayerPrefs.SetFloat("textSpeed", textSpeedSlider.value);
        PlayerPrefs.SetFloat("autoTextSpeed", autoTextSpeedSlider.value);
        PlayerPrefs.SetFloat("autoDelay", autoDelaySlider.value);
        PlayerPrefs.Save();
        SetResolution(resolutionDropdown.value);
    }
   public void ResetSettings()
    {
        resolutionDropdown.value = currentResolutionIndex;
        fullscreenToggle.isOn = true;

        BGMslider.value = defultVolume;
        SFXslider.value = defultVolume;
        textSpeedSlider.value = defultTextSpeed;
        autoTextSpeedSlider.value = defultAutoTextSpeed;
        autoDelaySlider.value = defultAutoDelay;

    }
    /*private IEnumerator PreviewText(float speed)
    {
        while (true)
        {
            previewText.text = "";
            foreach (char c in previewString)
            {
                previewText.text += c;
                yield return new WaitForSeconds(speed);
            }
            yield return new WaitForSeconds(1f);
        }
    }*/

}


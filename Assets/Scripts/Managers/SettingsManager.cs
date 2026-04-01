using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
   AudioEffectsManager audioEffectsManager = AudioEffectsManager.Instance;

	[SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private Slider BGMslider;
    [SerializeField] private Slider SFXslider;
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private Slider autoTextSpeedSlider;
    [SerializeField] private Slider autoDelaySlider;

    [SerializeField] private AudioMixer bgmMixer;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private Toggle muteToggle;

	[SerializeField] private Toggle skipAfterChoice;
	[SerializeField] private Toggle skipUnseen;

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
    private int defaultResolutionIndex;

    //private Coroutine previewCoroutine;

    private void Start()
    {
        ShowResolutions();
        defaultResolutionIndex = currentResolutionIndex;
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
            sfxMixer.SetFloat("sfxVolume", -80);
        }
    }
    public void MuteAll(bool active)
    {
        if (active)
        {
            SetBGMVolume(0);
            SetSFXVolume(0);

		}
        else
        {
            SetBGMVolume(BGMslider.value);
            SetSFXVolume(SFXslider.value);
        }
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
	public void ShowResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> resolutionTexts = new List<string>();
        List<Resolution> uniqueResolutions = new List<Resolution>();
        HashSet<string> seen = new HashSet<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string key = resolutions[i].width + "x" + resolutions[i].height;
            if (seen.Contains(key)) continue;
            seen.Add(key);

            uniqueResolutions.Add(resolutions[i]);
            resolutionTexts.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = uniqueResolutions.Count - 1;
            }
        }
        resolutions = uniqueResolutions.ToArray();

        resolutionDropdown.AddOptions(resolutionTexts);
        resolutionDropdown.value = currentResolutionIndex;

        Resolution current = Screen.currentResolution;
        bool foundCurrent = false;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == current.width &&
                resolutions[i].height == current.height)
            {
                foundCurrent = true;
                break;
            }
        }

        if (!foundCurrent)
        {
            List<Resolution> tempList = new List<Resolution>(resolutions);
            tempList.Add(current);

            resolutions = tempList.ToArray();
            resolutionTexts.Add(current.width + " x " + current.height);

            currentResolutionIndex = resolutions.Length - 1;
        }

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
		fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
		muteToggle.isOn = PlayerPrefs.GetInt("muted", 0)==1;
        MuteAll(muteToggle.isOn);
		skipAfterChoice.isOn = PlayerPrefs.GetInt("skipAfterChoice", 0)==1;
		skipUnseen.isOn = PlayerPrefs.GetInt("skipUnseen", 0)==1;

		int resIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = resIndex;

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
		PlayerPrefs.SetInt("muted", muteToggle.isOn ? 1:0);
		PlayerPrefs.SetInt("skipAfterChoice", skipAfterChoice.isOn ? 1 : 0);
		PlayerPrefs.SetInt("skipUnseen", skipUnseen.isOn ? 1 : 0);

		PlayerPrefs.Save();
        SetResolution(resolutionDropdown.value);
    }
   public void ResetSettings()
    {
        ShowResolutions();
        resolutionDropdown.value = defaultResolutionIndex;
        fullscreenToggle.isOn = true;

        BGMslider.value = defultVolume;
        SFXslider.value = defultVolume;
        textSpeedSlider.value = defultTextSpeed;
        autoTextSpeedSlider.value = defultAutoTextSpeed;
        autoDelaySlider.value = defultAutoDelay;

        muteToggle.isOn = false;
        skipUnseen.isOn = false;
        skipAfterChoice.isOn = false;
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


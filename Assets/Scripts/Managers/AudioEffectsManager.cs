using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioEffectsManager : MonoBehaviour
{
	public static AudioEffectsManager Instance;

    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource backgroundMusicSource;
    [SerializeField] private AudioMixer bgmMixer;
    [SerializeField] private AudioMixer sfxMixer;
	[System.Serializable]
    public class MusicTrack
    {
        public string musicName;
        public AudioClip audio;
    }
    public List<MusicTrack> musicTracks;
    
    [System.Serializable]
    public class SoundEffect
    {
        public string sfxName;
        public AudioClip audio;
    }
    public List<SoundEffect> soundEffects;

    private Dictionary<string, AudioClip> lookupMusic; //lookup dictionary for quick access to expressions
    private Dictionary<string, AudioClip> lookupSFX; //lookup dictionary for quick access to expressions


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);

        backgroundMusicSource.loop = true;

        lookupMusic = new Dictionary<string, AudioClip>();
        foreach (var music in musicTracks)
        {
            lookupMusic[music.musicName] = music.audio;
        }

        lookupSFX = new Dictionary<string, AudioClip>();
        foreach (var sfx in soundEffects)
        {
            lookupSFX[sfx.sfxName] = sfx.audio;
        }
    }
    void Start(){
        SetVolume();
    }
    void SetVolume(){
		bgmMixer.SetFloat("bgmVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 0.5f)) * 20);
		if (PlayerPrefs.GetFloat("MusicVolume", 0.5f) == 0)
		{
			bgmMixer.SetFloat("bgmVolume", -80);
		}
		sfxMixer.SetFloat("sfxVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 0.5f)) * 20);
		if (PlayerPrefs.GetFloat("SFXVolume", 0.5f) == 0)
		{
			sfxMixer.SetFloat("sfxVolume", -80);
		}
	}
    public void PlayMusic(string MusicName, bool loop)
    {
        backgroundMusicSource.Stop();
        if (lookupMusic.TryGetValue(MusicName, out var music))
        {
            
            backgroundMusicSource.loop = loop;
            backgroundMusicSource.clip = music;
            backgroundMusicSource.Play();
        }
        else
        {
            Debug.Log("sound not found " + MusicName);
        }
    }
    public void StopMusic()
    {
        backgroundMusicSource.Stop();
    }
    public void PlaySFX(string SfxName, float volume = 1)
    {
        if (lookupSFX.TryGetValue(SfxName, out var sfx))
        {
            sfxSource.PlayOneShot(sfx,volume);
        }
        else
        {
            Debug.Log("sound not found "+ SfxName);
        }

    }
}

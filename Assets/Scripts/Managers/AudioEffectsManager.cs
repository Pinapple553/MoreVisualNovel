using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static BackgroundManager;

public class AudioEffectsManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource backgroundMusicSource;
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

    void Awake()
    {
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

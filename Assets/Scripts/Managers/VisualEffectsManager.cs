using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class VisualEffectsManager : MonoBehaviour
{
    [SerializeField] private RectTransform screenRoot;
    private Vector2 originalPos;
    private Coroutine shakeRoutine;


    [SerializeField] 
    private VideoPlayer videoPlayer;
    [SerializeField] 
    private RawImage videoImage;
    [SerializeField] 
    private RenderTexture renderTexture;


    [System.Serializable]
    public class Cutscene
    {
        public string cutsceneName;
        public VideoClip videoClip;
    }
    public List<Cutscene> cutscenes;
    private Dictionary<string, VideoClip> lookup; //lookup dictionary for quick access to expressions
    void Awake()
    {
        lookup = new Dictionary<string, VideoClip>();
        foreach (var obj in cutscenes)
        {
            lookup[obj.cutsceneName] = obj.videoClip;
        }
    }
    public void ShakeUI(float duration = 1, float intensity = 15f, bool horizontal = true, bool vertical = true)
    {
        float time = 0.3f * duration;
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(time, intensity, horizontal, vertical));
    }

    private IEnumerator ShakeRoutine( float duration, float intensity, bool horizontal, bool vertical)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 offset = Vector2.zero;

            if (horizontal)
                offset.x = Random.Range(-1f, 1f) * intensity;

            if (vertical)
                offset.y = Random.Range(-1f, 1f) * intensity;

            screenRoot.anchoredPosition = originalPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        screenRoot.anchoredPosition = new Vector3(0,0,0);
        shakeRoutine = null;
    }

    public void PlayCutscene(string cutsceneName)
    {
        if (!lookup.TryGetValue(cutsceneName, out var clip))
        {
            Debug.LogWarning("Cutscene not found: " + cutsceneName);
            return;
        }

        videoImage.gameObject.SetActive(true);
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

}


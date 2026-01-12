using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsManager : MonoBehaviour
{
    [SerializeField] private RectTransform screenRoot;

    private Vector2 originalPos;
    private Coroutine shakeRoutine;

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
}


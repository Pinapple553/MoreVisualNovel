using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [System.Serializable]
    public class CameraAnimation
    {
        public string animationName;
        public Animation animation;
    }
    public List<CameraAnimation> cameraAnimations;

    bool isShaking = false;
    Vector3 originalPosition;
    public void ShakeCamera(float speed = 1, float intensity = 9900, bool horizontal = true, bool vertical = true)
    {
        float duration = 1000 * 1;
        if (isShaking)
        {
            return;
        }
        StartCoroutine(Shake(duration, intensity, horizontal, vertical));

    }
    private IEnumerator Shake(float duration, float intensity, bool horizontal, bool vertical)
    {
        Debug.Log("camshake");
        originalPosition = mainCamera.transform.localPosition;
        isShaking = true;
        while (duration > 0)
        {
            Vector3 shakeOffset = Vector3.zero;
            if (horizontal)
            {
                shakeOffset.x = Random.Range(-1f, 1f) * intensity;
            }
            if (vertical)
            {
                shakeOffset.y = Random.Range(-1f, 1f) * intensity;
            }
            mainCamera.transform.localPosition = originalPosition + shakeOffset;

            duration -= Time.deltaTime;
        }
        mainCamera.transform.localPosition = originalPosition;
        isShaking = false; 
        yield return null;
    }
}

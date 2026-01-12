using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterVisuals : MonoBehaviour
{
    [System.Serializable]
    public class CharacterExpression //class to hold expression data (can add as many as needed per character)
    {
        public string expressionName; //name of the expression
        public Sprite sprite; //sprite file for the expression
    }

    public string characterName; //name of the character
    public Image characterImageUI; //UI image component to display the character

    public List<CharacterExpression> expressions; //list of all the characters expressions

    private Dictionary<string, Sprite> lookup; //lookup dictionary for quick access to expressions

    void Awake()
    {
        lookup = new Dictionary<string, Sprite>();
        foreach (var exp in expressions) //link expression names to character sprites
        {
            lookup[exp.expressionName] = exp.sprite;
        }
    }

    public void SetExpression(string expressionId) //set character sprite by expression name
    {
        if (lookup.TryGetValue(expressionId, out var sprite))
        {
            characterImageUI.sprite = sprite;
        }
        else
        {
            Debug.Log("Missing expression "+expressionId+" for "+characterName);
        }
    }
    public void Hide()
    {
        SetExpression("neutral");
        characterImageUI.enabled = false;
    }
    public void Show(string expressionId = null)
    {
        if (!string.IsNullOrEmpty(expressionId))
            SetExpression(expressionId);

        characterImageUI.enabled = true;
    }
    public void SetPosition(float position)
    {
        RectTransform rect = characterImageUI.rectTransform;

        rect.anchorMin = new Vector2(position, rect.anchorMin.y);
        rect.anchorMax = new Vector2(position, rect.anchorMax.y);

        rect.anchoredPosition = new Vector2(0f, rect.anchoredPosition.y);
    }
    public void MoveToPosition(float position, float duration = 0.4f)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(position, duration));
    }

    private IEnumerator MoveRoutine(float target, float duration)
    {
        RectTransform rect = characterImageUI.rectTransform;

        float start = rect.anchorMin.x;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / duration);

            float x = Mathf.Lerp(start, target, t);

            rect.anchorMin = new Vector2(x, rect.anchorMin.y);
            rect.anchorMax = new Vector2(x, rect.anchorMax.y);

            yield return null;
        }

        rect.anchorMin = new Vector2(target, rect.anchorMin.y);
        rect.anchorMax = new Vector2(target, rect.anchorMax.y);
    }

    public void FlipCharacter(string direction)
    {
        RectTransform rect = characterImageUI.rectTransform;
        if (direction == "right")
        {
            rect.transform.eulerAngles = new Vector3(rect.transform.eulerAngles.x, 180, rect.transform.eulerAngles.z);
        }
        else
        {
            rect.transform.eulerAngles = new Vector3(rect.transform.eulerAngles.x, 0 , rect.transform.eulerAngles.z);
        }
        
    }

}
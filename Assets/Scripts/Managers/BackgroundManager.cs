using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundImage 
    {
        public string backgroundName; 
        public Sprite sprite; 
    }
    public List<BackgroundImage> backgrounds; 

    [SerializeField]
    private Image backgroundImageUI;
    private Dictionary<string, Sprite> lookup; //lookup dictionary for quick access to expressions

    void Awake()
    {
        lookup = new Dictionary<string, Sprite>();
        foreach (var exp in backgrounds) 
        {
            lookup[exp.backgroundName] = exp.sprite;
        }
    }

    public void ChangeBackground(string backgroundId) 
    {
        if (lookup.TryGetValue(backgroundId, out var sprite))
        {
            backgroundImageUI.sprite = sprite;
        }
        else
        {
            Debug.Log("background not found "+backgroundId);
        }
    }
}


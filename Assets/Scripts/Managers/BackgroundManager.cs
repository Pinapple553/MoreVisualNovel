using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public string CurrentBackgroundID { get; private set; } //for saving /loading purposes, stores the current background ID

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
        CurrentBackgroundID = backgroundId;
        GameManager.Instance.currentBackgroundID = backgroundId;

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


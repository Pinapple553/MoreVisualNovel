using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterVisuals : MonoBehaviour
{
    [System.Serializable]
    public class CharacterExpression //class to hold expression data (can add as many as needed per character)
    {
        public string expressionId; //name of the expression
        public Sprite sprite; //sprite file for the expression
    }

    public string characterId; //name of the character
    public Image image; //UI image component to display the character

    public List<CharacterExpression> expressions; //list of all the characters expressions

    private Dictionary<string, Sprite> lookup; //lookup dictionary for quick access to expressions

    void Awake()
    {
        lookup = new Dictionary<string, Sprite>();
        foreach (var exp in expressions) //link expression names to character sprites
        {
            lookup[exp.expressionId] = exp.sprite;
        }
    }

    public void SetExpression(string expressionId) //set character sprite by expression name
    {
        if (lookup.TryGetValue(expressionId, out var sprite))
        {
            image.sprite = sprite;
        }
        else
        {
            Debug.Log("Missing expression "+expressionId+" for "+characterId);
        }
    }
}
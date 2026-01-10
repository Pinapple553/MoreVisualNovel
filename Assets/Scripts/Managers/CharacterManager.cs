using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public List<CharacterVisuals> characters;

    private Dictionary<string, CharacterVisuals> lookup; //lookup dictionary for quick access to expressions
    void Awake()
    {
        lookup = new Dictionary<string, CharacterVisuals>();
        foreach (var character in characters) //link character names to character images
        {
            lookup[character.characterId] = character;
        }
    }

    public void SetExpression(string characterId, string expressionId) //set character sprite by expression name
    {
        if (lookup.TryGetValue(characterId, out var character))
        {
            character.SetExpression(expressionId);
        }
        else
        {
            Debug.LogWarning("Unknown character "+characterId);
        }
    }
    public bool CharExists(string characterId) 
    {
        if (lookup.TryGetValue(characterId, out var character))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}

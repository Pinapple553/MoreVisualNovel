using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<CharacterVisuals> characters;
    private Dictionary<string, CharacterVisuals> lookup;

    void Awake()
    {
        lookup = new Dictionary<string, CharacterVisuals>();
        foreach (var character in characters)
            lookup[character.characterName] = character;
    }

    private bool TryGetCharacter(string name, out CharacterVisuals character)
    {
        if (lookup.TryGetValue(name, out character))
            return true;

        Debug.LogWarning("Unknown character " + name);
        return false;
    }
    public bool CharExists(string name)
    {
        if (lookup.TryGetValue(name, out var character))
            return true;
        else
            return false;
    }

    public void Show(string characterName, string expression = "neutral")
    {
        if (TryGetCharacter(characterName, out var character))
            character.Show(expression);
    }

    public void Hide(string characterName)
    {
        if (TryGetCharacter(characterName, out var character))
            character.Hide();
    }

    public void HideAll()
    {
        foreach (var character in characters)
            character.Hide();
    }

    public void SetExpression(string characterName, string expression)
    {
        if (TryGetCharacter(characterName, out var character))
            character.SetExpression(expression);
    }

    public void SetCharacterPosition(string characterName, float position)
    {
        if (TryGetCharacter(characterName, out var character))
            character.SetPosition(position);
    }

    public void MoveCharacterPosition(string characterName, float position)
    {
        if (TryGetCharacter(characterName, out var character))
            character.MoveToPosition(position);
    }

}

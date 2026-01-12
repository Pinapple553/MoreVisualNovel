using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CharacterVisuals;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class StoryManager : MonoBehaviour
{
    [Header("Story")]
    [SerializeField]
    private TextAsset inkJSONAsset;
    public Story story;

    [Header("Scene Objects")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private CharacterManager characterManager;
    [SerializeField]
    private BackgroundManager backgroundManager;
    [SerializeField]
    private AudioEffectsManager audioEffectsManager;
    [SerializeField]
    private VisualEffectsManager visualEffectsManager;


    [Header("UI Prefabs")]
    [SerializeField]
    private TextMeshProUGUI textPrefab;
    [SerializeField]
    private TextMeshProUGUI speakerPrefab;
    [SerializeField]
    private Button buttonPrefab;
    [SerializeField]
    private Image textBoxPrefab;
    //[SerializeField] private Image backgroudImagePrefab;

    [Header("Story Settings")]
    [SerializeField]
    public float textSpeed = 0.05f;

    //dialogue handling
    private GameObject choicesContainer; // Container for choice buttons
    private Image textBoxInstance;
    private Image backgroudImageInstance;
    private TextMeshProUGUI currentDialogue;
    private TextMeshProUGUI currentSpeakerText;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool choicesCreated = false;

    private InputSystem controls; //Input system

    private void OnEnable()
    {
        controls.UI.Enable();
    }
    private void OnDisable()
    {
        controls.UI.Disable();
    }
    void Awake()
    {
        controls = new InputSystem();
        controls.UI.Advance.performed += ctx =>
        {
            GameObject clicked = EventSystem.current.currentSelectedGameObject;
            if (!(clicked != null && clicked.GetComponent<Button>() != null))   // ignores clicks on buttons so story dosn't advance twice when making a choice
            {
                AdvanceStory();
            }
        };

        CreateBackgroundImage();
        CreateTextBox();
        CreateChoiceContainer();
        //OrderCanvasItems();

        StartStory();
    }

    // Creates a new Story object with the compiled story which we can then play!
    void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        AdvanceStory();
    }
    void AdvanceStory()
    {
        if (isTyping)// If text is still typing, finish instantly
        {
            StopCoroutine(typingCoroutine);
            ShowDialogue(story.currentText);
            isTyping = false;
            return;
        }
        if (story.canContinue) //shows next line of dialogue if possible
        {
            //visualEffectsManager.ShakeCamera();
            RemoveChoices(); // Remove any existing choices
            string text = story.Continue().Trim();
            TypeDialogue(text);
            HandleTags();
            return;
        }
        if (story.currentChoices.Count > 0)// If choices exist, show them
        {
            ShowChoices();
            return;
        }
    }
    void ShowDialogue(string text)
    {
        if (currentDialogue == null)
        {
            currentDialogue = Instantiate(textPrefab, textBoxInstance.transform);
        }
        if (text.Contains(":"))
        {
            String[] textParts = text.Split(new char[] { ':' }, 2);
            ChangeSpeaker(textParts[0]);
            currentDialogue.text = textParts[1];
        }
        else
        {
            ChangeSpeaker("");
            currentDialogue.text = text;
        }
    }
    void TypeDialogue(string text)
    {
        if (currentDialogue == null)
        {
            currentDialogue = Instantiate(textPrefab, textBoxInstance.transform);
        }
        if (text.Contains(":"))
        {
            String[] textParts = text.Split(new char[] { ':' }, 2);
            ChangeSpeaker(textParts[0]);
            typingCoroutine = StartCoroutine(TypeText(currentDialogue, textParts[1]));
        }
        else
        {
            ChangeSpeaker("");
            typingCoroutine = StartCoroutine(TypeText(currentDialogue, text));
        }
    }
    IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText)
    {
        isTyping = true;
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }
    void ShowChoices()
    {
        if (choicesCreated) return; // Prevents creating choices multiple times
        foreach (Choice choice in story.currentChoices)
        {
            Button button = Instantiate(buttonPrefab, choicesContainer.transform, false);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.text.Trim();

            // Gets the text from the button prefab
            TextMeshProUGUI choiceText = button.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = choice.text.Trim();

            Choice localChoice = choice;
            button.onClick.AddListener(() =>
            {
                story.ChooseChoiceIndex(localChoice.index);
                AdvanceStory();
            });
            choicesCreated = true;
        }
    }
    void RemoveChoices()// Destroys the choice buttons
    {
        foreach (Transform t in choicesContainer.transform)
        {
            Destroy(t.gameObject);
        }
        choicesCreated = false;
    }
    void OrderCanvasItems()
    {
        textBoxInstance.transform.SetAsFirstSibling();
        choicesContainer.transform.SetAsFirstSibling();
        backgroudImageInstance.transform.SetAsFirstSibling();

    }
    void CreateChoiceContainer()
    {
        choicesContainer = new GameObject("ChoicesContainer");
        choicesContainer.transform.SetParent(canvas.transform, false);

        RectTransform rect = choicesContainer.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 0);
        rect.anchorMax = new Vector2(1f, 0);
        rect.pivot = new Vector2(1f, 0);
        rect.anchoredPosition = new Vector2(-80, 350);
        rect.sizeDelta = new Vector2(350, 600);

        VerticalLayoutGroup layout = choicesContainer.AddComponent<VerticalLayoutGroup>();
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;
        layout.childAlignment = TextAnchor.LowerCenter;
        layout.spacing = 20f;
    }
    void CreateTextBox()
    {
        textBoxInstance = Instantiate(textBoxPrefab, canvas.transform);
    }
    void CreateBackgroundImage()
    {
        //backgroudImageInstance = Instantiate(backgroudImagePrefab, canvas.transform);
    }
    void ChangeSpeaker(string spreakerName)
    {
        if (currentSpeakerText == null)
        {
            currentSpeakerText = Instantiate(speakerPrefab, textBoxInstance.transform);
        }
        currentSpeakerText.text = spreakerName;
    }
    void HandleTags()
    {
        foreach (string tag in story.currentTags)
        {
            string[] splitTag = tag.Split(" ");

            switch (splitTag[0].Trim())
            {
                case "char":
                    switch (splitTag[1].Trim())
                    {
                        case "show":
                            if (characterManager.CharExists(splitTag[2]))
                            {
                                if (splitTag.Length > 3)
                                {
                                    characterManager.Show(splitTag[2], splitTag[3]);
                                }
                                else
                                {
                                    characterManager.Show(splitTag[2]);
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Unknown character in tag: " + tag);
                            }
                            break;

                        case "hide":
                            if (characterManager.CharExists(splitTag[2]))
                            {
                                if (splitTag.Length > 3)
                                {
                                    characterManager.Hide(splitTag[2]);
                                }
                                else
                                {
                                    characterManager.Hide(splitTag[2]);
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Unknown character in tag: " + tag);
                            }
                            break;

                        case "move":
                            if (splitTag.Length < 4) break;
                            if (characterManager.CharExists(splitTag[2]))
                            {
                                if (float.TryParse(splitTag[3], out float movePosition))
                                {
                                    characterManager.MoveCharacterPosition(splitTag[2], movePosition);
                                }
                                else
                                {
                                    Debug.LogWarning("Unknown position in tag: " + tag);
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Unknown character in tag: " + tag);
                            }
                            break;
                        case "set":
                            if (splitTag.Length < 4) break;
                            if (characterManager.CharExists(splitTag[2]))
                            {
                                if (float.TryParse(splitTag[3], out float setPosition))
                                {
                                    characterManager.SetCharacterPosition(splitTag[2], setPosition);
                                }
                                else
                                {
                                    Debug.LogWarning("Unknown position in tag: " + tag);
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Unknown character in tag: " + tag);
                            }
                            break;
                        case "flip":
                            if (splitTag.Length < 4) break;
                            if (characterManager.CharExists(splitTag[2]))
                            {
                                characterManager.FlipCharacter(splitTag[2], splitTag[3]);
                            }
                            else
                            {
                                Debug.LogWarning("Unknown character in tag: " + tag);
                            }
                            break;
                        default:
                            if (characterManager.CharExists(splitTag[1]))
                            {
                                if (splitTag.Length > 2) //set character expression
                                {
                                    characterManager.SetExpression(splitTag[1], splitTag[2]);
                                }
                                else
                                {
                                    characterManager.SetExpression(splitTag[1], "neutral");//show neutral expression
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Unknown character in tag: " + tag);
                            }
                            break;
                    }
                    break;

                case "bg":
                    if (splitTag.Length < 2) break;
                    backgroundManager.ChangeBackground(splitTag[1]);
                    break;

                case "sfx":
                    if (splitTag.Length < 2) break;
                    if (splitTag.Length >2 && float.TryParse(splitTag[2], out float volume))
                    {
                        audioEffectsManager.PlaySFX(splitTag[1], volume);
                    }
                    else
                    {
                        audioEffectsManager.PlaySFX(splitTag[1]);
                    }

                    break;
                case "music":
                    if (splitTag.Length < 2) break;
                    if (splitTag[1] =="stop")
                    {
                        audioEffectsManager.StopMusic();
                        break;
                    }
                    bool loop = true;
                    if (splitTag.Length > 2 && splitTag[2] == "loop")
                    {
                        loop = true;
                    }
                    if (splitTag.Length > 2 && splitTag[2] == "once")
                    {
                        loop = false;
                    }
                    audioEffectsManager.PlayMusic(splitTag[1], loop);
                    break;
                case "txtspeed":
                    switch (splitTag[1])
                    {
                        case "slow":
                            textSpeed = 0.5f;
                            break;
                        case "normal":
                            textSpeed = 0.05f;
                            break;
                        case "fast":
                            textSpeed = 0.01f;
                            break;
                        default:
                            break;
                    }
                    break;
                case "vfx":
                    Debug.Log("VFX tag handling not implemented yet: " + tag);
                    break;
                default:
                    Debug.Log("Unhandled tag: " + tag);
                    break;
            }
        }
    }
}

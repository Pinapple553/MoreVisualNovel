using Ink.Runtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CharacterVisuals;

public class StoryManager : MonoBehaviour
{
    [Header("Story")]
    [SerializeField]
    private TextAsset inkJSONAsset;
    public Story story;

    [Header("Scene Objects")]
    [SerializeField]
    private CharacterManager characterManager;
    [SerializeField]
    private BackgroundManager backgroundManager;
    [SerializeField]
    private AudioEffectsManager audioEffectsManager;
    [SerializeField]
    private VisualEffectsManager visualEffectsManager;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private TextMeshProUGUI speakerText;
    [SerializeField]
    private Image textBox;
    [SerializeField]
    private GameObject choicesContainer;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject settingsScreen;

    [Header("UI Prefabs")]
    [SerializeField]
    private Button buttonPrefab;

    [Header("Story Settings")]
    public float baseTextSpeed = 0.05f;
    public float currentTextSpeed = 0.05f;
    public float autoTextSpeed = 0.05f;
    public float autoDelay = 2f;



    //dialogue handling
    private Image textBoxInstance;
    private Image backgroudImageInstance;
    private TextMeshProUGUI currentDialogue;
    private TextMeshProUGUI currentSpeakerText;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool choicesCreated = false;
    private bool fastForwarding = false;
    private bool autoPlaying = false;
    Coroutine currentAuto;

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
        if (SceneManager.GetActiveScene().name == "GameScene")
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
            controls.UI.Pause.performed += ctx =>
            {
                if (SceneManager.GetActiveScene().name == "GameScene")
                {
                    PauseGame();
                }
            }; 

            StartStory();

        }
    }


    public Story GetCurrentStory()
    {
        return story;
    }
    // Creates a new Story object with the compiled story which we can then play!
    void StartStory()
    {
        if(GameManager.Instance.currentStory != null)
        {
            story = GameManager.Instance.currentStory;
        }
        else
        {
            story = new Story(inkJSONAsset.text);
        }
        GameManager.Instance.currentStory = story;

        if (GameManager.Instance.loadFromSave)
        {
            GameData data = SaveLoadManager.Instance
                .LoadGameData(GameManager.Instance.loadPage, GameManager.Instance.loadSlot);

            if (data != null)
            {
                story.state.LoadJson(data.storyJson);
                backgroundManager.ChangeBackground(data.background_id);
            }

            GameManager.Instance.loadFromSave = false;
        }

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
            if (story.currentChoices.Count > 0)// If choices exist, show them
            {
                ShowChoices();
                return;
            }
            return;
        }
        if (story.currentChoices.Count > 0)// If choices exist, show them
        {
            ShowChoices();
            return;
        }
        GameManager.Instance.currentStory = story;
    }
    void ShowDialogue(string text)
    {
        if (currentDialogue == null)
        {
            currentDialogue = dialogueText;
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
            currentDialogue = dialogueText;
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
            for (int i = 0; i < fullText.Length; i++)
            {
            textComponent.text = fullText.Substring(0, i + 1);
            yield return new WaitForSeconds(currentTextSpeed);
        }
        isTyping = false;
    }
    void ShowChoices()
    {
        if (choicesCreated) return; // Prevents creating choices multiple times
        foreach (Choice choice in story.currentChoices)
        {
            Button button = Instantiate(buttonPrefab, choicesContainer.transform, false);
            // Gets the text from the button prefab
            TextMeshProUGUI choiceText = button.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = choice.text.Trim();

            Choice localChoice = choice;
            button.onClick.AddListener(() =>
            {
                story.ChooseChoiceIndex(localChoice.index);
                if (fastForwarding)
                {
                    FastForward();
                }
                else if (autoPlaying)
                {
                    StartCoroutine(AutoAdvance(autoDelay));
                }
                else
                {
                    AdvanceStory();
                }
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
    void ChangeSpeaker(string spreakerName)
    {
        if (currentSpeakerText == null)
        {
            currentSpeakerText = speakerText;
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
                    if (splitTag.Length > 2 && float.TryParse(splitTag[2], out float volume))
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
                    if (splitTag[1] == "stop")
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
                    if (fastForwarding)
                    {
                        break;
                    }
                    switch (splitTag[1])
                    {
                        case "slow":
                            currentTextSpeed = baseTextSpeed * 5;
                            break;
                        case "normal":
                            currentTextSpeed = baseTextSpeed;
                            break;
                        case "fast":
                            currentTextSpeed = baseTextSpeed / 5;
                            break;
                        default:
                            break;
                    }
                    break;
                case "vfx":
                    switch (splitTag[1])
                    {
                        case "screen_shake":
                            if (splitTag.Length > 2)
                            {
                                float.TryParse(splitTag[2], out float speed);
                                visualEffectsManager.ShakeUI(speed, 15, true, true);
                            }
                            else
                            {
                                visualEffectsManager.ShakeUI();
                            }

                            break;
                        case "cutscene":
                            visualEffectsManager.PlayCutscene(splitTag[2]);
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    Debug.Log("Unhandled tag: " + tag);
                    break;
            }
        }
    }
    public void FastForward()
    {
        currentTextSpeed = baseTextSpeed / 5;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            currentDialogue.text = story.currentText;
            isTyping = false;
        }
        StartAutoPlay(autoDelay);
    }
    public void ToggleFastForward()
    {
        fastForwarding = !fastForwarding;
        if (fastForwarding)
        {
            FastForward();
        }
        else
        {
            currentTextSpeed = baseTextSpeed;
            StopAutoPlay();
        }
    }
    public void Skip()
    {
        StopAutoPlay();

        while (story.canContinue)
        {
            story.Continue();
        }
        AdvanceStory();
        ShowChoices();
    }
    public void ToggleAutoPlay(float delay)
    {
        if (!autoPlaying)
        {
            StartAutoPlay(delay);
        }
        else
        {
            StopAutoPlay();
        }
    }
    public void StartAutoPlay(float delay)
    {
        if (autoPlaying) return;

        currentTextSpeed = autoTextSpeed;
        autoPlaying = true;
        currentAuto = StartCoroutine(AutoAdvance(delay));
    }
    public void StopAutoPlay()
    {
        autoPlaying = false;
        currentTextSpeed = baseTextSpeed;

        if (currentAuto != null)
            StopCoroutine(currentAuto);
    }
    private IEnumerator AutoAdvance(float delay)
    {
        while (autoPlaying)
        {
            yield return new WaitUntil(() => !isTyping);

            if (story.currentChoices.Count > 0)
            {
                autoPlaying = false;
                yield break;
            }

            if (story.canContinue)
            {
                yield return new WaitForSeconds(delay);
                AdvanceStory();
            }
            else
            {
                autoPlaying = false;
                yield break;
            }
        }
    }
    public void SetTextSpeed(float newTextSpeed)
    {
        currentTextSpeed = newTextSpeed;
    }
    public void SetAutoDelay(float newAutoDelay)
    {
        autoDelay = newAutoDelay;
    }
    public void SetAutoTextSpeed(float newAutoTextSpeed)
    {
        autoTextSpeed = newAutoTextSpeed;
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
    }
    public void ShowSettings()
    {
        settingsScreen.SetActive(!settingsScreen.activeSelf);
    }
}


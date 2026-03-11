using System.Collections;
using TMPro;
using UnityEngine;

public class InstructionTexts : MonoBehaviour
{
    public TMP_Text instructionText;
    public CanvasGroup dialogueCanvasGroup;

    public float fadeTime = 2f;

    public string[] instructions; // list of texts
    
    public CanvasGroup controlsCanvasGroup;
    public float controlsVisibleTime = 10f; // how long controls stay
    public float controlsFadeTime = 1f;    // fade duration
    public TMP_Text continuePrompt;
    void Awake()
    {
        if (controlsCanvasGroup != null)
        {
            controlsCanvasGroup.alpha = 0;
            controlsCanvasGroup.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        // Pause the game
        Time.timeScale = 0f;

        StartCoroutine(PlayInstructions());
    }

    IEnumerator PlayInstructions()
    {
        foreach (string message in instructions)
        {
            yield return StartCoroutine(ShowInstructionRoutine(message));
        }

        // Resume the game
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Show controls AFTER instructions
        if (controlsCanvasGroup != null)
            yield return StartCoroutine(ShowControls());
    }

    IEnumerator ShowInstructionRoutine(string message)
    {
        instructionText.text = message;

        // show everything
        dialogueCanvasGroup.alpha = 1;

        // Wait for player to press E
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.unscaledDeltaTime;
            dialogueCanvasGroup.alpha = 1 - (timer / fadeTime);
            yield return null;
        }
        dialogueCanvasGroup.alpha = 0;
    }
 
    IEnumerator ShowControls()
    {
        controlsCanvasGroup.gameObject.SetActive(true);
        controlsCanvasGroup.alpha = 1;

        yield return new WaitForSecondsRealtime(controlsVisibleTime);

        float timer = 0f;

        while (timer < controlsFadeTime)
        {
            timer += Time.unscaledDeltaTime;
            controlsCanvasGroup.alpha = 1 - (timer / controlsFadeTime);
            yield return null;
        }

        controlsCanvasGroup.alpha = 0;
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class InstructionTexts : MonoBehaviour
{
    public TMP_Text instructionText;
    public CanvasGroup dialogueCanvasGroup;

    public float fadeTime = 2f;

    public string[] instructions; // list of texts

    public TMP_Text controlsText;
    public CanvasGroup controlsCanvasGroup;
    public float controlsVisibleTime = 20f; // how long controls stay
    public float controlsFadeTime = 1f;    // fade duration
    public TMP_Text continuePrompt;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        StartCoroutine(PlayInstructions());
        // Show controls
        if (controlsText != null && controlsCanvasGroup != null)
            StartCoroutine(ShowControls());
    }

    IEnumerator PlayInstructions()
    {
        foreach (string message in instructions)
        {
            yield return StartCoroutine(ShowInstructionRoutine(message));
        }
    }

    IEnumerator ShowInstructionRoutine(string message)
    {
        instructionText.text = message;

        // show everything
        dialogueCanvasGroup.alpha = 1;

        // Wait for player to press E
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        // fade out container
        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            dialogueCanvasGroup.alpha = 1 - (timer / fadeTime);
            yield return null;
        }
        dialogueCanvasGroup.alpha = 0;
    }
    IEnumerator ShowControls()
    {
        // make sure it's visible first
        controlsCanvasGroup.alpha = 1;

        yield return new WaitForSeconds(controlsVisibleTime);

        float timer = 0f;
        while (timer < controlsFadeTime)
        {
            timer += Time.deltaTime;
            controlsCanvasGroup.alpha = 1 - (timer / controlsFadeTime);
            yield return null;
        }

        controlsCanvasGroup.alpha = 0;
    }
}

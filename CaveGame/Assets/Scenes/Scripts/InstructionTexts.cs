using System.Collections;
using TMPro;
using UnityEngine;

public class InstructionTexts : MonoBehaviour
{
    public TMP_Text instructionText;
    public CanvasGroup canvasGroup;

    public float visibleTime = 5f;
    public float fadeTime = 2f;

    public string[] instructions; // list of texts

    public TMP_Text controlsText;
    public CanvasGroup controlsCanvasGroup;
    public float controlsVisibleTime = 20f; // how long controls stay
    public float controlsFadeTime = 1f;    // fade duration

    void Start()
    {
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

        // show text
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(visibleTime);

        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1 - (timer / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 0;

        yield return new WaitForSeconds(1f);
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

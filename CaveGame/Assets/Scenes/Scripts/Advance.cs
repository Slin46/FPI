using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Advance : MonoBehaviour
{
    public CanvasGroup fadeScreen;
    public float delayBeforeFade = 3f;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeScreen.alpha = timer / fadeDuration;
            yield return null;
        }

        fadeScreen.alpha = 1;

        SceneManager.LoadScene("GameScene");
    }
}

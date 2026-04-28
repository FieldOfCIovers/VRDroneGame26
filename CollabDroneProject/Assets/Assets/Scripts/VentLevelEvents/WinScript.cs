using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{

    public bool finishedCourse = false;
    //Fade To Black Variables 
    public CanvasGroup fadePanel;
    public float fadeDuration;
    private bool isFading = false;

    private void Start()
    {
        StartCoroutine(FadeFromBlack());
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit");

        GameObject triggerObject = gameObject;

        if (triggerObject.tag == "Finish" && finishedCourse == false)
        {
            //Temporary Code for Tutorial Debug
            finishedCourse = true;
            StartCoroutine(FadeToBlack());
            SceneManager.LoadScene("TitleScreen");

        }
    }

    private IEnumerator FadeToBlack()
    {

        fadePanel.alpha = 0.0f;
        float elapsed = 0.0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0.0f, 1.0f, elapsed / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 1.0f;
    }

    private IEnumerator FadeFromBlack()
    {

        fadePanel.alpha = 1.0f;
        float elapsed = 0.0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1.0f, 0.0f, elapsed / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 0.0f;
    }
}

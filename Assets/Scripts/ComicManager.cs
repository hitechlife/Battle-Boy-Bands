using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicManager : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject title;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunComic());
    }

    IEnumerator RunComic() {
        for (int i = 0; i < panels.Length; i++) {
            panels[i].SetActive(true);
            Image img = panels[i].GetComponent<Image>();
            yield return DoFadeIn(img, 0.8f);

            // Skip panel if this is the final comic
            if (i == panels.Length - 1) continue;

            yield return new WaitForSeconds(3f);
            yield return DoFadeOut(img, 0.8f);
            panels[i].SetActive(false);
        }

        continueButton.SetActive(true);
        quitButton.SetActive(true);
        title.SetActive(true);
    }


    // Taken from https://answers.unity.com/questions/1379010/fade-gui-from-coroutine.html
    private IEnumerator DoFadeOut(Image img, float fadeTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - Mathf.Clamp01(elapsedTime / fadeTime));
        }
    }

    private IEnumerator DoFadeIn(Image img, float fadeTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            img.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp01(elapsedTime / fadeTime));
        }
    }
}

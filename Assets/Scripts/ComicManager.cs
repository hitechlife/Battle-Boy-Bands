using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicManager : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject creditsButton;
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
                        yield return new WaitForSeconds(4.05f);

            // Skip panel if this is the final comic
            if (i == panels.Length - 1) continue;

            // yield return DoFadeOut(img, 0.8f);
            // panels[i].SetActive(false);
        }

        continueButton.SetActive(true);
        quitButton.SetActive(true);
        creditsButton.SetActive(true);
        title.SetActive(true);

        StartCoroutine(DoFadeIn(continueButton.GetComponent<Image>(), 0.8f));
        StartCoroutine(DoFadeIn(creditsButton.GetComponent<Image>(), 0.8f));
        StartCoroutine(DoFadeIn(quitButton.GetComponent<Image>(), 0.8f));
        StartCoroutine(DoFadeInText(title.GetComponent<Text>(), 0.8f));
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
        img.color = new Color(1.0f, 1.0f, 1.0f, 0);
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            img.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp01(elapsedTime / fadeTime));
        }
    }

    private IEnumerator DoFadeInText(Text txt, float fadeTime)
    {
        txt.color = new Color(1.0f, 1.0f, 1.0f, 0);
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            txt.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp01(elapsedTime / fadeTime));
        }
    }
}

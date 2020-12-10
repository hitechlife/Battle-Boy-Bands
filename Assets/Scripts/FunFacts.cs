using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunFacts : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
        StartCoroutine(ShowFacts());
    }

    IEnumerator ShowFacts() {
        while(true) {
            text.color = new Color(207f/255f, 63/255f, 171/255f, 0);
            text.text = BattleLineManager.S.RetrieveQuip(Random.Range(1, BattleLineManager.S.QuipLen() + 1));
            yield return DoFadeInText(text, 0.8f);
            yield return new WaitForSeconds(6.4f);
            yield return DoFadeOutText(text, 0.8f);
        }
    }

    private IEnumerator DoFadeOutText(Text txt, float fadeTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            txt.color = new Color(207f/255f, 63/255f, 171/255f, 1.0f - Mathf.Clamp01(elapsedTime / fadeTime));
        }
    }

    private IEnumerator DoFadeInText(Text txt, float fadeTime)
    {
        txt.color = new Color(207f/255f, 63/255f, 171/255f, 0);
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            txt.color = new Color(207f/255f, 63/255f, 171/255f, Mathf.Clamp01(elapsedTime / fadeTime));
        }
    }
}

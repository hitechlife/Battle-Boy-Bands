using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCrowdAnimation : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (GameManager.instance?.bossesDefeated == 3) StartCoroutine(AnimateCrowdMenu());
    }

    IEnumerator AnimateCrowdMenu() {
        // crowd.SetActive(true);
        object[] loadedSprite = Resources.LoadAll("Crowd", typeof(Sprite));
        // FMOD.Studio.PLAYBACK_STATE state = FMOD.Studio.PLAYBACK_STATE.PLAYING;
        // float timer = 0;
        float animateInterval = 0.07f;
        while (true) { // hardcoded len of crowd noise
            foreach (object obj in loadedSprite) {
                Sprite s = (Sprite)obj;
                image.sprite = s;
                // timer += animateInterval;
                yield return new WaitForSeconds(animateInterval);
            }
            // timer += Time.deltaTime;
            yield return null;
            // Sound.getPlaybackState(out state);
        }
        // crowd.SetActive(false);
    }
}

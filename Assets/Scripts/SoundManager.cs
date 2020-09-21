using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    private int bpmIncrement = 20;

    // Singleton reference
    public static SoundManager S;

    private void Awake()
    {
        S = this;
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        soundToPlay.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            BeatManager.S.ModifySoundValue(bpmIncrement);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            BeatManager.S.ModifySoundValue(-bpmIncrement);
        }
    }
}

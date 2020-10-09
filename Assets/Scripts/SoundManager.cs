using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    private int bpmIncrement = 20;
    [SerializeField]
    private List<AudioSource> tracks;

    public int trackIndex;
    private int trackStartTime;
    private int trackStopTime;

    // Singleton reference
    public static SoundManager S;

    private void Awake()
    {
        S = this;
        tracks[trackIndex].Play();
        // trackStartTime = tracks[trackIndex].GetComponent<TrackProperties>().startTime;
        // trackStopTime = tracks[trackIndex].GetComponent<TrackProperties>().stopTime;
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        soundToPlay.Play();
    }

    private void Update()
    {
        // Changes BPM
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

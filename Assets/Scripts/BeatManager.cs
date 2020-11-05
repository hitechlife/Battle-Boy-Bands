using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{

    // Serialized fields
    [SerializeField]
    private int beatsPerMinute;
    [SerializeField]
    private AudioSource beatSound;
    [SerializeField]
    private bool playSound = true;

    // Constants
    private readonly int SECONDS_CONST = 60;
    public readonly int SUBDIVISION_CONST = 4;
    public readonly int NUM_BREAK_BARS = 2;

    private float counter = 0;
    public bool isFirstLoop = true;
    public bool isEnemyLoop = false;
    public bool isPlayerLoop = false;
    public bool isPlayerResponseLoop = false;

    // Timing vars
    public float gain = 0.5F;
    double nextTick = 0.0F;
    private float amp = 0.0F;
    private float phase = 0.0F;
    double sampleRate = 0.0F;
    int accent;
    bool running = true;
    bool okToToggle;

    // Singleton reference
    public static BeatManager S;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        accent = SUBDIVISION_CONST;
        counter = NUM_BREAK_BARS;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;

        GetComponent<AudioSource>().Play();
    }

    // This is from the Unity documentation, thanks Unity!
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        double samplesPerTick = sampleRate * 60.0F / beatsPerMinute * 4.0F / SUBDIVISION_CONST;
        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;

        int n = 0;
        while (n < dataLen)
        {
            float x = gain * amp * Mathf.Sin(phase);
            int i = 0;
            while (i < channels)
            {
                data[n * channels + i] += x;
                i++;
            }
            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                amp = 1.0F;

                if (++accent > SUBDIVISION_CONST)
                {
                    accent = 1;
                    // This broke it earlier
                    // amp *= 2.0F;

                    counter++;
                }

                if (counter > NUM_BREAK_BARS)
                {
                    if (okToToggle)
                    {
                        ToggleCounters();
                    }
                    counter = 1;
                    okToToggle = true;
                }

                // Debug.Log("Tick: " + accent + "/" + SUBDIVISION_CONST);
                // Debug.Log("Bar: " + counter + "/" + NUM_BREAK_BARS);
            }

            phase += amp * 0.3F;
            amp *= 0.993F;
            n++;
        }
    }

    void ToggleCounters()
    {
        if (isFirstLoop)
        {
            isFirstLoop = false;
            isEnemyLoop = true;
        }
        else if (isEnemyLoop)
        {
            isEnemyLoop = false;
            isPlayerLoop = true;
        }
        else if (isPlayerLoop)
        {
            isPlayerLoop = false;
            isPlayerResponseLoop = true;
        }
        else if (isPlayerResponseLoop)
        {
            isPlayerResponseLoop = false;
            isEnemyLoop = true;
        }
    }

    public void ModifySoundValue(int bpmModifier)
    {
        int newBPM = beatsPerMinute + bpmModifier;

        if (newBPM < 0)
        {
            newBPM = 0;
        }

        beatsPerMinute = newBPM;
    }
}

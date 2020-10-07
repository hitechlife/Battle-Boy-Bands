﻿using System.Collections;
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

    // Private variables - beat timer
    private float measureTimer;

    // Constants
    private float SECONDS_CONST = 60;
    private int SUBDIVISION_CONST = 4;
    private double START_TICK;
    private double SAMPLE_RATE;

    // Singleton reference
    public static BeatManager S;

    private void Awake()
    {
        START_TICK = AudioSettings.dspTime;
        SAMPLE_RATE = AudioSettings.outputSampleRate;
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        double samplesPerTick = SAMPLE_RATE * 60.0F / beatsPerMinute * 4.0F / 4;
        // print(samplesPerTick);

        if (measureTimer <= 0)
        {
            if (playSound)
            {
                SoundManager.S.PlaySound(beatSound);
            }
            measureTimer = 1.0f;
        }

        measureTimer -= (beatsPerMinute / SECONDS_CONST) * Time.deltaTime;
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

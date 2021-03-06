﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{

    // Serialized fields
    [SerializeField]
    public int beatsPerMinute;
    [SerializeField]
    private AudioSource beatSound;
    [SerializeField]
    private bool playSound = true;
    [SerializeField]
    public static bool doubleTime = true;

    [Header("Player and Enemy Squishing")]
    [SerializeField]
    private Animator playerParentAnimator;
    [SerializeField]
    private Animator enemyParentAnimator;
    public bool squishPlayer;
    public bool squishOpponent;
    // [SerializeField]
    // private float squishAmount = 20.0f;

    [Header("Constants and Misc.")]
    // Constants
    private readonly int SECONDS_CONST = 60;
    public readonly int SUBDIVISION_CONST = 4;
    public readonly int NUM_BREAK_BARS = doubleTime ? 4 : 2;

    public int counter = 0;
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
    public int accent;
    bool running = true;
    bool okToToggle;

    // Singleton reference
    public static BeatManager S;
    private int playerLoops = 2;

    public int playerLoopInt = 0;

    public bool allowSquishPlayer = true;
    public bool allowSquishOpponent = true;

    private bool isSecondBoss;

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

        isSecondBoss = GameManager.instance.bossesDefeated % 2 == 1;

        StartCoroutine(RunAnimations());
    }

    private void Update()
    {
        
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

                    if (isPlayerLoop)
                    {
                        playerLoopInt++;
                    }

                    if (allowSquishPlayer)
                    {
                        squishPlayer = true;
                    }

                    if (allowSquishOpponent)
                    {
                        squishOpponent = true;
                    }
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
                else if (counter == NUM_BREAK_BARS)
                {
                    ToggleSquishStates();
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
            playerLoops--;
            if (playerLoops > 0) return;
            isPlayerLoop = false;
            isPlayerResponseLoop = true;
            playerLoops = 2;
            playerLoopInt = 0;
        }
        else if (isPlayerResponseLoop)
        {
            isPlayerResponseLoop = false;
            isEnemyLoop = true;
        }
    }

    void ToggleSquishStates()
    {
        if (isFirstLoop)
        {
            allowSquishPlayer = false;
            allowSquishOpponent = true;
        }
        else if (isEnemyLoop)
        {
            allowSquishPlayer = true;
            allowSquishOpponent = false;
        }
        else if (isPlayerLoop)
        {

        }
        else if (isPlayerResponseLoop)
        {
            allowSquishPlayer = false;
            allowSquishOpponent = true;
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

    IEnumerator RunAnimations()
    {
        while (true)
        {
            if (squishPlayer)
            {
                playerParentAnimator.SetTrigger("Squish");
                squishPlayer = false;
            }

            if (squishOpponent)
            {
                enemyParentAnimator.SetTrigger("Squish");
                squishOpponent = false;
            }
            yield return null;
        }
    }
}

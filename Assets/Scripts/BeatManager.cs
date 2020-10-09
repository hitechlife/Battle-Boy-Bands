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

    // Private variables - beat timer
    private float measureTimer;

    // Constants
    private readonly int SECONDS_CONST = 60;
    private readonly int SUBDIVISION_CONST = 4;
    public readonly int NUM_BREAK_BARS = 8;

    public float counter = 0;
    public bool isFirstLoop = true;
    public bool isEnemyLoop = false;
    public bool isPlayerLoop = false;

    // Singleton reference
    public static BeatManager S;

    private void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (measureTimer <= 0)
        {
            if (playSound)
            {
                SoundManager.S.PlaySound(beatSound);
            }

            measureTimer = 1.0f;
            counter++;
            counter %= NUM_BREAK_BARS;
            // print(counter);

            if (counter == 0)
            {
                ToggleCounters();
            }
        }

        measureTimer -= (beatsPerMinute / SECONDS_CONST) * Time.deltaTime;
        // print(measureTimer);
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

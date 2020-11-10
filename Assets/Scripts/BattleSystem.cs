﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, PLAYERCHOICE, OPPONENTTURN, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    private static FMOD.Studio.EventInstance Sound;
    private static FMOD.Studio.EventInstance Music;

    [FMODUnity.EventRef]
    public string fmodEvent = "event:/Music/Track 3";

    [SerializeField] [Range(-12f, 12f)]
    private float Points;

    //TODO: probably make stuff serialized and not public
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    public Text opponentInfoText;
    public Image opponentInfoIcon;
    public int maxTurns = 3;

    Player player;
    Opponent opponent;

    public Text enemyText;
    public Text playerText;
    public Text battleSpeaker;
    public Text announcerText;

    // Since we only have 3 choices, easier to access each separately
    // Put into array??
    public GameObject choice0;
    public GameObject choice1;
    public GameObject choice2;
    public GameObject coolTimer;

    private bool gameOver;
    private bool playerAnswered;
    // private string enemyText = "sad rap line";
    // private string playerText = "...";

    // X system
    private int numOfX = 0;
    public SpriteRenderer[] xs;
    public Sprite disabledX;
    public Sprite enabledX;

    [Header("Line IDs - make sure to only initialize the Enemy ID")]
    private int enemyID;
    [SerializeField]
    private int currentEnemyLineID;
    [SerializeField]
    private int currentPlayerLineID;

    private int selectionNum;

    // Start is called before the first frame update
    void Start()
    {
        // Music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        // Music.start();

        state = BattleState.START;
        StopAllCoroutines();
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        Music.setParameterByName("Points", Points);
    }

    // Sets up the initial battle state
    IEnumerator SetupBattle()
    {
        //need to instantiate at specific coords?
        player = playerPrefab.GetComponent<Player>();
        opponent = GameManager.opponents[GameManager.currBoss];

        // Render opponent info and set the track

        // Opponent sprite
        opponentPrefab.GetComponent<Image>().sprite = GameManager.sprites[GameManager.currBoss][0];

        // Opponent name and icon
        opponentInfoText.text = GameManager.currBoss == 1 ? opponent.GetName().Split(' ')[0] : opponent.GetName();;
        opponentInfoIcon.sprite = opponent.GetSprite();

        // Max turns and enemy ID
        maxTurns = opponent.GetTurns();
        enemyID = opponent.GetID();
        ClearText();

        // Track and BPM
        fmodEvent = "event:/Music/Track " + (GameManager.currBoss + 1);
        BeatManager.S.beatsPerMinute = GameManager.musicTracks[GameManager.currBoss].BPM;
        // BeatManager.S.clip?? = GameManager.musicTracks[GameManager.currBoss].clip;

        SetChoices(false);
        for (int i = 0; i < xs.Length; i++)
        {
            xs[i].sprite = disabledX;
        }

        // Start music
        Music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        Music.start();

        battleSpeaker.text = "Announcer";
        announcerText.text = "You vs. " + opponent.GetName();
        //TODO: play opponent.GetIntro() using this.GetComponent<AudioSource>;

        // Initial loop
        while (BeatManager.S.isFirstLoop)
        {
            yield return null;
        }
        print("DONE");

        StartCoroutine(BattleRoutine());
    }

    IEnumerator BattleRoutine() {
        Debug.Log("starting battle routine");
        // Game always ends on your turn
        for (int i = 0; i < maxTurns; i++) {
            Debug.Log("On turn " + (i+1));
            // Starts with enemy insult

            yield return StartCoroutine(OpponentTurn());
            // yield return new WaitForSeconds(2f);
            while (BeatManager.S.isEnemyLoop)
            {
                // First half
                if (BeatManager.S.counter <= BeatManager.S.NUM_BREAK_BARS/2) {
                    enemyText.text = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID).Split('/')[0];
                } else {
                    enemyText.text = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID).Split('/')[1];
                }
                yield return null;
            }

            // Player should be choosing answer during this time....
            // Note that cooldown timer coroutine is also started at this point
            yield return StartCoroutine(StartChoiceSelection());
            // yield return new WaitForSeconds(2f);
            while (BeatManager.S.isPlayerLoop)
            {
                yield return null;
            }

            // Display these after timer complete based on choice
            switch (selectionNum) {
                case 0:
                    if (numOfX > 0) {
                        numOfX--;
                    }
                    xs[numOfX].sprite = disabledX;
                    PlayCheers();
                    break;
                case 1:
                    PlayNeutral();
                    break;
                case 2:
                    xs[Mathf.Min(numOfX,xs.Length-1)].sprite = enabledX;
                    PlayBoos();
                    if (numOfX < xs.Length) {
                        numOfX++;
                    }
                    break;
                default:
                    break;
            }

            // defaults to the wrong answer if nothing chosen yet
            // does this in Cooldown routine
            // yield return StartCoroutine(ChoicesTimer(2f /*BeatManager.S.SUBDIVISION_CONST*/));

            // Display chosen insult
            yield return StartCoroutine(PlayerTurn());

            // yield return new WaitForSeconds(4f);
            while (BeatManager.S.isPlayerResponseLoop)
            {
                // First half
                if (BeatManager.S.counter <= BeatManager.S.NUM_BREAK_BARS/2) {
                    // playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selectionNum]).Split('/')[0];
                } else { //Second half
                    playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selectionNum]).Split('/')[1];                }
                yield return null;
            }
            currentEnemyLineID = BattleLineManager.S.RetrieveEnemyLineID(currentPlayerLineID);
            // enemy text should be set by TryInsult at this point

            // Break out of loop if 3 x's
            if (numOfX >= xs.Length) {
                break;
            }
        }

        EndBattle();
    }

    void EndBattle() {
        // Stop music?
        Debug.Log("Ending game");
        gameOver = true;
        battleSpeaker.text = "Announcer";
        string winner = numOfX >= xs.Length ? opponent.GetName() : player.name;
        ClearText();
        announcerText.text = "And the winner is... " + winner + "!!";
        if (numOfX < xs.Length) {
            PlayCheers();
            GameManager.instance.DefeatedBoss(enemyID);
        } else {
            PlayBoos();
        }
        SetChoices(false);
        StopAllCoroutines();
    }

    IEnumerator TryInsult(int selection)
    {
        //TODO: stop using Getcomponent so much
        //TODO: don't hardcode good/ok/bad per choice or enemy text
        currentPlayerLineID = BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selection];
        selectionNum = selection;

        switch (selection) {
            case 0: //good
                // playerText = choice0.GetComponentInChildren<Text>().text;
                // enemyText = "nice!!!";
                // if (numOfX > 0) {
                //     numOfX--;
                //     // xs[numOfX].sprite = disabledX;
                //     // Points--;
                //     //xs[numOfX].enabled = false;
                // }
                choice1.GetComponentInChildren<Button>().interactable = false;
                choice2.GetComponentInChildren<Button>().interactable = false;
                break;
            case 1: //ok
                // playerText = choice1.GetComponentInChildren<Text>().text;
                // enemyText = "meh...";
                choice0.GetComponentInChildren<Button>().interactable = false;
                choice2.GetComponentInChildren<Button>().interactable = false;
                break;
            case 2: //bad
                    // playerText = choice2.GetComponentInChildren<Text>().text;
                    //TODO: add "bad" sound for bad choice
                    // enemyText = "boooooo";
                // if (numOfX < xs.Length) {
                //     // xs[numOfX].sprite = enabledX;
                //     numOfX++;
                //     // Points++;
                // }
                choice1.GetComponentInChildren<Button>().interactable = false;
                choice0.GetComponentInChildren<Button>().interactable = false;
                break;
            default: //invalid choice
                //TODO: default to "bad" choice
                // enemyText = "boooooo";
                break;
        }

        // yield return new WaitForSeconds(2f);

        // state = BattleState.OPPONENTTURN;
        // currSpeaker.text = opponent.name;

        // yield return new WaitForSeconds(2f);

        // StartCoroutine(OpponentTurn(enemyText));

        yield return null;
    }

    IEnumerator StartChoiceSelection()
    {
        ClearText();
        selectionNum = -1;
        // Let enemy text display while we pick a choice
        state = BattleState.PLAYERCHOICE;
        battleSpeaker.text = "YOU CHOOSE";
        playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[0]).Split('/')[0];
        SetChoices(true);

        int i = 0;
        foreach (GameObject choice in new GameObject[] { choice0, choice1, choice2 })
        {
            choice.GetComponentInChildren<Text>().text = BattleLineManager.S.RetrieveChoiceLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[i]);
            i += 1;
        }

        // int doubleTime = BeatManager.doubleTime ? 2 : 1;

        yield return StartCoroutine(ChoicesTimer(60f * BeatManager.S.SUBDIVISION_CONST * BeatManager.S.NUM_BREAK_BARS / BeatManager.S.beatsPerMinute));

        // yield return new WaitForSeconds(2f);
        // yield return null;
    }

    IEnumerator PlayerTurn() {
        state = BattleState.PLAYERTURN;
        battleSpeaker.text = player.name;

        // ClearText();
        // playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selectionNum]);
        yield return null;
    }

    // OnClicked functions
    public void ChooseInsult(int selection)
    {
        if (state != BattleState.PLAYERCHOICE)
            return;

        // Don't let player pick more than once
        if (playerAnswered)
            return;

        // SetChoices(false);
        playerAnswered = true;

        StartCoroutine(TryInsult(selection));
    }

    IEnumerator OpponentTurn()
    {
        state = BattleState.OPPONENTTURN;
        battleSpeaker.text = opponent.GetName();
        // battleText.text = enemyText;
        ClearText();
        // enemyText.text = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID);
        playerAnswered = false;
        //TODO: display ALL enemy lines except the last one before letting player pick a choice
        yield return null;

        // StartCoroutine(PlayerTurn());
    }

    void SetChoices(bool active) {
        choice0.SetActive(active);
        choice1.SetActive(active);
        choice2.SetActive(active);
        coolTimer.SetActive(active);
        choice0.GetComponentInChildren<Button>().interactable = true;
        choice1.GetComponentInChildren<Button>().interactable = true;
        choice2.GetComponentInChildren<Button>().interactable = true;

        // if (active) StartCoroutine(ChoicesTimer(2f /*BeatManager.S.SUBDIVISION_CONST*/));
    }

    IEnumerator ChoicesTimer(float timeToWait) {
        Slider slider = coolTimer.GetComponentInChildren<Slider>();
        if (!slider) Debug.LogError("Slider not valid!");
        slider.value = 1;

        bool flippedOn = false;
        timeToWait--;

        // Decrease slider value over timeToWait seconds
        while (BeatManager.S.isPlayerLoop) {
            if(coolTimer.activeSelf == false) break;
            slider.value -= Time.deltaTime/timeToWait;

            //TODO: skipping beat bug fix temp

            // If we are *right before* the end of the timer
            if (BeatManager.S.accent == BeatManager.S.SUBDIVISION_CONST && BeatManager.S.counter == BeatManager.S.NUM_BREAK_BARS && !flippedOn) {
                flippedOn = true;
                print ("in switch statement");
                if (!playerAnswered) ChooseInsult(2);
                // Display these after timer complete based on choice
                switch (selectionNum) {
                    case 0:
                        if (Points > 0)
                        {
                            Points--;
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        if (Points < xs.Length) {
                            Points++;
                        }
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
        print("Choices timer completed");
        //TODO: let's not have 2 be the wrong answer everytime...
        // if (!playerAnswered) {
        //     ChooseInsult(2);
        // }

        SetChoices(false);
    }

    void ClearText() {
        playerText.text = "";
        enemyText.text = "";
        announcerText.text = "";
    }
    public void PlayBoos()
    {
        Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Crowd Noises/Crowd Boos");
        Sound.start();
        Sound.release();
    }

    public void PlayCheers()
    {
        Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Crowd Noises/Crowd Cheers");
        Sound.start();
        Sound.release();
    }

    public void PlayNeutral()
    {
        Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Crowd Noises/Crowd Neutral");
        Sound.start();
        Sound.release();
    }

}

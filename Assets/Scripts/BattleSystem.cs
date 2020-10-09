﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, OPPONENTTURN, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    //TODO: probably make stuff serialized and not public
    public BattleState state;

    //TODO: replace with actual prefabs if needed?
    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    public int maxTurns = 3;

    Player player;
    Opponent opponent;

    public Text battleText;
    public Text battleSpeaker;

    // Since we only have 3 choices, easier to access each separately
    // Put into array??
    public GameObject choice0;
    public GameObject choice1;
    public GameObject choice2;
    public GameObject coolTimer;

    private bool gameOver;
    private bool playerAnswered;
    private string enemyText = "sad rap line";

    // X system
    public int numOfX;
    public Image[] xs;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // Sets up the initial battle state
    IEnumerator SetupBattle()
    {
        //TODO: need to instantiate at specific coords
        // GameObject playerGO = Instantiate(playerPrefab);
        player = playerPrefab.GetComponent<Player>();

        // GameObject opponentGO = Instantiate(opponentPrefab);
        opponent = opponentPrefab.GetComponent<Opponent>();

        SetChoices(false);
        for (int i = 0; i < xs.Length; i++) {
            // if (i < numOfX) {
            //     xs[i].enabled = true;
            // } else {
                xs[i].enabled = false;
            // }
        }

        battleSpeaker.text = "Announcer";
        battleText.text = "You vs. " + opponent.name;

        // Initial loop
        while (BeatManager.S.isFirstLoop)
        {
            yield return null;
        }
        print("DONE");

        StartCoroutine(BattleRoutine());
    }

    IEnumerator BattleRoutine() {
        //TODO: snc with beatmanager
        // Game always ends on your turn
        for (int i = 0; i < maxTurns; i++) {
            // Starts with enemy insult
            yield return StartCoroutine(OpponentTurn(enemyText));
            // yield return new WaitForSeconds(1f);
            while (BeatManager.S.isEnemyLoop)
            {
                yield return null;
            }

            // Player should be choosing answer during this time....
            yield return StartCoroutine(PlayerTurn());
            // yield return new WaitForSeconds(2f); //TODO: not hardcoded
            while (BeatManager.S.isPlayerLoop)
            {
                yield return null;
            }

            // defaults to the wrong answer if nothing chosen yet
            //TODO: let's not have 2 be the wrong answer everytime...
            if (!playerAnswered) ChooseInsult(2);

            // Display chosen insult
            while (BeatManager.S.isPlayerResponseLoop)
            {
                yield return null;
            }
            // enemy text should be set by TryInsult at this point

            // Break out of loop if 3 x's
            if (numOfX >= 3) {
                break;
            }
        }

        EndBattle();
    }

    void EndBattle() {
        gameOver = true;
        battleSpeaker.text = "Announcer";
        string winner = numOfX >= 3 ? opponent.name : player.name;
        battleText.text = "And the winner is... " + winner + "!!";
        SetChoices(false);
    }

    IEnumerator TryInsult(int selection)
    {
        //TODO: stop using Getcomponent so much
        //TODO: don't hardcode good/ok/bad per choice or enemy text
        battleSpeaker.text = player.name;
        switch (selection) {
            case 0: //good
                battleText.text = choice0.GetComponentInChildren<Text>().text;
                enemyText = "nice!!!";
                if (numOfX > 0) {
                    numOfX--;
                    xs[numOfX].enabled = false;
                }
                break;
            case 1: //ok
                battleText.text = choice1.GetComponentInChildren<Text>().text;
                enemyText = "meh...";
                break;
            case 2: //bad
                battleText.text = choice2.GetComponentInChildren<Text>().text;
                enemyText = "boooooo";
                if (numOfX < xs.Length) {
                    xs[numOfX].enabled = true;
                    numOfX++;
                }
                break;
            default: //invalid choice
                //TODO: default to "bad" choice
                enemyText = "boooooo";
                break;
        }

        // yield return new WaitForSeconds(2f);

        // state = BattleState.OPPONENTTURN;
        // currSpeaker.text = opponent.name;

        // yield return new WaitForSeconds(2f);

        // StartCoroutine(OpponentTurn(enemyText));

        yield return null;
    }

    IEnumerator PlayerTurn()
    {
        // Let enemy text display while we pick a choice
        state = BattleState.PLAYERTURN;
        // enemyText.text = "vs. " + opponent.name + "- your turn";
        // currSpeaker.text = "You";
        SetChoices(true);

        //TODO: obviously... change... these are placeholders
        choice0.GetComponentInChildren<Text>().text = "this is correct";
        choice1.GetComponentInChildren<Text>().text = "this is ok";
        choice2.GetComponentInChildren<Text>().text = "this is incorrect";

        //TODO: fill cooldown meter

        // yield return new WaitForSeconds(2f);
        yield return null;

        // // defaults to the wrong answer
        // //TODO: let's not have 2 be the wrong answer everytime...
        // if (!playerAnswered)
        //     ChooseInsult(2);
    }

    // OnClicked functions
    public void ChooseInsult(int selection)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        SetChoices(false);
        playerAnswered = true;

        StartCoroutine(TryInsult(selection));
    }

    IEnumerator OpponentTurn(string enemyText)
    {
        state = BattleState.OPPONENTTURN;
        battleSpeaker.text = opponent.name;
        battleText.text = enemyText;
        playerAnswered = false;
        //TODO: sync with beatmanager
        //TODO: display ALL enemy lines except the last one before letting player pick a choice
        yield return null;

        // StartCoroutine(PlayerTurn());
    }

    void SetChoices(bool active) {
        choice0.SetActive(active);
        choice1.SetActive(active);
        choice2.SetActive(active);
        coolTimer.SetActive(active);

        //TODO: don't hardcode this value!!
        if (active) StartCoroutine(ChoicesTimer(BeatManager.S.SUBDIVISION_CONST));
    }

    IEnumerator ChoicesTimer(float timeToWait) {
        Slider slider = coolTimer.GetComponentInChildren<Slider>();
        if (!slider) Debug.LogError("Slider not valid!");
        slider.value = 1;
        float deltaTime = timeToWait;

        // Temporary scaling fix until we can integrate the BeatManager more
        float scalingFactor = 0.8f;

        // Decrease slider value over timeToWait seconds
        while (deltaTime > 0) {
            if(coolTimer.activeSelf == false) break;
            slider.value -= scalingFactor * Time.deltaTime/timeToWait;
            yield return null;
        }
    }

}

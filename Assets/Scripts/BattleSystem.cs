using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, PLAYERCHOICE, OPPONENTTURN, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    //TODO: probably make stuff serialized and not public
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    public GameObject opponentInfo;
    //TODO: set opponent info to opponent name, only write "kendrick" for kendrick amore
    //TODO: show lines one at a time
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
    public int numOfX;
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
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // Sets up the initial battle state
    IEnumerator SetupBattle()
    {
        //TODO: need to instantiate at specific coords
        // GameObject playerGO = Instantiate(playerPrefab);
        player = playerPrefab.GetComponent<Player>();

        //TODO: set opponentPrefab.sprite based on currBoss and gamemanager.sprites list
        // GameObject opponentGO = Instantiate(opponentPrefab);
        opponent = GameManager.opponents[GameManager.currBoss];

        enemyID = opponent.GetID();
        ClearText();

        SetChoices(false);
        for (int i = 0; i < xs.Length; i++)
        {
            xs[i].sprite = disabledX;
        }

        battleSpeaker.text = "Announcer";
        announcerText.text = "You vs. " + opponent.GetName();
        // yield return new WaitForSeconds(2f);

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
            

            // defaults to the wrong answer if nothing chosen yet
            // does this in Cooldown routine
            // yield return StartCoroutine(ChoicesTimer(2f /*BeatManager.S.SUBDIVISION_CONST*/));

            // Display chosen insult
            yield return StartCoroutine(PlayerTurn());
            // yield return new WaitForSeconds(4f);
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
        // Stop music?
        Debug.Log("Ending game");
        gameOver = true;
        battleSpeaker.text = "Announcer";
        string winner = numOfX >= 3 ? opponent.GetName() : player.name;
        ClearText();
        announcerText.text = "And the winner is... " + winner + "!!";
        if (numOfX < 3) {
            GameManager.instance.DefeatedBoss(enemyID);
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
                if (numOfX > 0) {
                    numOfX--;
                    xs[numOfX].sprite = disabledX;
                }
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
                if (numOfX < xs.Length) {
                    xs[numOfX].sprite = enabledX;
                    numOfX++;
                }
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
        // Let enemy text display while we pick a choice
        state = BattleState.PLAYERCHOICE;
        battleSpeaker.text = "YOU CHOOSE";
        battleText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[0]).Split('\n')[0];
        SetChoices(true);

        // 
        int i = 0;
        foreach (GameObject choice in new GameObject[] { choice0, choice1, choice2 })
        {
            choice.GetComponentInChildren<Text>().text = BattleLineManager.S.RetrieveChoiceLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[i]);
            i += 1;
        }

        //TODO: obviously... change... these are placeholders
        // choice0.GetComponentInChildren<Text>().text = "this is correct";
        // choice1.GetComponentInChildren<Text>().text = "this is ok";
        // choice2.GetComponentInChildren<Text>().text = "this is incorrect";

        int doubleTime = BeatManager.doubleTime ? 2 : 1;

        yield return StartCoroutine(ChoicesTimer(BeatManager.S.SUBDIVISION_CONST * doubleTime));

        // yield return new WaitForSeconds(2f);
        // yield return null;
    }

    IEnumerator PlayerTurn() {
        state = BattleState.PLAYERTURN;
        battleSpeaker.text = player.name;
        // battleText.text = playerText;
        // print("player line: " + currentPlayerLineID);
        // print("enemy line: " + currentEnemyLineID);
        ClearText();
        playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selectionNum]);
        currentEnemyLineID = BattleLineManager.S.RetrieveEnemyLineID(currentPlayerLineID);
        //TODO: display ALL enemy lines except the last one before letting player pick a choice
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
        enemyText.text = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID);
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

        // Temporary scaling fix until we can integrate the BeatManager more
        float scalingFactor = 0.8f;

        // Decrease slider value over timeToWait seconds
        while (slider.value > 0 && BeatManager.S.isPlayerLoop) {
            if(coolTimer.activeSelf == false) break;
            slider.value -= scalingFactor * Time.deltaTime/timeToWait;
            yield return null;
        }
        print("Choices timer completed");
        //TODO: let's not have 2 be the wrong answer everytime...
        if (!playerAnswered) ChooseInsult(2);

        SetChoices(false);
    }

    void ClearText() {
        playerText.text = "";
        enemyText.text = "";
        announcerText.text = "";
    }

}
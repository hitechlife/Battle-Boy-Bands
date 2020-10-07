using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, OPPONENTTURN, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    //TODO: replace with actual prefabs if needed?
    public GameObject playerPrefab;
    public GameObject opponentPrefab;

    Player player;
    Opponent opponent;

    public Text enemyText;
    public Text choice1;
    public Text choice2;

    public int numofx;
    public bool endgame;

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
        GameObject playerGO = Instantiate(playerPrefab);
        player = playerGO.GetComponent<Player>();

        GameObject opponentGO = Instantiate(opponentPrefab);
        opponent = opponentGO.GetComponent<Opponent>();

        enemyText.text = "vs. " + opponent.name;

        endgame = false;
        for (int i = 0; i < xs.Length; i++)
        {
            if (i < numofx)
            {
                xs[i].enabled = true;
            }
            else
            {
                xs[i].enabled = false;
            }
        }

        //TODO: sync with beatmanager
        yield return new WaitForSeconds(2f);

        StartCoroutine(PlayerTurn());
    }

    IEnumerator TryInsult(int selection)
    {
        state = BattleState.OPPONENTTURN;
        if (selection == 1)
        {
            enemyText.text = "vs. " + opponent.name + "- nice!!!";
            player.answered = true;
            if (numofx >0)
            {
                numofx = numofx- 1;
                xs[numofx].enabled = false;
            }
        }
        if (selection == 2)
        {
            enemyText.text = "vs. " + opponent.name + "- booooooo";
            if (numofx < 3)
            {
                xs[numofx].enabled = true;
                numofx = numofx+ 1;

            }
        }

        yield return new WaitForSeconds(2f);

        //TODO: win/lose is probably checked here
        if(numofx ==3)
        {
            endgame = true;
        }
        StartCoroutine(OpponentTurn());
    }

    IEnumerator PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        enemyText.text = "vs. " + opponent.name + "- your turn";
        //TODO: obviously... change... these are placeholders
        choice1.text = "this is correct";
        choice2.text = "this is incorrect";

        //TODO: sync with beatmanager
        yield return new WaitForSeconds(2f);

        // defaults to the wrong answer
        //TODO: let's not have 2 be the wrong answer everytime...
        if (!player.answered)
            ChooseInsult(2);
    }

    // OnClicked functions
    public void ChooseInsult(int selection)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(TryInsult(selection));
    }

    IEnumerator OpponentTurn()
    {
        state = BattleState.OPPONENTTURN;
        enemyText.text = "vs. " + opponent.name + "- opponent turn, sick burns";
        player.answered = false;
        //TODO: sync with beatmanager
        yield return new WaitForSeconds(2f);

        StartCoroutine(PlayerTurn());
    }
}

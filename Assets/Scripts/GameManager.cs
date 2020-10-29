using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    private int BossNum = 0;
    private List<Opponent> opponents;

    void Awake()
    {
        //check if instance exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyonLoad(this.gameObject);

        battleScript = GetComponent<BattleManager>();
        beatScript = GetComponent<BeatManager>();
    }

    void newBoss()
    {
        BossNum++;
        InitLevel();
    }

    void InitLevel()
    {
        //set up scene and stuff
    }
}


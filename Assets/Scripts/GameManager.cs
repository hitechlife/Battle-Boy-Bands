using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    struct enemy {
        public string name;
        public bool defeated;
    }
    public static GameManager instance = null;
    private static List<Opponent> opponents;

    //TODO: keep track of boss win or lose

    void Awake()
    {
        //check if instance exists
        if (instance == null)
        {
            instance = this;
            opponents.Add(new Opponent("Wash Depp", false, 6));
            opponents.Add(new Opponent("Kendrick Amore", false, 6));
            opponents.Add(new Opponent("Lil\' Pay", false, 9));
            for (int i = 0; i < opponents.Count; i++) {
                opponents[i].ID = i;
            }
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        // battleScript = GetComponent<BattleManager>();
        // beatScript = GetComponent<BeatManager>();
    }

    public void LoadBoss(int boss) {
        //TODO: call this from selection screen
        //TODO: load opponent info into BattleSystem!!
    }

    public void DefeatedBoss(int boss) {
        opponents[boss].defeated = true;
    }

    // void newBoss()
    // {
    //     BossNum++;
    //     InitLevel();
    // }

    public void InitLevel(string newScene)
    {
        //set up scene and stuff
        SceneManager.LoadScene(newScene);
    }
}
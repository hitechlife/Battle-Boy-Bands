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
    public static List<Opponent> opponents;
    //TODO: add list of sprites, that is initialized by loading assets from some folder? we probably need 1 folder per boss, so maybe a list of list of sprites would be better here (so like first list is boss1, second list boss2, etc)
    public static int currBoss;

    void Awake()
    {
        //check if instance exists
        if (instance == null)
        {
            instance = this;
            opponents = new List<Opponent>();
            opponents.Add(new Opponent("Wash Depp", false, 6));
            opponents.Add(new Opponent("Kendrick Amore", false, 6));
            opponents.Add(new Opponent("Lil\' Pay", false, 9));
            for (int i = 0; i < opponents.Count; i++) {
                opponents[i].SetID(i);
            }
            Debug.Log("initalized gamemanager");
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            Debug.Log("destroyed gamemanager");
        }
        DontDestroyOnLoad(this.gameObject);


        // battleScript = GetComponent<BattleManager>();
        // beatScript = GetComponent<BeatManager>();
    }

    // Called from selection screen
    public void LoadBoss(int boss) {
        currBoss = boss;
    }

    public void DefeatedBoss(int boss) {
        opponents[boss].Defeat();
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
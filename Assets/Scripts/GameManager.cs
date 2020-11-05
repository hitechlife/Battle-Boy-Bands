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

    void Awake()
    {
        //check if instance exists
        if (instance == null)
        {
            instance = this;
            opponents.Add(new Opponent("Wash Depp", false));
            opponents.Add(new Opponent("Kendrick Amore", false));
            opponents.Add(new Opponent("Lil\' Pay", false));
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        // battleScript = GetComponent<BattleManager>();
        // beatScript = GetComponent<BeatManager>();
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
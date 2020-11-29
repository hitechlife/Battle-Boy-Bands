using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Versioning;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // struct enemy {
    //     public string name;
    //     public bool defeated;
    // }

    public struct track {
        public int BPM;
        public AudioClip clip;
    }

    public static GameManager instance = null;
    public static List<Opponent> opponents;


    public static List<List<Sprite>> sprites;
    public static List<Sprite> boss1;
    public static List<Sprite> boss2;
    public static List<Sprite> boss3;
    public static List<Sprite> icons;
    public static List<Sprite> versus;
    public static List<track> musicTracks;
    public Text[] bossScores;
    private int[] BPM = { 95, 105, 120 };

    
    public static int currBoss;

    void Awake()
    {
        //check if instance exists
        if (instance == null)
        {
            instance = this;
            sprites = new List<List<Sprite>>();
            musicTracks = new List<track>();
            currBoss = 2; //TODO: temp testing line

            icons = new List<Sprite>();
            boss1 = new List<Sprite>();
            boss2 = new List<Sprite>();
            boss3 = new List<Sprite>();
            versus = new List<Sprite>();

            object[] loadedSprite = Resources.LoadAll("boss1", typeof(Sprite));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                boss1.Add((Sprite)loadedSprite[i]);
            }

            loadedSprite = Resources.LoadAll("boss2", typeof(Sprite));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                boss2.Add((Sprite)loadedSprite[i]);
            }

            loadedSprite = Resources.LoadAll("boss3", typeof(Sprite));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                boss3.Add((Sprite)loadedSprite[i]);
            }

            loadedSprite = Resources.LoadAll("bar with icons", typeof(Sprite));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                icons.Add((Sprite)loadedSprite[i]);
            }

            loadedSprite = Resources.LoadAll("tracks", typeof(AudioClip));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                musicTracks.Add(new track {
                    BPM = BPM[i],
                    clip = (AudioClip)loadedSprite[i],
                });
            }

            loadedSprite = Resources.LoadAll("versus", typeof(Sprite));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                versus.Add((Sprite)loadedSprite[i]);
            }


            sprites.Add(boss1);
            sprites.Add(boss2);
            sprites.Add(boss3);

            opponents = new List<Opponent>();
            opponents.Add(new Opponent("Wash Depp", false, 6, icons[0], null));
            opponents.Add(new Opponent("Kendrick Amore", false, 6, icons[1], null));
            opponents.Add(new Opponent("Lil\' Pay", false, 3, icons[2], null));
            for (int i = 0; i < opponents.Count; i++) {
                opponents[i].SetID(i);
            }

            Debug.Log("initalized gamemanager");
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            // Destroy(this.gameObject);
            Debug.Log("not the static gamemanager");
        }


        // battleScript = GetComponent<BattleManager>();
        // beatScript = GetComponent<BeatManager>();
    }

    void Start() {
        if (SceneManager.GetActiveScene().name == "BossSelectionMenu" && bossScores.Length == 3) {
            for (int i = 0; i < 3; i++) {
                bossScores[i].text = GetOpponentRank(i);
            }
        }
    }

    // Called from selection screen
    public void LoadBoss(int boss) {
        currBoss = boss;
    }

    public void DefeatedBoss(int boss) {
        opponents[boss].Defeat();
    }

    public string GetGrade(float score) {
        float maxScore = opponents[currBoss].GetTurns() * 2;
        if (score == maxScore) return "A+";
        float percentage = score / maxScore;
        if (percentage >= 0.9f) return "A";
        if (percentage >= 0.75f) return "B";
        if (percentage >= 0.5f) return "C";
        return "F";
    }

    public string GetOpponentRank(int opponent) {
        return opponents[opponent].GetRank();
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Versioning;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static FMOD.Studio.EventInstance selectSound;
    private static FMOD.Studio.EventInstance winloseselectSound;
    private static FMOD.Studio.EventInstance choiceselectSound;

    [FMODUnity.EventRef]
    public string fmodEvent = "event:/Music/Main Menu Select";
    public string fmodEvent1 = "event:/Music/Win Lose Select";
    public string fmodEvent2 = "event:/Music/Choice Select";


    // struct enemy {
    //     public string name;
    //     public bool defeated;
    // }

    public struct track
    {
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
    public static List<Sprite> mc;
    public static List<Sprite> versus;

    //voicelines for each boss
    public static List<List<AudioClip>> voicelines;
    public static List<AudioClip> boss1voice;
    public static List<AudioClip> boss2voice;
    public static List<AudioClip> boss3voice;
    public static List<AudioClip> mcVoice;
    public static List<AudioClip> announcerVoice;

    public static List<track> musicTracks;
    public Text[] bossScores;
    public GameObject[] redX;
    public GameObject endScreenButton;
    private int[] BPM = { 95, 105, 120 };


    public static int currBoss;
    public int bossesDefeated;

    void Awake()
    {
        //check if instance exists
        if (instance == null)
        {
            instance = this;
            sprites = new List<List<Sprite>>();
            voicelines = new List<List<AudioClip>>();
            musicTracks = new List<track>();
            currBoss = 0;
            bossesDefeated = 0;

            icons = new List<Sprite>();
            boss1 = new List<Sprite>();
            boss2 = new List<Sprite>();
            boss3 = new List<Sprite>();
            mc = new List<Sprite>();
            versus = new List<Sprite>();

            boss1voice = new List<AudioClip>();
            boss2voice = new List<AudioClip>();
            boss3voice = new List<AudioClip>();
            mcVoice = new List<AudioClip>();
            announcerVoice = new List<AudioClip>();

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

            loadedSprite = Resources.LoadAll("MC", typeof(Sprite));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                mc.Add((Sprite)loadedSprite[i]);
            }

            loadedSprite = Resources.LoadAll("bar with icons", typeof(Sprite));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                icons.Add((Sprite)loadedSprite[i]);
            }

            loadedSprite = Resources.LoadAll("tracks", typeof(AudioClip));
            for (int i = 0; i < loadedSprite.Length; i++)
            {
                musicTracks.Add(new track
                {
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
            sprites.Add(mc);

            object[] loadedVoice = Resources.LoadAll("boss1voice", typeof(AudioClip));
            for (int i = 0; i < loadedVoice.Length; i++)
            {
                boss1voice.Add((AudioClip)loadedVoice[i]);
            }
            loadedVoice = Resources.LoadAll("boss2voice", typeof(AudioClip));
            for (int i = 0; i < loadedVoice.Length; i++)
            {
                boss2voice.Add((AudioClip)loadedVoice[i]);
            }
            loadedVoice = Resources.LoadAll("boss3voice", typeof(AudioClip));
            for (int i = 0; i < loadedVoice.Length; i++)
            {
                boss3voice.Add((AudioClip)loadedVoice[i]);
            }
            loadedVoice = Resources.LoadAll("mcvoice", typeof(AudioClip));
            for (int i = 0; i < loadedVoice.Length; i++)
            {
                mcVoice.Add((AudioClip)loadedVoice[i]);
            }
            loadedVoice = Resources.LoadAll("announcervoice", typeof(AudioClip));
            for (int i = 0; i < loadedVoice.Length; i++)
            {
                announcerVoice.Add((AudioClip)loadedVoice[i]);
            }

            voicelines.Add(boss1voice);
            voicelines.Add(boss2voice);
            voicelines.Add(boss3voice);
            voicelines.Add(mcVoice);
            voicelines.Add(announcerVoice);

            opponents = new List<Opponent>();
            opponents.Add(new Opponent("Wash Depp", false, 9, icons[0], null));
            opponents.Add(new Opponent("Kendrick Amore", false, 9, icons[1], null));
            opponents.Add(new Opponent("Lil\' Pay", false, 10, icons[2], null));
            for (int i = 0; i < opponents.Count; i++)
            {
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

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "BossSelectionMenu" && bossScores.Length == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                bossScores[i].text = "Your rank:\n" + GetOpponentRank(i);
                if (bossScores[i].text.Contains("Rap")) {
                    bossScores[i].color = Color.green;
                } else {
                    bossScores[i].color = Color.white;
                }
                if (opponents[i].GetDefeated()) {
                    redX[i].SetActive(true);
                }
            }

            if (GameManager.instance.bossesDefeated >= opponents.Count) {
                endScreenButton.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Called from selection screen
    public void LoadBoss(int boss)
    {
        currBoss = boss;
    }

    public void DefeatedBoss(int boss)
    {
        opponents[boss].Defeat();
        if (bossesDefeated < opponents.Count) bossesDefeated++;
    }

    public string GetGrade(float score)
    {
        float maxScore = opponents[currBoss].GetTurns() * 2;
        if (score == maxScore) return "Rap Legend";
        float percentage = score / maxScore;
        if (percentage >= 0.9f) return "Rap God";
        if (percentage >= 0.75f) return "Rap Amateur";
        if (percentage >= 0.5f) return "Rap Noob";
        return "Fail";
    }

    public string GetOpponentRank(int opponent)
    {
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

    public void PlaySelectSound()
    {
         selectSound = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
         selectSound.start();
    }

    public void PlayWinLoseSelectSound()
    {
        winloseselectSound = FMODUnity.RuntimeManager.CreateInstance(fmodEvent1);
        winloseselectSound.start();
    }

    public void ChoiceSelectSound()
    {
        choiceselectSound = FMODUnity.RuntimeManager.CreateInstance(fmodEvent2);
        choiceselectSound.start();
    }
}

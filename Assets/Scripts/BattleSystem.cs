using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, PLAYERCHOICE, OPPONENTTURN, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    private static FMOD.Studio.EventInstance Sound;
    private static FMOD.Studio.EventInstance Music;

    [FMODUnity.EventRef]
    public string fmodEvent = "event:/Music/Track 3";

    [SerializeField]
    [Range(-12f, 12f)]
    private float Points;

    //TODO: probably make stuff serialized and not public

    [SerializeField] private AudioSource voicePlayer;
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject playerLosePrefab;
    public GameObject opponentPrefab;
    private Image playerPose;
    private Image opponentPose;
    public GameObject versusScreen;
    public Text opponentInfoText;
    public Image opponentInfoIcon;
    public int maxTurns = 3;

    Player player;
    Opponent opponent;

    public Text enemyText;
    public Text playerText;
    public Text battleSpeaker;
    public Text announcerText;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public Text LoseRank;
    public Text LoseScore;
    public Text WinRank;
    public Text WinScore;

    // Since we only have 3 choices, easier to access each separately
    // Put into array??
    public GameObject[] choices;
    // public GameObject choice0;
    // public GameObject choice1;
    // public GameObject choice2;
    private int[] choicesMap;
    public GameObject coolTimer;

    // Score system
    public Slider scoreSlider;

    private bool gameOver;
    private bool playerAnswered;
    // private string enemyText = "sad rap line";
    // private string playerText = "...";

    // X system
    private int numOfX = 0;
    public Image[] xs;
    public Sprite disabledX;
    public Sprite enabledX;

    [Header("Line IDs - make sure to only initialize the Enemy ID")]
    private int enemyID;
    [SerializeField]
    private int currentEnemyLineID;
    [SerializeField]
    private int currentPlayerLineID;

    private int selectionNum;
    [SerializeField] private GameObject[] tierMarkings;
    // [SerializeField] private GameObject[] tierTextMarkings;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField] private GameObject crowd;

    private float baseTimeScale;

    private void Awake()
    {
        baseTimeScale = Time.timeScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        // Music.start();

        playerPose = playerPrefab.GetComponent<Image>();
        opponentPose = opponentPrefab.GetComponent<Image>();
        choicesMap = new int[3];
        state = BattleState.START;
        StopAllCoroutines();
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        // crowd.SetActive(false);
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        Music.setParameterByName("Points", Points);

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            // Fix this later
            // Time.timeScale = Time.timeScale == baseTimeScale ? 0.0f : baseTimeScale;
            // pauseMenu.SetActive(Time.timeScale != baseTimeScale);
        }
    }

    // Sets up the initial battle state
    IEnumerator SetupBattle()
    {
        //need to instantiate at specific coords?
        player = playerPrefab.GetComponent<Player>();
        opponent = GameManager.opponents[GameManager.currBoss];

        // Render opponent info and set the track

        // Setup challenger sprites: 0 challenge, 1 sing, 2 loss, 3 win
        opponentPose.sprite = GameManager.sprites[GameManager.currBoss][0];
        playerPose.sprite = GameManager.sprites[GameManager.opponents.Count][0];

        // Opponent first line
        //opponentPrefab.GetComponent<AudioSource>().clip = GameManager.voicelines[GameManager.currBoss][0];

        // Opponent name and icon
        opponentInfoText.text = GameManager.currBoss == 1 ? opponent.GetName().Split(' ')[0] : opponent.GetName(); ;
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

        // Initialize scoring
        scoreSlider.value = 0;
        scoreSlider.maxValue = opponent.GetTurns() * 2;

        // Start music
        Music = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        Music.start();
        PlayAnnouncerVoiceLine(GameManager.currBoss);

        //Open start screen
        versusScreen.SetActive(true);
        versusScreen.GetComponent<Image>().sprite = GameManager.versus[GameManager.currBoss];

        battleSpeaker.text = "Announcer";
        announcerText.text = "You vs. " + opponent.GetName();

        // Initial loop
        while (BeatManager.S.isFirstLoop)
        {
            if (BeatManager.S.counter > BeatManager.S.NUM_BREAK_BARS / 2)
            {
                versusScreen.SetActive(false);
            } else {
                versusScreen.SetActive(true);
            }
            yield return null;
        }
        print("DONE");

        StartCoroutine(BattleRoutine());
    }

    IEnumerator BattleRoutine()
    {
        Debug.Log("starting battle routine");
        // Game always ends on your turn
        for (int i = 0; i < maxTurns; i++)
        {
            Debug.Log("On turn " + (i + 1));
            // Starts with enemy insult

            yield return StartCoroutine(OpponentTurn());

            // Opponent singing, player challenging
            playerPrefab.SetActive(true);
            playerLosePrefab.SetActive(false);
            opponentPose.sprite = GameManager.sprites[GameManager.currBoss][1];
            playerPose.sprite = GameManager.sprites[GameManager.opponents.Count][0];
            bool played = false;

            // yield return new WaitForSeconds(2f);
            while (BeatManager.S.isEnemyLoop)
            {
                if (!played) {
                    played = true;
                    voicePlayer.Stop();
                    PlayEnemyVoiceLine(currentEnemyLineID);
                }
                print(enemyID);
                print(currentEnemyLineID);
                // First half
                if (BeatManager.S.counter <= BeatManager.S.NUM_BREAK_BARS / 2)
                {
                    float delay = 60f * (BeatManager.S.SUBDIVISION_CONST * (BeatManager.S.NUM_BREAK_BARS / 2)) / BeatManager.S.beatsPerMinute;
                    string line = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID).Split('/')[0];
                    // yield return StartCoroutine(EnemyTyper(line,delay));
                    enemyText.text = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID).Split('/')[0];

                }
                else
                {
                    float delay = 60f * (BeatManager.S.SUBDIVISION_CONST * (BeatManager.S.NUM_BREAK_BARS / 2)) / BeatManager.S.beatsPerMinute;
                    string line = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID).Split('/')[1];
                    // yield return StartCoroutine(EnemyTyper(line,delay));
                    enemyText.text = BattleLineManager.S.RetrieveEnemyLine(enemyID, currentEnemyLineID).Split('/')[1];
                }
                yield return null;
            }

            // Opponent challenging, player challenging
            opponentPose.sprite = GameManager.sprites[GameManager.currBoss][0];
            playerPose.sprite = GameManager.sprites[GameManager.opponents.Count][0];

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
            //TODO: have more theatrics about displaying your chosen lines
            yield return StartCoroutine(PlayerTurn());

            // Opponent challenging, player singing
            opponentPose.sprite = GameManager.sprites[GameManager.currBoss][0];
            playerPose.sprite = GameManager.sprites[GameManager.opponents.Count][1];

            bool updatedChoice = false;
            bool updatedPoints = false;
            played = false;
            // yield return new WaitForSeconds(4f);
            while (BeatManager.S.isPlayerResponseLoop)
            {
                if (!played) {
                    played = true;
                    voicePlayer.Stop();
                    PlayPlayerVoiceLine(currentPlayerLineID);
                }
                // First half
                if (BeatManager.S.counter <= BeatManager.S.NUM_BREAK_BARS / 2)
                {
                    // playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selectionNum]).Split('/')[0];
                }
                else
                { //Second half
                    playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selectionNum]).Split('/')[1];

                    // Display these after timer complete based on choice
                    if (!updatedChoice)
                    {
                        updatedChoice = true;
                        switch (selectionNum)
                        {
                            case 0:
                                // if (numOfX > 0) {
                                //     numOfX--;
                                // }
                                // xs[numOfX].sprite = disabledX;

                                // Opponent losing, player winning
                                opponentPose.sprite = GameManager.sprites[GameManager.currBoss][2];
                                playerPose.sprite = GameManager.sprites[GameManager.opponents.Count][3];

                                StartCoroutine(FillScore(scoreSlider.value + 2));
                                PlayCheers();
                                StartCoroutine(AnimateCrowd());

                                //changing back
                                //opponentPrefab.GetComponent<Image>().sprite = GameManager.sprites[GameManager.currBoss][0];
                                break;
                            case 1:

                                // No sprite change

                                PlayNeutral();
                                StartCoroutine(FillScore(scoreSlider.value + 1));
                                break;
                            case 2:
                                // xs[Mathf.Min(numOfX,xs.Length-1)].sprite = enabledX;

                                // Opponent winning, player losing
                                opponentPose.sprite = GameManager.sprites[GameManager.currBoss][3];
                                playerPrefab.SetActive(false);
                                playerLosePrefab.SetActive(true);
                                // playerPose.sprite = GameManager.sprites[GameManager.opponents.Count][2];
                                PlayBoos();
                                // if (numOfX < xs.Length) {
                                //     numOfX++;
                                // }
                                break;
                            default:
                                break;
                        }
                    }

                    // Temp fix to transitioning to next bar
                    if (BeatManager.S.accent == BeatManager.S.SUBDIVISION_CONST && BeatManager.S.counter == BeatManager.S.NUM_BREAK_BARS && !updatedPoints)
                    {
                        updatedPoints = true;
                        // Display these after timer complete based on choice
                        switch (selectionNum)
                        {
                            case 0:
                                if (Points > 0)
                                {
                                    Points--;
                                }
                                break;
                            case 1:
                                break;
                            case 2:
                                if (Points < xs.Length)
                                {
                                    Points++;
                                    print("increased points");
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                yield return null;
            } // end while loop

            currentEnemyLineID = BattleLineManager.S.RetrieveEnemyLineID(currentPlayerLineID);
            // enemy text should be set by TryInsult at this point

            // Break out of loop if 3 x's
            // if (numOfX >= xs.Length) {
            //     break;
            // }
        }

        EndBattle();
    }

    void EndBattle()
    {
        // Stop music?
        Debug.Log("Ending game");
        gameOver = true;
        // battleSpeaker.text = "Announcer";
        // string winner = numOfX >= xs.Length ? opponent.GetName() : player.name;
        // ClearText();
        // announcerText.text = "And the winner is... " + winner + "!!";

        string rank = GameManager.instance.GetGrade(scoreSlider.value);
        opponent.SetRank(rank);

        if (rank.Contains("Rap"))
        {
            WinRank.text = rank;
            WinScore.text = "" + Mathf.Floor(scoreSlider.value) + "/" + scoreSlider.maxValue;
            WinPanel.SetActive(true);
            PlayCheers();
            GameManager.instance.DefeatedBoss(enemyID);
        }
        else
        {
            LoseRank.text = rank;
            LoseScore.text = "" + Mathf.Floor(scoreSlider.value) + "/" + scoreSlider.maxValue;
            LosePanel.SetActive(true);
            PlayBoos();
        }
        SetChoices(false);
        // ResultsPanel.SetActive(true);
        // for (int i = 0; i < ResultsPanel.transform.childCount; i++) {
        //     GameObject g = ResultsPanel.transform.GetChild(i).gameObject;
        //     if (g.name == "CongratsText") { //TODO: change this
        //         g.GetComponent<Text>().text = "And the winner is... " + winner + "!!";
        //     }
        // }
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Music.release();
        StopAllCoroutines();
    }

    IEnumerator EnemyTyper(string line, float delay)
    {
        enemyText.text = "";
        //sets delay for each character
        delay = (delay - 0.5f) / (line.Length);
        foreach (char c in line)
        {
            enemyText.text += c;
            yield return new WaitForSeconds(delay);
        }
        //keep it there for a moment
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator TryInsult(int selection)
    {
        currentPlayerLineID = BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[selection];
        selectionNum = selection;

        switch (selection)
        {
            case 0: //good

                choices[choicesMap[1]].GetComponentInChildren<Button>().interactable = false;
                choices[choicesMap[2]].GetComponentInChildren<Button>().interactable = false;
                break;
            case 1: //ok

                choices[choicesMap[0]].GetComponentInChildren<Button>().interactable = false;
                choices[choicesMap[2]].GetComponentInChildren<Button>().interactable = false;
                break;
            case 2: //bad

                choices[choicesMap[1]].GetComponentInChildren<Button>().interactable = false;
                choices[choicesMap[0]].GetComponentInChildren<Button>().interactable = false;
                break;
            default: //invalid choice
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
        //TODO: have giant text flash saying "YOUR TURN"???
        state = BattleState.PLAYERCHOICE;
        battleSpeaker.text = "YOU CHOOSE";
        playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[0]).Split('/')[0];
        playerText.text += "\n\n(Get ready to choose a second line...)";

        int i = 0;
        int[] arr = { 0, 1, 2 };
        int[] randChoice = shuffle(arr);

        foreach (GameObject choice in choices)
        {
            int j = randChoice[i];
            choicesMap[j] = i;

            choice.GetComponentInChildren<Text>().text = BattleLineManager.S.RetrieveChoiceLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[j]);

            choice.GetComponent<Button>().onClick.RemoveAllListeners();

            choice.GetComponent<Button>().onClick.AddListener(() => ChooseInsult(j));
            i += 1;
        }

        // int doubleTime = BeatManager.doubleTime ? 2 : 1;

        yield return new WaitUntil(() => BeatManager.S.counter > BeatManager.S.NUM_BREAK_BARS/2);
        yield return new WaitUntil(() => BeatManager.S.playerLoopInt >= 2 * GameManager.instance.bossesDefeated + 2);

        SetChoices(true);
        playerText.text = BattleLineManager.S.RetrievePlayerLine(BattleLineManager.S.RetrievePlayerLines(enemyID, currentEnemyLineID)[0]).Split('/')[0];

        // Runs for 1.5 x BeatManger
        yield return StartCoroutine(ChoicesTimer(60f * (BeatManager.S.SUBDIVISION_CONST * (BeatManager.S.NUM_BREAK_BARS + BeatManager.S.NUM_BREAK_BARS / 2)) / (BeatManager.S.beatsPerMinute) - (4 * GameManager.instance.bossesDefeated)));

        // yield return new WaitForSeconds(2f);
        // yield return null;
    }

    IEnumerator PlayerTurn()
    {
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
        yield return null;

        // StartCoroutine(PlayerTurn());
    }

    void SetChoices(bool active)
    {
        foreach (GameObject choicei in choices)
        {
            choicei.SetActive(active);
        }
        coolTimer.SetActive(active);
        foreach (GameObject choicei in choices)
        {
            choicei.GetComponentInChildren<Button>().interactable = true;
        }

        // if (active) StartCoroutine(ChoicesTimer(2f /*BeatManager.S.SUBDIVISION_CONST*/));
    }

    IEnumerator AnimateCrowd() {
        // crowd.SetActive(true);
        object[] loadedSprite = Resources.LoadAll("Crowd", typeof(Sprite));
        // FMOD.Studio.PLAYBACK_STATE state = FMOD.Studio.PLAYBACK_STATE.PLAYING;
        // float timer = 0;
        float animateInterval = 0.05f;
        while (BeatManager.S.isPlayerResponseLoop) { // hardcoded len of crowd noise
            foreach (object obj in loadedSprite) {
                Sprite s = (Sprite)obj;
                crowd.GetComponent<Image>().sprite = s;
                // timer += animateInterval;
                yield return new WaitForSeconds(animateInterval);
                if (!BeatManager.S.isPlayerResponseLoop) break;
            }
            // timer += Time.deltaTime;
            yield return null;
            // Sound.getPlaybackState(out state);
        }
        // crowd.SetActive(false);
    }

    // Animate the score meter filling up
    IEnumerator FillScore(float target)
    {
        while (scoreSlider.value <= target)
        {
            scoreSlider.value += Time.deltaTime;

            // Check if we've reached a tier
            if (scoreSlider.value / scoreSlider.maxValue >= 0.9)
            {
                ReachedTier(0);
            }
            if (scoreSlider.value / scoreSlider.maxValue >= 0.75)
            {
                ReachedTier(1);
            }
            if (scoreSlider.value / scoreSlider.maxValue >= 0.5)
            {
                ReachedTier(2);
            }
            yield return null;
        }
    }

    IEnumerator ChoicesTimer(float timeToWait)
    {
        Slider slider = coolTimer.GetComponentInChildren<Slider>();
        if (!slider) Debug.LogError("Slider not valid!");
        slider.value = 1;

        bool flippedOn = false;
        timeToWait--;

        // Decrease slider value over timeToWait seconds
        while (BeatManager.S.isPlayerLoop)
        {
            if (coolTimer.activeSelf == false) break;
            slider.value -= Time.deltaTime / timeToWait;

            //TODO: skipping beat bug fix temp

            // If we are *right before* the end of the timer
            if (BeatManager.S.accent == BeatManager.S.SUBDIVISION_CONST && BeatManager.S.counter == BeatManager.S.NUM_BREAK_BARS && !flippedOn && slider.value == 0)
            {
                flippedOn = true;
                if (!playerAnswered) ChooseInsult(2);
                // Display these after timer complete based on choice
                switch (selectionNum)
                {
                    case 0:
                     //   if (Points > 0)
                      //  {
                      //      Points--;
                      //  }
                        break;
                    case 1:
                        break;
                    case 2:
                   //     if (Points < xs.Length)
                     //   {
                     //       Points++;
                       // }
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
        print("Choices timer completed");
        // if (!playerAnswered) {
        //     ChooseInsult(2);
        // }

        SetChoices(false);
    }

    void ClearText()
    {
        playerText.text = "";
        enemyText.text = "";
        announcerText.text = "";
    }

    // Changes color to white if you reach a tier
    void ReachedTier(int i)
    {
        Image[] images = tierMarkings[i].GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.color = Color.white;
        }
    }
    public void PlayBoos()
    {
        Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Crowd Noises/Crowd Boos");
        // if (Points < xs.Length)
        // {
        //     Points++;
        // }
        Sound.start();
        Sound.release();
    }

    public void PlayEnemyVoiceLine(int lineToPlay)
    {
        if (GameManager.voicelines[GameManager.currBoss].Count <= lineToPlay) return;

        voicePlayer.clip = GameManager.voicelines[GameManager.currBoss][lineToPlay];
        voicePlayer.Play();
    }

    public void PlayPlayerVoiceLine(int lineToPlay)
    {
        if (GameManager.voicelines[GameManager.opponents.Count].Count <= lineToPlay) return;

        voicePlayer.clip = GameManager.voicelines[GameManager.opponents.Count][lineToPlay];
        voicePlayer.Play();
    }

    public void PlayAnnouncerVoiceLine(int lineToPlay)
    {
        if (GameManager.voicelines[GameManager.opponents.Count+1].Count <= lineToPlay) return;

        voicePlayer.clip = GameManager.voicelines[GameManager.opponents.Count+1][lineToPlay];
        voicePlayer.Play();
    }

    public void PlayCheers()
    {
        Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Crowd Noises/Crowd Cheers");
        // if (Points > 0)
        // {
        //     Points--;
        // }
        Sound.start();
        Sound.release();
    }

    public void PlayNeutral()
    {
        Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Crowd Noises/Crowd Neutral");
        Sound.start();
        Sound.release();
    }

    //TODO: had to put this here because the continue button uses THIS scene's
    // game manager and not the BOSS SCENE'S game manager which is different??
    // idk it's weird
    public void LoadSelectScene(string toLoad)
    {
        SceneManager.LoadScene(toLoad);
    }


    int[] shuffle(int[] array)
    {
        var currentIndex = array.Length;
        int temporaryValue;
        int randomIndex;

        // While there remain elements to shuffle...
        while (0 != currentIndex)
        {

            // Pick a remaining element...
            randomIndex = Random.Range(0, currentIndex);
            currentIndex -= 1;

            // And swap it with the current element.
            temporaryValue = array[currentIndex];
            array[currentIndex] = array[randomIndex];
            array[randomIndex] = temporaryValue;
        }

        return array;
    }

}

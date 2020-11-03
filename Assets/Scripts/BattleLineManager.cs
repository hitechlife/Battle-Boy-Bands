using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLineManager : MonoBehaviour
{
    [Header("Index of the enemy in the scene")]
    // [SerializeField]
    // private int enemyID;

    public static BattleLineManager S;

    // playerLineIDs[Enemy ID][Enemy Line ID] => int[] of player lines
    public int[][][] playerLineIDs =
    {
        new int[][]
        {
            new int[] { 0, 1, 2 },
            new int[] { 3, 4, 5 },
            new int[] { 6, 7, 8 },
            new int[] { 9, 10, 11 },
            new int[] { 12, 13, 14 }
        },
        new int[][]
        {
            new int[] { 9, 10, 11 },
            new int[] { 12, 13, 14 }
        }
    };

    // playerLineIDToEnemyLineID[Player Line ID] => Enemy Line ID
    public Dictionary<int, int> playerLineIDToEnemyLineID = new Dictionary<int, int>
    {
        { 0, 1 },
        { 1, 1 },
        { 2, 1 },
        { 3, 2 },
        { 4, 2 },
        { 5, 2 },
        { 6, 3 },
        { 7, 3 },
        { 8, 3 },
        { 9, 9 },
        { 10, 10 },
        { 11, 11 },
        { 12, 12 },
        { 13, 13 },
        { 14, 14 }
    };

    // playerLines[Player Line ID] => player line as a string
    private Dictionary<int, string> playerLines = new Dictionary<int, string>
    {
        { 0, "I don't wanna fight a toddler but for now I’ll bend that rule\nWanna pray to a rap god kid? I’ll kick your ass to Sunday school" },
        { 1, "I don't wanna fight a toddler but for now I’ll bend that rule\n Everytime you see people you always have some drool" },
        { 2, "I don't wanna fight a toddler but for now I’ll bend that rule\n Your rhymes are so sad" },
        { 3, "Guess you’re still young enough to be throwing tantrums\nTime to tell your mom to stop buying out your albums" },
        { 4, "Guess you’re still young enough to be throwing tantrums\nI came to win, you came to have fun" },
        { 5, "Guess you’re still young enough to be throwing tantrums\nKeep cryin’ all day while I stick you in my van" },
        { 6, "Nice try kid, but you can't cover up that stink\nAnd like the titanic, your popularity will sink"},
        { 7, "Nice try kid, but you can't cover up that stink\n It smells so bad the audience fainted, you dink" },
        { 8, "Nice try kid, but you can't cover up that stink\n You're so small" },
        { 9, "ninth test line" },
        { 10, "eleventh test line" },
        { 11, "twelfth test line" },
        { 12, "thirteenth test line" },
        { 13, "fourteenth test line" },
        { 14, "fifteenth test line" }
    };

    private Dictionary<int, string> choiceLines = new Dictionary<int, string> {
        { 0, "[Sunday School]" },
        { 1, "[Drool]" },
        { 2, "[Sad]" },
        { 3, "[Albums]" },
        { 4, "[Have fun]" },
        { 5, "[My van]" },
        { 6, "[Popularity will sink]"},
        { 7, "[You dink]" },
        { 8, "[So small]" },
        { 9, "tenth test line" },
        { 10, "eleventh test line" },
        { 11, "twelfth test line" },
        { 12, "thirteenth test line" },
        { 13, "fourteenth test line" },
        { 14, "fifteenth test line" }
    };

    /*
     * HOT BARS
     * 
     * Shaking so hard like you're my biggest fan
     * Well now get ready for the frying pan
     * Spitting bars so hot they'll think I'm grillin'
     * You're squirmin' and spittin' while I'm just chillin'
     * ...
     * ...
     * I'm dope, I'm slick, I'm hot, I'm greasy
     * Cookin' your sad face like it's over-easy
     */

    // enemyLines[Enemy ID][Response ID]
    public string[][] enemyLines = {
        new string[] { // enemy 1
            "Welcome to the final now it’s time for Lil’ Pay \nWhen I walk up on the stage y’all know it’s time to lil pray", 
            "You’re just a Millennial, don’t try to mess with Gen Z \nI already reached platinum with just a single LP", 
            "They call me lil' pay but I ain't so small \nI'm king of the world, you're a fly on the wall", 
            "Fourth enemy line", 
            "Fifth enemy line", 
            "Sixth enemy line", 
            "Seventh enemy line", 
            "Eighth enemy line",
            "Ninth enemy line",
            "Tenth enemy line",
            "Eleventh enemy line",
            "Twelfth enemy line",
            "Thirteenth enemy line",
            "Fourteenth enemy line",
            "Fifteenth enemy line"
        },
        new string[] { "Next enemy line", "Next next enemy line" }
    };

    // TODO: Lookup a new enemy line id

    private HashSet<int> usedPlayerLineIDs;
    private HashSet<int> usedEnemyLineIDs;

    private void Awake()
    {
        S = this;
        usedPlayerLineIDs = new HashSet<int>();
        usedEnemyLineIDs = new HashSet<int>();
    }

    /// <summary>
    /// Retrieves player lines for a given enemy line ID.
    /// </summary>
    /// <param name="enemyLineID">Number representing the line the enemy says.</param>
    /// <returns></returns>
    public int[] RetrievePlayerLines(int enemyID, int enemyLineID)
    {
        return playerLineIDs[enemyID][enemyLineID];
    }

    public string RetrievePlayerLine(int playerLineLookupID)
    {
        return playerLines[playerLineLookupID];
    }

    public int RetrieveEnemyLineID(int playerLineID)
    {
        return playerLineIDToEnemyLineID[playerLineID];
    }

    public string RetrieveEnemyLine(int enemyID, int enemyLineLookupID)
    {
        return enemyLines[enemyID][enemyLineLookupID];
    }

    public string RetrieveChoiceLine(int playerLineID) {
        return choiceLines[playerLineID];
    }
}

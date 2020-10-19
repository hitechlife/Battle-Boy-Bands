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
        { 2, 2 },
        { 3, 3 },
        { 4, 4 },
        { 5, 5 },
        { 6, 6 },
        { 7, 7 },
        { 8, 8 },
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
        { 0, "first test line" },
        { 1, "second test line" },
        { 2, "third test line" },
        { 3, "fourth test line" },
        { 4, "fifth test line" },
        { 5, "sixth test line" },
        { 6, "seventh test line" },
        { 7, "eighth test line"},
        { 8, "ninth test line" },
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
        new string[] { 
            "First enemy line", 
            "Second enemy line", 
            "Third enemy line", 
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
}

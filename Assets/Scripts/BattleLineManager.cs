using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLineManager : MonoBehaviour
{
    [Header("Index of the enemy in the scene")]
    [SerializeField]
    private int enemyID;

    public static BattleLineManager S;

    // playerLineIDs[Enemy ID][Enemy Line ID] => int[] of player lines
    public int[][][] playerLineIDs =
    {
        new int[][]
        {
            new int[] { 0, 1, 2, 3 },
            new int[] { 6, 4, 2, 0 },
            new int[] { 3, 5, 4, 1 }
        },
        new int[][]
        {
            new int[] { 7, 8, 9, 10 },
            new int[] { 11, 12, 13, 14 }
        }
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

    // enemyLines[Enemy ID][Response ID]
    public string[][] enemyLines = {
        new string[] { "First enemy line", "Second enemy line", "Third enemy line", "Fourth enemy line", "Fifth enemy line" },
        new string[] { "Sixth enemy line", "Seventh enemy line" }
    };

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
    public int[] RetrievePlayerLines(int enemyLineID)
    {
        return playerLineIDs[enemyID][enemyLineID];
    }

    public string RetrievePlayerLine(int playerLineLookupID)
    {
        return playerLines[playerLineLookupID];
    }

    public string RetrieveEnemyLine(int enemyLineLookupID)
    {
        return enemyLines[enemyID][enemyLineLookupID];
    }
}

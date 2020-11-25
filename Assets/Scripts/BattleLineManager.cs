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
        },
        new int[][]
        {
            new int[] { 0, 1, 2 },
            new int[] { 3, 4, 5 },
            new int[] { 6, 7, 8 },
            new int[] { 9, 10, 11 },
            new int[] { 12, 13, 14 },
            new int[] { 15, 16, 17 },
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
        { 9, 4 },
        { 10, 4 },
        { 11, 4 },
        { 12, 5 },
        { 13, 5 },
        { 14, 5 },
        { 15, 6 },
        { 16, 6 },
        { 17, 6 },
    };

    // playerLines[Player Line ID] => player line as a string
    private Dictionary<int, string> playerLines = new Dictionary<int, string>
    {
        { 0, "I don't wanna fight a toddler\nbut for now I’ll bend that rule/Wanna pray to a rap god kid?\nI’ll kick your ass to Sunday school" },
        { 1, "I don't wanna fight a toddler\nbut for now I’ll bend that rule/ I’m gonna trounce you\nsince I’m so cool" },
        { 2, "I don't wanna fight a toddler\nbut for now I’ll bend that rule/haha you suck,\nand that’s the truth" },
        { 3, "You're talkin\' like a soldier\nbut ain’t old enough to draft/Kid you wanna be on COD\nbut you're still stuck on Minecraft" },
        { 4, "You're talkin\' like a soldier\nbut ain’t old enough to draft/your rap game just died,\npress F in chat" },
        { 5, "You're talkin\' like a soldier\nbut ain’t old enough to draft/if you did you’d,\nuhh, just eat crayons" },
        { 6, "Your followers don't mean shit\nwhen we know that they’re all bought/Now just drop the mic\nand run back to ur job at Kidz Bop"},
        { 7, "Your followers don't mean shit\nwhen we know that they’re all bought/so basically all your bots\nfollow a rock" },
        { 8, "Your followers don't mean shit\nwhen we know that they’re all bought/and your stage presence\nreally really sucks" },
        { 9, "Guess you’re still young enough\nto be throwing tantrums/Time to tell your mom\nto stop buying out your albums" },
        { 10, "Guess you’re still young enough\nto be throwing tantrums/when your parents say \"I love you\"\nit’s definitely sarcasm" },
        { 11, "Guess you’re still young enough\nto be throwing tantrums/I’ll stick you in my van\nand then I’ll laugh, haha" },
        { 12, "Nice try kid,\nbut you can't cover up that stink/And like the titanic,\nyour popularity will sink" },
        { 13, "Nice try kid,\nbut you can't cover up that stink/It makes me squint so hard\nit looks like I wink " },
        { 14, "Nice try kid,\nbut you can't cover up that stink/You are a baby,\nwho went a poopy" },
        { 15, "Hate to break it to you, kid,\nbut there's more to the world/Than just your favorite Peppa Pig,\nSpongebob, and Big Bird" },
        { 16, "Hate to break it to you, kid,\nbut there's more to the world/Than dancing,\nand how to do a twirl" },
        { 17, "Hate to break it to you, kid,\nbut there's more to the world/Than you,\ncuz you’re so small" }
    };

    private Dictionary<int, string> choiceLines = new Dictionary<int, string> {
        { 0, "[Kick your ass to Sunday School]" },
        { 1, "[I’m so cool]" },
        { 2, "[You suck]" },
        { 3, "[You’re stuck on Minecraft]" },
        { 4, "[Press F in chat]" },
        { 5, "[Eat crayons]" },
        { 6, "[Your job at Kidz Bop]"},
        { 7, "[Your bots follow a rock]" },
        { 8, "[really really suck]" },
        { 9, "[Mom buys out your albums]" },
        { 10, "[It’s definitely sarcasm]" },
        { 11, "[I laugh, haha]" },
        { 12, "[Popularity will sink]" },
        { 13, "[Looks like I wink]" },
        { 14, "[You went a poopy]" },
        { 15, "[Your favorite Big Bird]" },
        { 16, "[Do a twirl]" },
        { 17, "[You’re so small]" },
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
        new string[] { "Next enemy line", "Next next enemy line" },
        new string[] { "Next enemy line", "Next next enemy line" },
        new string[] { // enemy 3
            "Welcome to the final\nnow it’s time for Lil’ Pay/When I walk up on the stage\ny’all know it’s time to lil pray", 
            "Bitch I’m 9 years old\n 'n I’m already flexin millions/Lil Pay’s on Call of Duty\nwhile ur ass is a civilian", 
            "Ok Boomer you're just mad\ncuz I’m already verified/Lil Pay’s an asteroid,\ncall this stage a genocide", 
            "You’re just a Millennial,\ndon’t try to mess with Gen Z/I already reached platinum\nwith just a single LP", 
            "They call me lil' pay\nbut I ain't so small/ I'm king of the world,\nyou're a fly on the wall", 
            /*"Nobody understands\nyour obscure 90's references/Keep it to yourself,\nwe don't care 'bout your preferences", 
            "I’m in it, legit bars\ntoo hype to bite / I’m winnin’,\nmom’s spaghetti-n’ your appetite", 
            "I might look small\nbut I’m big on TikTok / My rhymes hit you\nlike an electric shock",
            "Respect your elders?\nWhy even try? / I got a baby face\nbut you’re gonna be the one to cry",
            "Listen man,\nI’m way more endowed than you / In cash, in swag,\nand in, heh, other ways too"*/
        }
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

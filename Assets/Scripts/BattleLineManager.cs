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
        // Wash Depp
        new int[][]
        {
            new int[] { 0, 1, 2 },
            new int[] { 3, 4, 5 },
            new int[] { 6, 7, 8 },
            new int[] { 9, 10, 11 },
            new int[] { 12, 13, 14 },
            new int[] { 15, 16, 17 },
            new int[] { 18, 19, 20 },
            new int[] { 21, 22, 23 },
            new int[] { 24, 25, 26 },
            // new int[] { 27, 28, 29 }
        },
        // Kendrick Amore
        new int[][]
        {
            new int[] { 30, 31, 32 },
            new int[] { 33, 34, 35 },
            new int[] { 36, 37, 38 },
            new int[] { 39, 40, 41 },
            new int[] { 42, 43, 44 },
            new int[] { 45, 46, 47 },
            new int[] { 48, 49, 50 },
            new int[] { 51, 52, 53 },
            new int[] { 54, 55, 56 },
            // new int[] { 57, 58, 59 }
        },
        // Lil Pay
        new int[][]
        {
            new int[] { 60, 61, 62 },
            new int[] { 63, 64, 65 },
            new int[] { 66, 67, 68 },
            new int[] { 69, 70, 71 },
            new int[] { 72, 73, 74 },
            new int[] { 75, 76, 77 },
            new int[] { 78, 79, 80 },
            new int[] { 81, 82, 83 },
            new int[] { 84, 85, 86 },
            new int[] { 87, 88, 89 }
        }
    };

    // playerLineIDToEnemyLineID[Player Line ID] => Enemy Line ID
    public Dictionary<int, int> playerLineIDToEnemyLineID = new Dictionary<int, int>
    {
        // Wash Depp
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
        { 18, 7 },
        { 19, 7 },
        { 20, 7 },
        { 21, 8 },
        { 22, 8 },
        { 23, 8 },
        // We never reach this point
        { 24, 2 },
        { 25, 2 },
        { 26, 2 },
        // { 27, 1 },
        // { 28, 2 },
        // { 29, 5 },

        // Kendrick Amore
        { 30, 1 },
        { 31, 1 },
        { 32, 1 },
        { 33, 2 },
        { 34, 2 },
        { 35, 2 },
        { 36, 3 },
        { 37, 3 },
        { 38, 3 },
        { 39, 4 },
        { 40, 4 },
        { 41, 4 },
        { 42, 5 },
        { 43, 5 },
        { 44, 5 },
        { 45, 6 },
        { 46, 6 },
        { 47, 6 },
        { 48, 7 },
        { 49, 7 },
        { 50, 7 },
        { 51, 8 },
        { 52, 8 },
        { 53, 8 },
        // We never reach this point
        { 54, 2 },
        { 55, 2 },
        { 56, 2 },
        // { 57, 1 },
        // { 58, 2 },
        // { 59, 5 },

        // Lil Pay
        { 60, 1 },
        { 61, 1 },
        { 62, 1 },
        { 63, 2 },
        { 64, 2 },
        { 65, 2 },
        { 66, 3 },
        { 67, 3 },
        { 68, 3 },
        { 69, 4 },
        { 70, 4 },
        { 71, 4 },
        { 72, 5 },
        { 73, 5 },
        { 74, 5 },
        { 75, 6 },
        { 76, 6 },
        { 77, 6 },
        { 78, 7 },
        { 79, 7 },
        { 80, 7 },
        { 81, 8 },
        { 82, 8 },
        { 83, 8 },
        // We never reach this point
        { 84, 2 },
        { 85, 2 },
        { 86, 2 },
        { 87, 3 },
        { 88, 3 },
        { 89, 3 }
    };

    // playerLines[Player Line ID] => player line as a string
    private Dictionary<int, string> playerLines = new Dictionary<int, string>
    {
        // Wash Depp
        { 0, "You’re an old van,\nbut I’m a hot rod/If you wanna win this battle,\nyou better pray to your god"},
        { 1, "You’re an old van,\nbut I’m a hot rod/I’ve killed with my raps\nmore than Sweeney Todd"},
        { 2, "You’re an old van,\nbut I’m a hot rod/I drive really fast,\nso YOU don’t catch up"},
        { 3, "You’re an outdated book,\nan expired piece of meat/Too old to recognize\nwhen you’re obsolete"},
        { 4, "You’re an outdated book,\nan expired piece of meat/Rap battling you\nis no amazing feat"},
        { 5, "You’re an outdated book,\nan expired piece of meat/Nobody wants you,\nyou have stinky feet"},
        { 6, "So knock-off Nick Carter\nwants to pretend like he’s a rapper/You wanna be Elvis so bad? You can kick it\non your golden crapper"},
        { 7, "So knock-off Nick Carter\nwants to pretend like he’s a rapper/You’re so old,\nI bet you once were a flapper"},
        { 8, "So knock-off Nick Carter\nwants to pretend like he’s a rapper/Well, I love Backstreet Boys,\ncan I have your autograph?"},
        { 9, "I’m surprised you’re even here\nafter years of being dusted/This rap battle’s already over\n'cuz we all know your rhymes are busted"},
        { 10, "I’m surprised you’re even here\nafter years of being dusted/You’re tired and old,\nI doubt you’re well-adjusted"},
        { 11, "I’m surprised you’re even here\nafter years of being dusted/Look at all those dust mites,\newww, gross"},
        { 12, "You know what they say,\ncan’t teach an old dog new tricks/And that clearly applies\nsince you’re a whiny lil’ bitch"},
        { 13, "You know what they say,\ncan’t teach an old dog new tricks/I’ll hurt you with words,\nnot stones and sticks"},
        { 14, "You know what they say,\ncan’t teach an old dog new tricks/That’s why I don’t have a dog,\nthey’re hard to take care of"},
        { 15, "You say you’re immortal,\nbut that makes me a god/Just watch, as I say this,\nthe crowds will applaud"},
        { 16, "You say you’re immortal,\nbut that makes me a god/I’m the leader of youth,\nyou’re a noisy tightwad"},
        { 17, "You say you’re immortal,\nbut that makes me a god/Now get me some grapes,\nand send me your prayers"},
        { 18, "Stop talking about yourself,\nwe get that you’re desperate/Your life’s been spiraling\nsince you got arrested"},
        { 19, "Stop talking about yourself,\nwe get that you’re desperate/Now sit down, old-timer,\nand know when you’re bested"},
        { 20, "Stop talking about yourself,\nwe get that you’re desperate/Your time has passed,\nnow lay down and cry"},
        { 21, "Bragging about your pipes\nas if you’re Neil Young/The only thing you and Bob Dylan share\nis smoker’s lung"},
        { 22, "Bragging about your pipes\nas if you’re Neil Young/When was the last time\nyou actually sung?"},
        { 23, "Bragging about your pipes\nas if you’re Neil Young/Guess emphysema was the hot thing\nin your day huh"},
        { 24, "You talk like you’re still hip\nand have style to spare/You’re just an old man\nscreaming from his rocking chair"},
        { 25, "You talk like you’re still hip\nand have style to spare/You wanna make a comeback,\nbut no one would care"},
        { 26, "You talk like you’re still hip\nand have style to spare/ But it’s hard to be stylish\nwhen you threw out your back"},
        // unused
        // { 27, ""},
        // { 28, ""},
        // { 29, ""},

        // Kendrick Amore
        { 30, "Pretty face won’t do you good\nwhen there’s nothin behind’er/Leave the rap battles to me,\nget your ass back to Grindr"},
        { 31, "Pretty face won’t do you good\nwhen there’s nothin behind’er/Is your robot girlfriend running?\n‘Cuz you better go find her"},
        { 32, "Pretty face won’t do you good\nwhen there’s nothin behind’er/Your skull is just empty,\nand you are stupid"},
        { 33, "I don’t even care\nif you wanna go after my bae/Least my fans won’t leave\nafter a fuckin' bad hair day"},
        { 34, "I don’t even care\nif you wanna go after my bae/DM’s are so distant,\nthey’re 6 feet away"},
        { 35, "I don’t even care\nif you wanna go after my bae/I don’t even want her,\nshe’s lowkey kind of crazy"},
        { 36, "Pardon my French but I bring\nrevolution and bloodshed/Your council will soon know\nthat I look best in dark red"},
        { 37, "Pardon my French but I bring\nrevolution and bloodshed/you won’t be pretty\nwhen you’re super duper dead"},
        { 38, "Pardon my French but I bring\nrevolution and bloodshed/also you’re actually so gross,\nnah nah nah nah nah"},
        { 39, "Then that’s seven years of bad luck,\nis that right?/At least it’s not 25 years\nlike your life"},
        { 40, "Then that’s seven years of bad luck,\nis that right?/Superstitions don’t mean nothin’,\nI’m always outta sight"},
        { 41, "Then that’s seven years of bad luck,\nis that right?/I better go hide until then,\nsee you later"},
        { 42, "I don’t need arms to beat you,\nI can just use my words/Spittin’ rhymes so fast\nI’m like a fuckin' Firebird"},
        { 43, "I don’t need arms to beat you,\nI can just use my words/You think you’re charming?\nDon’t be absurd"},
        { 44, "I don’t need arms to beat you,\nI can just use my words/Bird bird bird,\nbird is the word"},
        { 45, "I’m cool as a cucumber,\nalways asked for/And soon you’ll be the one\ncryin’ out for more"},
        { 46, "I’m cool as a cucumber,\nalways asked for/You think you’ve got bitches\nbut I’m the one they adore"},
        { 47, "I’m cool as a cucumber,\nalways asked for/And then I answer “hell yeah”\ncuz I’m just a good sport"},
        { 48, "Bro you put on so much hairspray\nit got to your brain/That explains why everything\nyou say sounds so insane"},
        { 49, "Bro you put on so much hairspray\nit got to your brain/Can’t take you seriously\nwith that ridiculous mane"},
        { 50, "Bro you put on so much hairspray\nit got to your brain/Your hair is so fake\nyour parents must be ashamed"},
        { 51, "You talk so much game,\nlike you’re some kind of pimp/They’re only there for your money\n‘cause they know you’re a simp"},
        { 52, "You talk so much game,\nlike you’re some kind of pimp/But you never get girls\ncause they know you’re a wimp"},
        { 53, "You talk so much game,\nlike you’re some kind of pimp/That kind of stuff is\nreally going out on a limb"},
        { 54, "At least I don’t look\nlike an Andy Warhol clone/You only worship yourself\ncause you’re high on cologne"},
        { 55, "At least I don’t look\nlike an Andy Warhol clone/You’re too in love with yourself\nto claim the rap throne"},
        { 56, "At least I don’t look\nlike an Andy Warhol clone/I’m Leonardo Da Vinci,\nI make rap milestones"},
        // unused
        // { 57, ""},
        // { 58, ""},
        // { 59, ""},

         // Lil Pay
        { 60, "I don't wanna fight a toddler\nbut for now I’ll bend that rule/Wanna pray to a rap god kid?\nI’ll kick your ass to Sunday school" },
        { 61, "I don't wanna fight a toddler\nbut for now I’ll bend that rule/I’m gonna trounce you\nsince I’m so cool" },
        { 62, "I don't wanna fight a toddler\nbut for now I’ll bend that rule/haha you suck,\nand that’s the truth" },
        { 63, "You're talkin\' like a soldier\nbut ain’t old enough to draft/Kid you wanna be on COD\nbut you're still stuck on Minecraft" },
        { 64, "You're talkin\' like a soldier\nbut ain’t old enough to draft/your rap game just died,\npress F in chat" },
        { 65, "You're talkin\' like a soldier\nbut ain’t old enough to draft/if you did you’d,\nuhh, just eat crayons" },
        { 66, "Your followers don't mean shit\nwhen we know that they’re all bought/Now just drop the mic\nand run back to your job at Kidz Bop"},
        { 67, "Your followers don't mean shit\nwhen we know that they’re all bought/so basically all your bots\nfollow a rock" },
        { 68, "Your followers don't mean shit\nwhen we know that they’re all bought/and your stage presence\nreally really sucks" },
        { 69, "Guess you’re still young enough\nto be throwing tantrums/Time to tell your mom\nto stop buying out your albums" },
        { 70, "Guess you’re still young enough\nto be throwing tantrums/when your parents say \"I love you\"\nit’s definitely sarcasm" },
        { 71, "Guess you’re still young enough\nto be throwing tantrums/I’ll stick you in my van\nand then I’ll laugh, haha" },
        { 72, "Nice try kid,\nbut you can't cover up that stink/And like the titanic,\nyour popularity will sink" },
        { 73, "Nice try kid,\nbut you can't cover up that stink/It makes me squint so hard\nit looks like I wink " },
        { 74, "Nice try kid,\nbut you can't cover up that stink/You are a baby,\nwho went a poopy" },
        { 75, "Hate to break it to you, kid,\nbut there's more to the world/Than just your favorite Peppa Pig,\nSpongebob, and Big Bird" },
        { 76, "Hate to break it to you, kid,\nbut there's more to the world/Than dancing,\nand how to do a twirl" },
        { 77, "Hate to break it to you, kid,\nbut there's more to the world/Than you,\ncuz you’re so small" },
        { 78, "Funny, your mom’s spaghetti\nis extremely stiff/When I came over last night,\nI had a whiff" },
        { 79, "Funny, your mom’s spaghetti\nis extremely stiff/I’m gonna rap so hard\nyou’ll go jump off a cliff" },
        { 80, "Funny, your mom’s spaghetti\nis extremely stiff/My mom’s is better,\nyou should try it" },
        { 81, "Oh you think you’re hot\n‘cause you can lipsync?/You definitely don’t sing\ncause your lyrics stink" },
        { 82, "Oh you think you’re hot\n‘cause you can lipsync?/You sound like you’re choking\non a bad drink" },
        { 83, "Oh you think you’re hot\n‘cause you can lipsync?/Well, so can I,\nso take that" },
        { 84, "You got the face of a baby\nand the brain of one too/You’re just babbling and whining\ncause you’re getting a boo-boo" },
        { 85, "You got the face of a baby\nand the brain of one too/Just crawlin’ around\nlike you don’t know what to do" },
        { 86, "You got the face of a baby\nand the brain of one too/Goo goo gah gah,\ngo cry to your mama" },
        { 87, "Oh it’s so cute\nyou think you have game/I’ll give you “the talk”\nafter I put you to shame" },
        { 88, "Oh it’s so cute\nyou think you have game/you got bullies out there\ncallin’ your name" },
        { 89, "Oh it’s so cute\nyou think you have game/Keep this PG,\nthis is a Christian show" },
    };

    private Dictionary<int, string> choiceLines = new Dictionary<int, string> {

        // Wash Depp
        { 0, "[Pray to your god]"},
        { 1, "[Sweeney Todd]"},
        { 2, "[YOU don’t catch up]"},
        { 3, "[You’re obsolete]"},
        { 4, "[No amazing feat]"},
        { 5, "[Stinky feet]"},
        { 6, "[Golden crapper]"},
        { 7, "[Were a flapper]"},
        { 8, "[Have your autograph]"},
        { 9, "[Rhymes are busted]"},
        { 10, "[Well-adjusted]"},
        { 11, "[Eww, gross]"},
        { 12, "[Whiny lil’ bitch]"},
        { 13, "[Stones and sticks]"},
        { 14, "[Hard to take care of]"},
        { 15, "[Crowds will applaud]"},
        { 16, "[Noisy tightwad]"},
        { 17, "[Your prayers]"},
        { 18, "[You got arrested]"},
        { 19, "[Know when you’re bested]"},
        { 20, "[Lay down and cry]"},
        { 21, "[Smoker’s lung]"},
        { 22, "[Last time you sung]"},
        { 23, "[Emphysema]"},
        { 24, "[Rocking chair]"},
        { 25, "[No one cares]"},
        { 26, "[Your back]"},
        // Unused
        // { 27, "[]"},
        // { 28, "[]"},
        // { 29, "[]"},

        // Kendrick Amore
        { 30, "[Back to Grindr]"},
        { 31, "[Go find her]"},
        { 32, "[You are stupid]"},
        { 33, "[Bad hair day]"},
        { 34, "[6 feet away]"},
        { 35, "[Kind of crazy]"},
        { 36, "[Best in dark red]"},
        { 37, "[Super duper dead]"},
        { 38, "[nah nah nah]"},
        { 39, "[25 years like your life]"},
        { 40, "[Always outta sight]"},
        { 41, "[See you later]"},
        { 42, "[Fuckin' Firebird]"},
        { 43, "[Don’t be absurd]"},
        { 44, "[Bird is the word]"},
        { 45, "[Cryin’ out for more]"},
        { 46, "[One they adore]"},
        { 47, "[A good sport]"},
        { 48, "[You sound insane]"},
        { 49, "[Ridiculous mane]"},
        { 50, "[Parents must be ashamed]"},
        { 51, "[You’re a simp]"},
        { 52, "[You’re a wimp]"},
        { 53, "[Going out on a limb]"},
        { 54, "[You’re high on cologne]"},
        { 55, "[Claim the rap throne]"},
        { 56, "[Rap milestones]"},
        // Unused
        // { 57, "[]"},
        // { 58, "[]"},
        // { 59, "[]"},

        // Lil Pay
        { 60, "[Kick your ass to Sunday School]" },
        { 61, "[I’m so cool]" },
        { 62, "[You suck]" },
        { 63, "[You’re stuck on Minecraft]" },
        { 64, "[Press F in chat]" },
        { 65, "[Eat crayons]" },
        { 66, "[Your job at Kidz Bop]"},
        { 67, "[Your bots follow a rock]" },
        { 68, "[really really suck]" },
        { 69, "[Mom buys out your albums]" },
        { 70, "[It’s definitely sarcasm]" },
        { 71, "[I laugh, haha]" },
        { 72, "[Popularity will sink]" },
        { 73, "[Looks like I wink]" },
        { 74, "[You went a poopy]" },
        { 75, "[Your favorite Big Bird]" },
        { 76, "[Do a twirl]" },
        { 77, "[You’re so small]" },
        { 78, "[I had a whiff]"},
        { 79, "[Jump off a cliff]"},
        { 80, "[You should try it]"},
        { 81, "[‘Cause your lyrics stink]"},
        { 82, "[Choking on a bad drink]"},
        { 83, "[So take that]"},
        { 84, "[You’re getting a boo-boo]"},
        { 85, "[Don’t know what to do]"},
        { 86, "[Go cry to mama]"},
        { 87, "[I put you to shame]"},
        { 88, "[Out there callin’ your name]"},
        { 89, "[Christian show]"}
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
        new string[] { // Wash Depp
            "You think you’re better than the veteran,\nbut you ain’t matching up/You’re sputterin’ while I’m gunnin’ it,\ndon’t even bother catching up",
            "My band sung about love,\nbut I know how to fight/Tryin’ to stand up to me? Bitch,\nyou don’t have the right",
            "I’m the modern Elvis Presley,\nwhen it comes to rap I'm the king./Wanna battle against me, kid?\nYou can choke on my middle ring.",
            "I can tell you're getting worried\ncuz engine’s had a stall/I was in this fuckin' race\nbefore you knew how to crawl",
            "Think newer is better?\nWell, talent never fades/And getting this good\ncan only come with age",
            "Try to catch up,\nbut you’re always in my shadow/Legends never die,\nI’m immortal, I won this battle",
            "The people don’t want you,\nyou just want a fan/You’ll get chewed up, kid,\nand spat out again",
            "You don’t get a voice like mine\nby singing showtunes/I was hanging with Dylan\nwhile you were in the womb",
            "Alright kid, I’m your elder,\nlet me be your teacher/you’re just a wannabe,\nI’m still the main feature"
        },
        new string[] { // Kendrick Amore
            "Hey there handsome, get your hands up\ncuz it’s time for your surrender/I’m the OG ladies man\nand you're a little pretender",
            "Open up your girl’s DMs\nyou’ll find 'em full of this MC/See you’re getting heated\ncuz your stan’s all hyung up on me",
            "Yeah, I’m the king\nof the pretty boy committee/you’re so lowly,\nit’s a shame and a pity",
            "You better watch yourself -\noh wait, nevermind/’cuz you so damn ugly,\nyou breakin’ mirrors all the time",
            "I got looks so good,\nthey’re known to disarm/so don’t beat yourself up\nwhen you’re drawn to my charm",
            "I’m so damn hot\nwhen I’m walking down the street/bitches be comin’ up to me\nlike they in heat",
            "You know, this profile\ntook 2 glorious hours/Meanwhile you don’t even\nlook like you took a shower",
            "I’m the guy she tells you\nnot to worry about/Cause when they see me,\nshe and all the other girls shout",
            "Bask in my presence,\nI’m oozing with style/Looks like you pulled those\nclothes out of a trash pile"
        },
        new string[] { // Lil Pay
            "Welcome to the final\nnow it’s time for Lil’ Pay/When I walk up on the stage\ny’all know it’s time to lil pray", 
            "Bitch I’m 9 years old\n 'n I’m already flexin millions/Lil Pay’s on Call of Duty\nwhile your ass is a civilian", 
            "Ok Boomer you're just mad\ncuz I’m already verified/Lil Pay’s an asteroid,\ncall this stage a genocide", 
            "You’re just a Millennial,\ndon’t try to mess with Gen Z/I already reached platinum\nwith just a single LP", 
            "They call me Lil' Pay\nbut I ain't so small/ I'm king of the world,\nyou're just a fly on the wall",
            "Nobody understands\nyour obscure 90's references/Keep it to yourself, we don't care\n'bout your preferences",
            "I’m in it, legit\nbars too hype to bite/I’m winnin’,\nmom’s spaghetti-n’ your appetite", 
            "I might look small\nbut I’m big on TikTok/My rhymes hit you\nlike an electric shock",
            "Respect your elders?\nWhy even try?/I got a baby face\nbut you’re gonna be the one to cry",
            "Listen man,\nI’m way more endowed than you/In cash, in swag,\nand in, heh, other ways too"
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private string name;
    private string rank;
    //TODO: change to enum?
    private int ID;
    private int turns;
    //TODO: somehow add insults already used?
    private bool defeated;
    private bool setID = false;
    private Sprite sprite;
    private AudioClip intro;
    private bool banana;

    public Opponent(string name, bool defeated, int turns, Sprite s, AudioClip a) {
        this.name = name;
        this.defeated = defeated;
        this.turns = turns;
        this.sprite = s;
        this.intro = a;
        this.rank = "";
        this.banana = false;
    }

    public void Defeat() {
        defeated = true;
    }

    public void Banana()
    {
        banana = true;
    }

    public bool GetDefeated() {
        return defeated;
    }

    public bool GetBanana()
    {
        return banana;
    }

    public string GetName() {
        return name;
    }

    public int GetTurns() {
        return turns;
    }

    public Sprite GetSprite() {
        return sprite;
    }

    public AudioClip GetIntro() {
        return intro;
    }

    public void SetID(int id) {
        if (setID) return; // Can only set ID once
        this.ID = id;
        setID = true;
    }

    public int GetID() {
        return this.ID;
    }

    public void SetRank(string rank) {
        this.rank = rank;
    }

    public string GetRank() {
        if (rank == "") return "Unknown";
        return rank;
    }
}

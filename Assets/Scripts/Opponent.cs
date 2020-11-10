﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private string name;
    //TODO: change to enum?
    private int ID;
    private int turns;
    //TODO: somehow add insults already used?
    private bool defeated;
    private bool setID = false;
    private Sprite sprite;
    private AudioClip intro;

    public Opponent(string name, bool defeated, int turns, Sprite s, AudioClip a) {
        this.name = name;
        this.defeated = defeated;
        this.turns = turns;
        this.sprite = s;
        this.intro = a;
    }

    public void Defeat() {
        defeated = true;
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
}

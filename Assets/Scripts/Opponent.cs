using System.Collections;
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

    public Opponent(string name, bool defeated, int turns) {
        this.name = name;
        this.defeated = defeated;
        this.turns = turns;
    }

    public void Defeat() {
        defeated = true;
    }

    public string GetName() {
        return name;
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

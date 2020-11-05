using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    new public string name;
    //TODO: change to enum?
    public int ID;
    public int turns;
    //TODO: insults already used
    public bool defeated;

    public Opponent(string name, bool defeated, int turns) {
        this.name = name;
        this.defeated = defeated;
        this.turns = turns;
    }
}

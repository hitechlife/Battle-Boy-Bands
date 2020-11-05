using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    new public string name;
    //TODO: change to enum?
    public int difficulty;
    //TODO: insults already used
    public bool defeated;

    public Opponent(string name, bool defeated) {
        this.name = name;
        this.defeated = defeated;
    }
}

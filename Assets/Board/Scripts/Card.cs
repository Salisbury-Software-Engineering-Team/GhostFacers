using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Card
{

    // needs changing
    public string Name;
    public string Stats;

    public Card(string N, string S) { Name = N; Stats = S; }
}

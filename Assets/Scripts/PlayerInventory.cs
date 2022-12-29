using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class PlayerInventory
{
    public static List<Design> Deck = new List<Design>();

    public static List<string> Items { get; set; } = new List<string>();

    static PlayerInventory()
    {
        Deck.Add(new Swordsman());
        Deck.Add(new Slinger());
        Deck.Add(new Impede());
        
        Items.Add("ReDress");
    }
}

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class PlayerInventory
{
    private static List<Design> _deck = new List<Design>();

    public static List<Design> Deck
    {
        get => _deck;
        set => _deck = value;
    }

    public static List<string> Items { get; set; } = new List<string>();

    static PlayerInventory()
    {
        _deck.Add(new Swordsman());
        _deck.Add(new Slinger());
        _deck.Add(new Impede());
    }
}

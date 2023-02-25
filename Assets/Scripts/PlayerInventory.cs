using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerInventory
{
    public List<Design> Deck = new List<Design>();

    public List<string> Items { get; set; } = new List<string>();
    
    public int MaxPlanks { get; set; }
    public int Lives { get; set; }
    public int MaxLives { get; set; }
    public int Gold { get; set; }
    public int MovesPerTurn { get; set; }
    public int NumberOfTurns { get; set; }
    public int AmountOfCardsToOffer { get; set; }
    public int AmountOfItemsToOffer { get; set; }

    public PlayerInventory()
    {
        Deck.Add(new GrandFinalist());
        Deck.Add(new GrandFinalist());
        Deck.Add(new LoneWolf());
        
        MaxPlanks = 3;
        Lives = 3;
        Gold = 0;
        MovesPerTurn = 1;
        NumberOfTurns = 6;
        AmountOfCardsToOffer = 4;
        AmountOfItemsToOffer = 3;
    }

    public void SaveDeck(IEnumerable<Design> newDeck)
    {
        Deck = newDeck.ToList();
        
        // Cut off deck if it's too big
        if (Deck.Count >= RunProgress.PlayerInventory.MaxPlanks) 
            Deck = Deck.GetRange(0, RunProgress.PlayerInventory.MaxPlanks);
    }
}

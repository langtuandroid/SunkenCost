using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Challenges;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerInventory
{
    public List<Design> Deck { get; private set; }= new List<Design>();

    public List<string> Items { get; private set; } = new List<string>();
    
    public int MaxPlanks { get; private set; }
    public int Lives { get; set; }
    public int MaxLives { get; private set; }
    public int Gold { get; private set; }
    public int MovesPerTurn { get; private set; }
    public int NumberOfTurns { get; private set; }
    public int AmountOfCardsToOffer { get; private set; }
    public int AmountOfItemsToOffer { get; private set; }
    
    public int AmountOfChallengesToOffer { get; private set; }

    public PlayerInventory()
    {
        Deck.Add(new Swordsman());
        Deck.Add(new Slinger());
        Deck.Add(new Impede());
        
        MaxPlanks = 3;
        MaxLives = 3;
        Gold = 2;
        MovesPerTurn = 1;
        NumberOfTurns = 2;
        AmountOfCardsToOffer = 2;
        AmountOfItemsToOffer = 3;
        AmountOfChallengesToOffer = 2;
        
        Lives = MaxLives;
    }

    public void SaveDeck(IEnumerable<Design> newDeck)
    {
        Deck = newDeck.ToList();
        
        // Cut off deck if it's too big
        if (Deck.Count >= RunProgress.PlayerInventory.MaxPlanks) 
            Deck = Deck.GetRange(0, RunProgress.PlayerInventory.MaxPlanks);
    }
    
    public void AlterGold(int amount)
    {
        Gold += amount;
    }

    public void AcceptChallengeReward(ChallengeRewardType challengeRewardType)
    {
        switch (challengeRewardType)
        {
            case ChallengeRewardType.None:
                break;
            case ChallengeRewardType.Plank:
                MaxPlanks++;
                break;
            case ChallengeRewardType.Move:
                MovesPerTurn++;
                break;
            case ChallengeRewardType.DesignOffer:
                break;
            case ChallengeRewardType.ItemOffer:
                break;
            case ChallengeRewardType.ChallengeOffer:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

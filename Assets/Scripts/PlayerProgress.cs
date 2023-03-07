using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Challenges;
using JetBrains.Annotations;
using OfferScreen;
using UnityEngine;

public class PlayerProgress
{
    private const int INIT_MAX_PLANKS = 3;
    private const int INIT_MAX_LIVES = 3;
    private const int INIT_GOLD = 5;
    private const int INIT_MOVES_PER_TURN = 1;
    private const int INIT_NUM_OF_TURNS = 6;
    
    private const int INIT_NUM_OF_CARD_OFFERS = 2;
    private const int INIT_NUM_OF_ITEM_OFFERS = 2;
    private const int INIT_NUM_OF_CHALLENGE_OFFERS = 2;
    
    public List<Design> Deck { get; private set; }= new List<Design>();

    public List<string> Items { get; private set; } = new List<string>();

    public readonly PriceHandler PriceHandler = new PriceHandler();

    public int MaxPlanks { get; private set; }
    public int Lives { get; set; }
    public int MaxLives { get; private set; }
    public int Gold { get; private set; }
    public int MovesPerTurn { get; private set; }
    public int NumberOfTurns { get; private set; }
    public int NumberOfCardsToOffer { get; private set; }
    public int NumberOfItemsToOffer { get; private set; }
    
    public int NumberOfChallengesToOffer { get; private set; }

    public PlayerProgress()
    {
        MaxPlanks = INIT_MAX_PLANKS;
        MaxLives = INIT_MAX_LIVES;
        Gold = INIT_GOLD;
        MovesPerTurn = INIT_MOVES_PER_TURN;
        NumberOfTurns = INIT_NUM_OF_TURNS;
        NumberOfCardsToOffer = INIT_NUM_OF_CARD_OFFERS;
        NumberOfItemsToOffer = INIT_NUM_OF_ITEM_OFFERS;
        NumberOfChallengesToOffer = INIT_NUM_OF_CHALLENGE_OFFERS;
        
        Lives = MaxLives;
    }

    public void InitialiseDeck()
    {
        Deck.Add(new Swordsman());
        Deck.Add(new Slinger());
        Deck.Add(new Impede());
    }

    public void SaveDeck(IEnumerable<Design> newDeck)
    {
        Deck = newDeck.ToList();
        
        // Cut off deck if it's too big
        if (Deck.Count >= RunProgress.PlayerProgress.MaxPlanks) 
            Deck = Deck.GetRange(0, RunProgress.PlayerProgress.MaxPlanks);
    }
    
    public void AlterGold(int amount)
    {
        Gold += amount;
    }

    public void AcceptChallengeReward(ChallengeRewardType challengeRewardType)
    {
        switch (challengeRewardType)
        {
            case ChallengeRewardType.Plank:
                MaxPlanks++;
                break;
            case ChallengeRewardType.Move:
                MovesPerTurn++;
                break;
            case ChallengeRewardType.DesignOffer:
                NumberOfCardsToOffer++;
                break;
            case ChallengeRewardType.ItemOffer:
                NumberOfItemsToOffer++;
                break;
            case ChallengeRewardType.ChallengeOffer:
                NumberOfChallengesToOffer++;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

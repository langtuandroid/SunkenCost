using System.Collections.Generic;
using System.Linq;
using Designs;
using OfferScreen;

public class PlayerStats
{
    private const int INIT_MAX_PLANKS = 3;
    private const int INIT_MAX_HEALTH = 30;
    private const int INIT_GOLD = 5;
    private const int INIT_MOVES_PER_TURN = 10;
    private const int INIT_NUM_OF_TURNS = 5;
    
    private const int INIT_NUM_OF_CARD_OFFERS = 3;
    private const int INIT_NUM_OF_ITEM_OFFERS = 1;
    private const int INIT_NUM_OF_CHALLENGE_OFFERS = 2;
    
    public List<Design> Deck { get; private set; }= new List<Design>();

    public readonly PriceHandler PriceHandler = new PriceHandler();

    public readonly Stat maxPlanks;

    public int MaxPlanks => maxPlanks.Value;

    public int PlanksBought { get; private set; } = 0;
    public int MovesBought { get; private set; } = 0;
    public int Health { get; set; }
    public int MaxHealth { get; private set; }
    public int Gold { get; set; }
    public int MovesPerTurn { get; set; }
    public int NumberOfTurns { get; private set; }
    public int NumberOfCardsToOffer { get; private set; }
    public int NumberOfItemsToOffer { get; private set; }
    
    public int NumberOfChallengesToOffer { get; private set; }

    public PlayerStats()
    {
        maxPlanks = new Stat(INIT_MAX_PLANKS);
        MaxHealth = INIT_MAX_HEALTH;
        Gold = INIT_GOLD;
        MovesPerTurn = INIT_MOVES_PER_TURN;
        NumberOfTurns = INIT_NUM_OF_TURNS;
        NumberOfCardsToOffer = INIT_NUM_OF_CARD_OFFERS;
        NumberOfItemsToOffer = INIT_NUM_OF_ITEM_OFFERS;
        NumberOfChallengesToOffer = INIT_NUM_OF_CHALLENGE_OFFERS;
        
        Health = MaxHealth;
    }

    public void InitialiseDeck(params string[] initialDeck)
    {
        foreach (var name in initialDeck)
        {
            Deck.Add(DesignFactory.InstantiateDesignFromString(name));
        }

        foreach (var design in Deck)
        {
            design.SetCost(3);
        }
    }

    public void SaveDeck(IEnumerable<Design> newDeck)
    {
        Deck = newDeck.ToList();
        
        // Cut off deck if it's too big
        if (Deck.Count >= RunProgress.PlayerStats.MaxPlanks) 
            Deck = Deck.GetRange(0, RunProgress.PlayerStats.MaxPlanks);
    }

    public void BuyPlank()
    {
        PlanksBought++;
        maxPlanks.AddModifier(new StatModifier(1, StatModType.Flat));
    }

    public void BuyMove()
    {
        MovesBought++;
        MovesPerTurn++;
    }
}

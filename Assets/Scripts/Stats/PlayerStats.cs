using System.Collections.Generic;
using System.Linq;
using Designs;
using OfferScreen;

public class PlayerStats
{
    public List<Design> Deck { get; private set; }

    public readonly PriceHandler PriceHandler = new PriceHandler();

    public readonly Stat maxPlanks;

    public int MaxPlanks => maxPlanks.Value;

    public int PlanksBought { get; private set; } = 0;
    public int MovesBought { get; private set; } = 0;
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Gold { get; set; }
    public int? MovesPerTurn { get; set; }
    public int NumberOfTurns { get; private set; }
    public int DesignOffersPerBattle { get; private set; }
    public int ItemOffersPerBattle { get; private set; }
    public int RewardOffersPerBattle { get; set; }
    public int EnemyMovementModifier { get; set; } = 0;

    public int ReRollCost { get; private set; }

    public PlayerStats(RunthroughStartingConfig config)
    {
        maxPlanks = new Stat(config.MaxPlanks);
        MaxHealth = config.MaxHealth;
        Gold = config.StartingGold;
        MovesPerTurn = config.MovesPerTurn;
        NumberOfTurns = config.TurnsPerBattle;
        DesignOffersPerBattle = config.DesignOffersPerBattle;
        ItemOffersPerBattle = config.ItemOffersPerBattle;
        RewardOffersPerBattle = config.RewardOffersPerBattle;
        ReRollCost = config.ReRollCost;

        Health = MaxHealth;
    }

    public void InitialiseDeck(List<Design> startingDeck)
    {
        Deck = startingDeck;
        SetDeckCostToAmount(0);
    }

    public void SaveDeck(IEnumerable<Design> newDeck)
    {
        Deck = newDeck.ToList();
        
        // Cut off deck if it's too big
        if (Deck.Count >= RunProgress.Current.PlayerStats.MaxPlanks) 
            Deck = Deck.GetRange(0, RunProgress.Current.PlayerStats.MaxPlanks);
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

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth) Health = MaxHealth;
    }

    public void SetDeckCostToAmount(int amount)
    {
        foreach (var design in Deck)
        {
            design.SetCost(amount);
        }
    }
}

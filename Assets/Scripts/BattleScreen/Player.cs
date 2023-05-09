using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;

public class Player : BattleEventResponder
{
    public static Player Current;

    private int _baseMovesPerTurn;
    
    public int Gold { get; private set; }

    public int Health { get; private set; }
    private int MaxHealth { get; set; }
    public int MovesUsedThisTurn { get; private set; } = 0;
    public int MovesPerTurn { get; set; }

    public bool HasMovesLimit => MovesPerTurn > -1;
    public bool IsOutOfMoves => MovesUsedThisTurn >= MovesPerTurn;

    protected override void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;

        Gold = RunProgress.PlayerStats.Gold;
        Health = RunProgress.PlayerStats.Health;
        MaxHealth = RunProgress.PlayerStats.MaxHealth;
        _baseMovesPerTurn = RunProgress.PlayerStats.MovesPerTurn;
        MovesPerTurn = _baseMovesPerTurn;
        base.Awake();
    }

    private void ResetMoves()
    {
        MovesUsedThisTurn = 0;
    }

    private BattleEvent EnemyReachedEnd(Enemy enemy)
    {
        return new BattleEvent(BattleEventType.PlayerLifeModified) 
            {modifier = -enemy.Health, source = DamageSource.Boat};
    }

    private BattleEvent ModifyLife(BattleEvent battleEvent)
    {
        var previousHealth = Health;
        Health += battleEvent.modifier;

        if (Health <= 0)
            return new BattleEvent(BattleEventType.PlayerDied);
        
        if (Health < previousHealth)
            return new BattleEvent(BattleEventType.PlayerLostLife);
        
        if (Health > MaxHealth)
            Health = MaxHealth;
        
        if (Health > previousHealth)
            return new BattleEvent(BattleEventType.PlayerGainedLife);
        
        return BattleEvent.None;
    }
    
    private BattleEvent UsedMove()
    {
        if (HasMovesLimit) MovesUsedThisTurn += 1;
        return new BattleEvent(BattleEventType.PlayerUsedMove);
    }

    private BattleEvent TryGainGold(BattleEvent battleEvent)
    {
        Gold += battleEvent.modifier;
        return new BattleEvent(BattleEventType.GainedGold);
    }

    public override BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent)
    {
        if (previousBattleEvent.type == BattleEventType.StartNextPlayerTurn)
        {
            ResetMoves();
            return BattleEventPackage.Empty;
        }
        
        var battleEvent = previousBattleEvent.type switch
        {
            BattleEventType.PlayerMovedPlank => UsedMove(),
            BattleEventType.EnemyReachedBoat => 
                EnemyReachedEnd(BattleEventsManager.Current.GetEnemyByResponderID(previousBattleEvent.affectedResponderID)),
            BattleEventType.PlayerLifeModified => ModifyLife(previousBattleEvent),
            BattleEventType.TryGainedGold => TryGainGold(previousBattleEvent),
            _ => BattleEvent.None
        };
        
        if (battleEvent.type == BattleEventType.None) return BattleEventPackage.Empty;

        return previousBattleEvent.type == BattleEventType.PlayerMovedPlank 
            ? new BattleEventPackage(battleEvent, new BattleEvent(BattleEventType.PlankMoved))
            : new BattleEventPackage(battleEvent);
    }
}

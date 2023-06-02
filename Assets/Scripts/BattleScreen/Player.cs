using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;

public class Player : BattleEventHandler
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

        Gold = RunProgress.Current.PlayerStats.Gold;
        Health = RunProgress.Current.PlayerStats.Health;
        MaxHealth = RunProgress.Current.PlayerStats.MaxHealth;
        _baseMovesPerTurn = RunProgress.Current.PlayerStats.MovesPerTurn;
        MovesPerTurn = _baseMovesPerTurn;
        base.Awake();
    }

    public override List<BattleEventResponseTrigger> GetBattleEventResponseTriggers()
    {
        return new List<BattleEventResponseTrigger>
        {
            AddResponseTrigger(BattleEventType.PlayerLostLife, 
                e => new BattleEvent(BattleEventType.PlayerDied),
                e => Health <= 0),
            AddActionTrigger(BattleEventType.PlayerLostLife, ResetMoves),
            AddResponseTrigger(BattleEventType.PlayerMovedPlank, 
                e => PlayerMovedPlank()),
            AddResponseTrigger(BattleEventType.EnemyAttackedBoat, 
                e => EnemyReachedEnd(-e.modifier)),
            AddResponseTrigger(BattleEventType.PlayerLifeModified, ModifyLife),
            AddResponseTrigger(BattleEventType.GainedGold, TryGainGold),
        };
    }

    private void ResetMoves()
    {
        MovesUsedThisTurn = 0;
    }

    private BattleEvent EnemyReachedEnd(int damage)
    {
        return new BattleEvent(BattleEventType.PlayerLifeModified) 
            {modifier = damage, source = DamageSource.Boat};
    }

    private BattleEvent ModifyLife(BattleEvent battleEvent)
    {
        var previousHealth = Health;
        Health += battleEvent.modifier;

        if (Health <= 0) Health = 0;

        if (Health < previousHealth)
            return new BattleEvent(BattleEventType.PlayerLostLife);
        
        if (Health > MaxHealth)
            Health = MaxHealth;
        
        if (Health > previousHealth)
            return new BattleEvent(BattleEventType.PlayerGainedLife);
        
        return BattleEvent.None;
    }

    private BattleEventPackage PlayerMovedPlank()
    {
        return new BattleEventPackage(UsedMove(), new BattleEvent(BattleEventType.PlankMoved));
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
}

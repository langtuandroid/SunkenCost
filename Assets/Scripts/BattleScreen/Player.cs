using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;

public class Player : BattleEventResponder
{
    public static Player Current;

    private int? _baseMovesPerTurn;
    public int Gold => RunProgress.Current.PlayerStats.Gold;
    public int Health { get; private set; }
    private int MaxHealth { get; set; }
    public int MovesUsedThisTurn { get; private set; } = 0;
    public int? MoveLimit { get; set; }
    
    public bool IsOutOfMoves => MoveLimit.HasValue && MovesUsedThisTurn >= MoveLimit;

    protected override void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;
        
        Health = RunProgress.Current.PlayerStats.Health;
        MaxHealth = RunProgress.Current.PlayerStats.MaxHealth;
        _baseMovesPerTurn = RunProgress.Current.PlayerStats.MovesPerTurn;
        MoveLimit = _baseMovesPerTurn;
        base.Awake();
    }

    public override List<BattleEventResponseTrigger> GetBattleEventResponseTriggers()
    {
        return new List<BattleEventResponseTrigger>
        {
            EventResponseTriggerWithArgument(BattleEventType.PlayerLostLife, 
                e => new BattleEvent(BattleEventType.PlayerDied),
                e => Health <= 0),
            PackageResponseTrigger(BattleEventType.PlayerMovedPlank, 
                e => PlayerMovedPlank()),
            EventResponseTriggerWithArgument(BattleEventType.EnemyAttackedBoat, 
                e => EnemyReachedEnd(-e.modifier)),
            EventResponseTriggerWithArgument(BattleEventType.PlayerLifeModified, ModifyLife),
            EventResponseTriggerWithArgument(BattleEventType.TryGainedGold, TryGainGold),
        };
    }

    public override List<BattleEventActionTrigger> GetBattleEventActionTriggers()
    {
        return new List<BattleEventActionTrigger>
        {
            ActionTrigger(BattleEventType.StartedNextPlayerTurn, ResetMoves),
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
        if (MoveLimit.HasValue) MovesUsedThisTurn += 1;
        return new BattleEvent(BattleEventType.PlayerUsedMove);
    }

    private BattleEvent TryGainGold(BattleEvent battleEvent)
    {
        RunProgress.Current.PlayerStats.Gold += battleEvent.modifier;
        return new BattleEvent(BattleEventType.GainedGold);
    }
}

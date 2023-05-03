using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

public class Player : BattleEventResponder
{
    public static Player Current;

    private int _baseMovesPerTurn;
    
    public int Gold { get; private set; }

    public int Lives { get; private set; }
    private int MaxLives { get; set; }
    public int MovesUsedThisTurn { get; private set; } = 0;
    public int MovesPerTurn { get; set; }
    
    public bool IsOutOfMoves => MovesUsedThisTurn >= MovesPerTurn;
    public int MovesRemaining => MovesPerTurn - MovesUsedThisTurn;

    protected override void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;

        Gold = RunProgress.PlayerStats.Gold;
        Lives = RunProgress.PlayerStats.Lives;
        MaxLives = RunProgress.PlayerStats.MaxLives;
        _baseMovesPerTurn = RunProgress.PlayerStats.MovesPerTurn;
        MovesPerTurn = _baseMovesPerTurn;
        base.Awake();
    }

    private void ResetMoves()
    {
        MovesUsedThisTurn = 0;
    }

    private BattleEvent EnemyReachedEnd()
    {
        return new BattleEvent(BattleEventType.PlayerLifeModified) 
            {modifier = -1, source = DamageSource.Boat};
    }

    private BattleEvent ModifyLife(BattleEvent battleEvent)
    {
        var previousLives = Lives;
        Lives += battleEvent.modifier;

        if (Lives <= 0)
            return new BattleEvent(BattleEventType.PlayerDied);
        
        if (Lives < previousLives)
            return new BattleEvent(BattleEventType.PlayerLostLife);
        
        if (Lives > MaxLives)
            Lives = MaxLives;
        
        if (Lives > previousLives)
            return new BattleEvent(BattleEventType.PlayerGainedLife);
        
        return BattleEvent.None;
    }
    
    private BattleEvent UsedMove()
    {
        MovesUsedThisTurn += 1;
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
            BattleEventType.EnemyReachedBoat => EnemyReachedEnd(),
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

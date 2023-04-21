using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;

public class Player : BattleEventResponder
{
    public static Player Current;
    
    public int MovesUsedThisTurn { get; private set; } = 0;
    public int MovesPerTurn { get; set; }
    
    private int _baseMovesPerTurn;
    
    public int Gold { get; private set; }

    public int Lives { get; private set; }
    private int MaxLives { get; set; }
    
    public bool IsOutOfMoves => MovesUsedThisTurn >= MovesPerTurn;
    public int MovesRemaining => MovesPerTurn - MovesUsedThisTurn;

    private void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;

        Gold = RunProgress.PlayerStats.Gold;
        Lives = RunProgress.PlayerStats.Lives;
        MaxLives = RunProgress.PlayerStats.MaxLives;
        _baseMovesPerTurn = RunProgress.PlayerStats.MovesPerTurn;
        MovesPerTurn = _baseMovesPerTurn;
    }

    private void ResetMoves()
    {
        MovesUsedThisTurn = 0;
    }

    private BattleEvent EnemyReachedEnd()
    {
        return new BattleEvent(BattleEventType.PlayerLifeModified, this) 
            {modifier = -1, damageSource = DamageSource.Boat};
    }

    private BattleEvent ModifyLife(BattleEvent battleEvent)
    {
        var previousLives = Lives;
        Lives += battleEvent.modifier;

        if (Lives <= 0)
            return new BattleEvent(BattleEventType.PlayerDied, this);
        
        if (Lives < previousLives)
            return new BattleEvent(BattleEventType.PlayerGainedLife, this);
        
        if (Lives > MaxLives)
            Lives = MaxLives;
        
        if (Lives > previousLives)
            return new BattleEvent(BattleEventType.PlayerGainedLife, this);
        
        return BattleEvent.None;
    }
    
    private BattleEvent UsedMove()
    {
        MovesUsedThisTurn += 1;
        return new BattleEvent(BattleEventType.PlayerUsedMove, this);
    }

    private BattleEvent TryGainGold(BattleEvent battleEvent)
    {
        Gold += battleEvent.modifier;
        return new BattleEvent(BattleEventType.GainedGold, this);
    }

    public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
    {
        switch (battleEvent.type)
        {
            case BattleEventType.EndedEnemyTurn:
                ResetMoves();
                break;
            case BattleEventType.PlayerMovedPlank: case BattleEventType.EnemyReachedBoat: 
            case BattleEventType.PlayerLostLife: case BattleEventType.PlayerGainedLife: 
            case BattleEventType.TryGainedGold:
                return true;
        }

        return false;
    }

    protected override BattleEvent GetResponse(BattleEvent battleEvent)
    {
        return battleEvent.type switch
        {
            BattleEventType.PlayerMovedPlank => UsedMove(),
            BattleEventType.EnemyReachedBoat => EnemyReachedEnd(),
            BattleEventType.PlayerLifeModified => ModifyLife(battleEvent),
            BattleEventType.TryGainedGold => TryGainGold(battleEvent),
            _ => throw new Exception("Unexpected BattleEventType")
        };
    }
}

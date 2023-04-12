using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;

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
        return new PlayerLifeBattleEvent(-1, DamageSource.Boat);
    }

    private BattleEvent ModifyLife(PlayerLifeBattleEvent lifeBattleEvent)
    {
        var previousLives = Lives;
        Lives += lifeBattleEvent.lifeModAmount;

        if (Lives <= 0)
            return new BattleEvent(BattleEventType.PlayerDied);
        
        if (Lives < previousLives)
            return new BattleEvent(BattleEventType.PlayerGainedLife);
        
        if (Lives > MaxLives)
            Lives = MaxLives;
        
        if (Lives > previousLives)
            return new BattleEvent(BattleEventType.PlayerGainedLife);
        
        return new BattleEvent(BattleEventType.None);
    }
    
    private BattleEvent UsedMove()
    {
        MovesUsedThisTurn += 1;
        return new BattleEvent(BattleEventType.PlayerUsedMove);
    }

    private BattleEvent TryGainGold(TryGainGoldBattleEvent tryGainGoldBattleEvent)
    {
        Gold += tryGainGoldBattleEvent.amount;
        return new BattleEvent(BattleEventType.GainedGold);
    }

    public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
    {
        switch (battleEvent.Type)
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

    public override IEnumerator DisplayEvent(BattleEvent battleEvent)
    {
        throw new NotImplementedException();
    }

    protected override BattleEvent GetResponse(BattleEvent battleEvent)
    {
        return battleEvent.Type switch
        {
            BattleEventType.PlayerMovedPlank => UsedMove(),
            BattleEventType.EnemyReachedBoat => EnemyReachedEnd(),
            BattleEventType.PlayerLifeModified => ModifyLife(battleEvent as PlayerLifeBattleEvent),
            BattleEventType.TryGainedGold => TryGainGold(battleEvent as TryGainGoldBattleEvent),
            _ => throw new Exception("Unexpected BattleEventType")
        };
    }
}

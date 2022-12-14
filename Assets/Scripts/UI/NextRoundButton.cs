using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoundButton : InGameButton
{
    public static NextRoundButton current;

    protected override void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        base.Awake();
    }

    protected override bool TestForSuccess()
    {
        return BattleManager.Current.TryNextTurn();
    }
}

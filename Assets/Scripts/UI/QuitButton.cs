using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : InGameButton
{
    public static QuitButton current;

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

    protected override bool TryClick()
    {
        BattleManager.Current.Quit();
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using BattleScreen;
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
        Battle.Current.ClickedQuit();
        return true;
    }
}

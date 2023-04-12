using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameButton : InGameButton
{
    protected override bool TryClick()
    {
        StartCoroutine(ExecuteAfterSound(MainMenu.PlayGame));
        return true;
    }
}

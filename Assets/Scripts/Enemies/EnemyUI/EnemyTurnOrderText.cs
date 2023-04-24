using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnOrderText : EnemyUIText
{
    [SerializeField] private Color _myTurnColor;

    public void SetTurnOrder(int turnOrder)
    {
        // Waiting
        if (turnOrder == -1)
        {
            SetText("");
            return;
        }

        var turnOrderString = turnOrder.ToString();
        var lastChar = turnOrderString[^1];
        var suffix = "th";

        switch (lastChar)
        {
            case '1' :
                suffix = "st";
                break;
            case '2' :
                suffix = "nd";
                break;
            case '3' :
                suffix = "rd";
                break;
        }
        
        SetText(turnOrder + suffix);
    }

    public void MyTurn()
    {
        ChangeColor(_myTurnColor);
    }

    public void EndMyTurn()
    {
        ChangeColor(defaultColor);
    }
}

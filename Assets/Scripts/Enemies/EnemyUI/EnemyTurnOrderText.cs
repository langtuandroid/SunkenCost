using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnOrderText : EnemyUIText
{
    [SerializeField] private Color _myTurnColor;

    public void SetTurnOrder(int place)
    {
        var turnOrder = place;
        
        // Waiting
        if (place == -1)
        {
            SetText("");
            return;
        }

        var placeString = place.ToString();
        var lastChar = placeString[placeString.Length - 1];
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

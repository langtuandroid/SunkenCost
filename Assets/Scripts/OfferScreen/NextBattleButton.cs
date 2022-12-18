using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NextBattleButton : MonoBehaviour
{
    [SerializeField] private Transform cardsToTakeGrid;
    public void Click()
    {
        // TODO: Get this somewhere better, make neater
        var deck = cardsToTakeGrid.GetChild(0).GetComponentsInChildren<DesignInfo>().Select(d => d.design)
            .ToList();
        if (deck.Count >= GameProgress.MaxPlanks) deck = deck.GetRange(0, GameProgress.MaxPlanks);
        PlayerInventory.Deck = deck;
        
        MainManager.Current.LoadNextBattle();
    }
}

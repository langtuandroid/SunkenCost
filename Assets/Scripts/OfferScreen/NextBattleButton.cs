using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NextBattleButton : MonoBehaviour
{
    [SerializeField] private Transform cardsToTakeGrid;
    public void Click()
    {
        Deck.Designs = cardsToTakeGrid.GetChild(0).GetComponentsInChildren<DesignInfo>().Select(d => d.design).ToList();
        MainManager.Current.LoadNextBattle();
    }
}

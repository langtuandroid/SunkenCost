using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager current;

    public int gold  = 0;

    public int StickCost
    {
        get
        {
            var stickCost = StickManager.current.stickCount - 3;
            if (StickManager.current.stickCount <= 3) stickCost = 0;
            return stickCost;
        }
    }

    public int RedrawCost { get; private set; } = 1;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
    }

    public void AlterGold(int amount, string reason = "")
    {
        gold += amount;
    }

    public bool CanBuyStick => gold >= StickCost;
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelController
{
    public int Level { get; private set; } = 1;
    public int Xp { get; private set; } = 0;

    public int XpNeededForNextLevel { get; private set; } = 1;

    public void AddXp(int amount)
    {
        Xp += amount;

        while (Xp >= XpNeededForNextLevel)
        {
            Level++;
            Xp %= XpNeededForNextLevel;
            XpNeededForNextLevel++;
            if (XpNeededForNextLevel > Level / 2) XpNeededForNextLevel = Level / 2;
            
            BattleEvents.Current.LevelUp();
            
            if (Level % 3 == 0)
                StickManager.current.CreateStick();
        }
    }
}

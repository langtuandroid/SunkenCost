using System;
using UnityEngine;

namespace Etchings
{
    public class RangedEtching : DamageEtching
    {
        protected override bool TestCharMovementActivatedEffect()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;

            // Not in play
            if (enemy.StickNum == 0 || enemy.StickNum >= PlankMap.current.stickCount) return false;

            var stickNum = Plank.GetPlankNum();

            
            // On this stick or not within range
            if (!CheckInfluence(enemy.StickNum)) return false;
            
            enemy.Plank.SetTempColour(design.Color);
            DamageEnemy(enemy);
            InGameSfxManager.current.DamagedEnemy();
            UsesUsedThisTurn += 1;
            return true;
        }

        protected override bool CheckInfluence(int stickNum)
        {
            return Math.Abs(stickNum - Plank.GetPlankNum()) >= MinRange &&
                   Math.Abs(stickNum - Plank.GetPlankNum()) <= MaxRange;
        }
    }
}
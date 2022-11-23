using System;
using UnityEngine;

namespace Etchings
{
    public class RangedEtching : DamageEtching
    {
        protected override bool TestCharMovementActivatedEffect()
        {
            var enemy = ActiveEnemiesManager.current.CurrentEnemy;

            // Not in play
            if (enemy.StickNum == 0 || enemy.StickNum >= StickManager.current.stickCount) return false;

            var stickNum = Stick.GetStickNumber();

            
            // On this stick or not within range
            if (!CheckInfluence(enemy.StickNum)) return false;
            
            enemy.Stick.SetTempColour(design.Color);
            DamageEnemy(enemy);
            InGameSfxManager.current.DamagedEnemy();
            UsesUsedThisTurn += 1;
            return true;
        }

        protected override bool CheckInfluence(int stickNum)
        {
            return Math.Abs(stickNum - Stick.GetStickNumber()) >= MinRange &&
                   Math.Abs(stickNum - Stick.GetStickNumber()) <= MaxRange;
        }
    }
}
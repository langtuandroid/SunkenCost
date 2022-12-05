
/*
using UnityEngine;

namespace Etchings
{
    public class BlockEtching : CharMovementActivatedEtching
    {
        private Stat _bufferMin;
        private Stat _bufferMax;

        private int BufferMax => _bufferMax.Value;
        private int BufferMin => _bufferMin.Value;

        protected override void Start()
        {
            _bufferMin = new Stat(design.GetStat(St.MinRange));
            _bufferMax = new Stat(design.GetStat(St.MaxRange));
            base.Start();
        }

        protected override bool TestCharMovementActivatedEffect()
        {
            var enemy = ActiveEnemiesManager.current.CurrentEnemy;
            int enemyDistance;
            var stickNum = Stick.GetStickNumber();
            
            switch (enemy.Direction)
            {
                case 1:
                    // Moving towards from left
                    enemyDistance = stickNum - enemy.StickNum;
                    break;
                case -1:
                    // Moving towards from right
                    enemyDistance = enemy.StickNum - stickNum;
                    break;
                default:
                    Debug.Log("Enemy direction weird! :" + enemy.Direction);
                    enemyDistance = 1;
                    break;
            }
            
            if (enemyDistance > BufferMax || enemyDistance < BufferMin) return false;
            
            enemy.Stick.SetTempColour(design.Color);
            enemy.Block();
            UsesUsedThisTurn += 1;
            return true;

        }
        
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }
        
    }
}
*/
using UnityEngine;

namespace Enemies.Enemies
{
    public class FlyingFishEnemy : Enemy
    {
        protected override void Init()
        {
            Name = "Flighting Fish";
            Mover.AddMove(1);
            SetInitialHealth(30);
            Gold = 1;
        }

        public override string GetDescription()
        {
            return "Skips over the plank ahead";
        }

        protected override void StartOfTurnAbility()
        {
            Mover.AddSkip(1);
            base.StartOfTurnAbility();
        }
        
        protected override bool TestForStartOfTurnAbility()
        {
            return (StickNum + NextDirection != StickManager.current.stickCount);
        }
    }
}
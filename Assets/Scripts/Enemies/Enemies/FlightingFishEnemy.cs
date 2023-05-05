using UnityEngine;

namespace Enemies.Enemies
{
    public class FlightingFishEnemy : Enemy
    {
        protected override void Init()
        {
            Name = "Flighting Fish";
            Mover.AddMove(MovementType.Skip, 2);
            SetInitialHealth(35);
            Gold = 1;
        }

        public override string GetDescription()
        {
            return "Skips over the plank ahead";
        }
    }
}
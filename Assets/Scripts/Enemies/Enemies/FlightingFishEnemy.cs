using UnityEngine;

namespace Enemies.Enemies
{
    public class FlightingFishEnemy : Enemy
    {
        public override string GetDescription()
        {
            return "Skips over the plank ahead (won't skip the last plank)";
        }
    }
}
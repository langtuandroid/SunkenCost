using UnityEngine;

namespace Enemies.Enemies
{
    public class SteeloEnemy : EliteEnemy
    {
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Steelo";
            Mover.AddMove(1);
            SetInitialHealth(120);
            Gold = 2;
        }

        public override string GetDescription()
        { 
            return "Big, slow, harder snail";
        }
    }
}
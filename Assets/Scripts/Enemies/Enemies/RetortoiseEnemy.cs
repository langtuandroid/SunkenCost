namespace Enemies.Enemies
{
    public class RetortoiseEnemy : Enemy
    {
        private const int HEAL_AMOUNT = 5;
        
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Retortoise";
            Mover.AddMove(1);
            SetInitialHealth(50);
            Gold = 5;
        }

        public override string GetDescription()
        { 
            return "Gets faster and heals " + HEAL_AMOUNT + " health each time it's attacked";
        }

        public override void TakeDamage(int damage, DamageSource damageSource)
        {
            if (damageSource != DamageSource.Plank) return;
            
            Mover.AddMovementModifier(1);
            Heal(HEAL_AMOUNT);
            
            base.TakeDamage(damage, damageSource);
        }
    }
}
namespace BattleScreen.BattleEvents.EventTypes
{
    public class PlayerLifeBattleEvent : BattleEvent
    {
        public readonly int lifeModAmount;
        public readonly DamageSource damageSource;

        public PlayerLifeBattleEvent(int lifeModAmount, DamageSource damageSource)
            : base(BattleEventType.PlayerLifeModified) => (this.lifeModAmount, this.damageSource ) = (lifeModAmount, damageSource);
    }
}
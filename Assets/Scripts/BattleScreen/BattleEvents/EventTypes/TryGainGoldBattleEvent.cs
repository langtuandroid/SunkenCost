namespace BattleScreen.BattleEvents.EventTypes
{
    public class TryGainGoldBattleEvent : BattleEvent
    {
        public readonly int amount;

        public TryGainGoldBattleEvent(int amount) : base(BattleEventType.TryGainedGold) => this.amount = amount;
    }
}
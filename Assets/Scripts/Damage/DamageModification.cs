using BattleScreen.BattleEvents;

namespace Damage
{
    public readonly struct DamageModification
    {
        public readonly BattleEventHandler creator;
        public readonly int modificationAmount;

        public DamageModification(BattleEventHandler creator, int modificationAmount) =>
            (this.creator, this.modificationAmount) = (creator, modificationAmount);
    }
}
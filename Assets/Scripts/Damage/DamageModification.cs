using BattleScreen;
using BattleScreen.BattleEvents;

namespace Damage
{
    public readonly struct DamageModification
    {
        public readonly BattleEventResponder creator;
        public readonly int modificationAmount;

        public DamageModification(BattleEventResponder creator, int modificationAmount) =>
            (this.creator, this.modificationAmount) = (creator, modificationAmount);
    }
}
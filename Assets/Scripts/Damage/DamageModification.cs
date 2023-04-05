using BattleScreen.ActionsAndResponses;

namespace Damage
{
    public readonly struct DamageModification
    {
        public readonly IBattleEventCreator creator;
        public readonly int modificationAmount;

        public DamageModification(IBattleEventCreator creator, int modificationAmount) =>
            (this.creator, this.modificationAmount) = (creator, modificationAmount);
    }
}
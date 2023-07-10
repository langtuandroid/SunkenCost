using System.Collections.Generic;
using BattleScreen;

namespace Damage
{
    public readonly struct DamageModificationPackage
    {
        public readonly List<DamageModification> flatModifications;
        public readonly List<DamageModification> multiModifications;

        public int ModCount => flatModifications.Count + multiModifications.Count;

        public DamageModificationPackage
            (List<DamageModification> flatModifications, List<DamageModification> multiModifications)
            => (this.flatModifications, this.multiModifications) = (flatModifications, multiModifications);

        public static DamageModificationPackage Empty()
        {
            return new DamageModificationPackage(new List<DamageModification>(), 
                new List<DamageModification>());
        }
    }
}
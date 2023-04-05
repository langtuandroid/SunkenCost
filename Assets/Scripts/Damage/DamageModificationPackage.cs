using System.Collections.Generic;
using BattleScreen;

namespace Damage
{
    public readonly struct DamageModificationPackage
    {
        public readonly List<DamageModification> flatModifications;
        public readonly List<DamageModification> multiModifications;

        public DamageModificationPackage
            (List<DamageModification> flatModifications, List<DamageModification> multiModifications)
            => (this.flatModifications, this.multiModifications) = (flatModifications, multiModifications);
    }
}
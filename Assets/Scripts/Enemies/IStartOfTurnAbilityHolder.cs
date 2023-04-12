using System.Collections;
using System.Collections.Generic;
using BattleScreen;

namespace Enemies
{
    public interface IStartOfTurnAbilityHolder
    {
        public List<BattleEvent> GetStartOfTurnAbility();
    }
}
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Enemies
{
    public interface IStartOfTurnAbilityHolder
    {
        public bool GetIfUsingStartOfTurnAbility();
        
        public BattleEventPackage GetStartOfTurnAbility();
    }
}
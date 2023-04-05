using System.Collections;

namespace Enemies
{
    public interface IStartOfTurnAbilityHolder
    {
        public IEnumerator ExecuteStartOfTurnAbility();
    }
}
using BattleScreen;
using Damage;
using Designs;
using Enemies;

namespace Etchings
{
    public abstract class DamageEtching : LandedOnPlankActivatedEtching
    {
        private int Damage => Design.GetStat(StatType.Damage);
        protected int MinRange => Design.GetStat(StatType.MinRange);
        protected int MaxRange => Design.GetStat(StatType.MaxRange);

        protected virtual BattleEvent DamageEnemy(int enemyResponderID)
        {
            return DamageHandler.DamageEnemy(Damage, enemyResponderID, DamageSource.Etching);
        }

        protected virtual BattleEvent DamageEnemy(Enemy enemy)
        {
            return DamageEnemy(enemy.ResponderID);
        }
    }
}
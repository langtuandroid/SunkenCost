using BattleScreen.BattleEvents;
using Etchings;

namespace Pickups.Varnishes
{
    public abstract class Varnish : BattleEventResponder
    {
        protected Etching Etching { get; private set; }

        public void Init(Etching etching)
        {
            Etching = etching;
        }
    }
}
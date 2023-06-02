using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using Etchings;

namespace Varnishes
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
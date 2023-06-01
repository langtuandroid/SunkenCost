using System.Collections.Generic;
using System.Linq;

namespace BattleScreen.BattleEvents
{
    public readonly struct BattleEventPackage
    {
        public readonly List<BattleEvent> battleEvents;

        public static BattleEventPackage Empty => new BattleEventPackage();
        public bool IsEmpty => battleEvents == null;
        
        public BattleEventPackage(params BattleEvent[] battleEvents) => this.battleEvents = battleEvents.ToList();
        public BattleEventPackage(List<BattleEvent> battleEventsList) => battleEvents = battleEventsList;
        public BattleEventPackage(IEnumerable<BattleEvent> battleEventsList) => battleEvents = battleEventsList.ToList();
        public BattleEventPackage(BattleEvent battleEvent) => battleEvents = new List<BattleEvent> {battleEvent};
        public BattleEventPackage(BattleEventType battleEventType) =>
            battleEvents = new List<BattleEvent> {new BattleEvent(battleEventType)};
        
        public BattleEventPackage WithIdentifier(BattleEventType battleEventType, int responderID)
        {
            battleEvents.Add(new BattleEvent(battleEventType) {primaryResponderID = responderID});
            return this;
        }
    }
}
namespace BattleScreen.BattleEvents.EventTypes
{
    public class VisualisedBattleEvent : BattleEvent
    {
        public IBattleEventVisualiser visualiser;

        public VisualisedBattleEvent(BattleEventType battleEventType, IBattleEventVisualiser visualiser) :
            base(battleEventType) => this.visualiser = visualiser;
    }
}
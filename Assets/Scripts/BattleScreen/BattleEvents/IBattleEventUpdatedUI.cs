namespace BattleScreen.BattleEvents
{
    public interface IBattleEventUpdatedUI
    {
        public bool GetIfUpdating(BattleEvent battleEvent);
        public void SaveStateResponse(BattleEventType battleEventType);
        public void LoadNextState();
    }
}
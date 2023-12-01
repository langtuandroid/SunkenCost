using System.Collections.Generic;
using BattleScreen.BattleEvents;
using Designs;

namespace BattleScreen.BattleBoard
{
    public class Deck
    {
        private readonly Queue<Design> _deck;

        public Deck(IEnumerable<Design> deck)
        {
            _deck = new Queue<Design>(deck.Shuffle());
            for (var i = 0; i < RunProgress.Current.PlayerStats.MaxPlanks; i++)
            {
                var nextDesign = _deck.Dequeue();
                CreateNewPlank(nextDesign);
            }
        }
        
        public BattleEvent ReRoll(int responderID)
        {
            var oldEtching = BattleEventResponseSequencer.Current.GetEtchingByResponderID(responderID);
            var plankNum = oldEtching.PlankNum;
            var plank = Board.Current.GetPlank(plankNum);
            var newEtching = EtchingFactory.Current.CreateEtching(plank, _deck.Dequeue());
            
            _deck.Enqueue(oldEtching.Design);
            
            return new BattleEvent(BattleEventType.Rolled)
            {
                primaryResponderID = oldEtching.ResponderID,
                secondaryResponderID = newEtching.ResponderID
            };
        }

        private void CreateNewPlank(Design design)
        {
            var plank = PlankFactory.Current.CreatePlank();
            EtchingFactory.Current.CreateEtching(plank, design);
        }
    }
}
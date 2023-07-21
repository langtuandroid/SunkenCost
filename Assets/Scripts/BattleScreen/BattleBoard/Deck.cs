using System.Linq;

namespace BattleScreen.BattleBoard
{
    public class Deck
    {
        private bool _hasSpawned = false;
        
        public void SpawnDeck()
        {
            if (_hasSpawned)
            {
                Board.Current.DestroyAllPlanks();
            }

            _hasSpawned = true;
            
            var deck = RunProgress.Current.PlayerStats.Deck.Shuffle().ToArray();
            for (var i = 0; i < RunProgress.Current.PlayerStats.MaxPlanks; i++)
            {
                var plank = PlankFactory.Current.CreatePlank();
                EtchingFactory.Current.CreateEtching(plank, deck[i]);
            }
        }
    }
}
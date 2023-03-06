using System.Collections;
using UnityEngine;

namespace Etchings
{
    public abstract class ActiveEtching : Etching
    {
        private Color _normalColor;
        private CanvasGroup _canvasGroup;
    
        protected override void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        
            BattleEvents.Current.OnEndEnemyTurn += EndEnemyTurn;
            BattleEvents.Current.OnSticksUpdated += designDisplay.Refresh;
        
            base.Start();
            _normalColor = designDisplay.TitleText.color;
        }
    
        protected void EndEnemyTurn()
        {
            UsesUsedThisTurn = 0;

            if (deactivationTurns > 0)
            {
                deactivationTurns--;

                if (deactivationTurns == 0)
                {
                    Stick.SetActive(true);
                }
            }
        }
    
    
        protected IEnumerator ColorForActivate()
        {
            designDisplay.TitleText.color = Color.green;
            yield return new WaitForSeconds(BattleManager.AttackTime);
            designDisplay.TitleText.color = _normalColor;
        }

        public void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1f : 0f;
        }
        

        public void Deactivate(int turns)
        {
            Stick.SetActive(false);
            deactivationTurns = turns;
        }

        private void OnDestroy()
        {
            BattleEvents.Current.OnEndEnemyTurn -= EndEnemyTurn;
            BattleEvents.Current.OnSticksUpdated -= designDisplay.Refresh;
        }
    }
}

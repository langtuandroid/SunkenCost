using System;
using System.Collections;
using UnityEngine;

namespace BattleScreen.UI
{
    public class IslandAnimator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _disturbanceCanvasGroup;
        [SerializeField] private CanvasGroup _islandCanvasGroup;

        private static float Speed => 0.01f * (1 - Battle.ActionExecutionSpeed);

        public void SurfaceIsland()
        {
            StartCoroutine(SurfaceIslandAnimation());
        }

        public void SinkIsland()
        {
            StartCoroutine(SinkIslandAnimation());
        }

        private IEnumerator SurfaceIslandAnimation()
        {
            _islandCanvasGroup.gameObject.SetActive(true);
            
            yield return new WaitForSecondsRealtime(0.3f);
            
            for (var progress = 0f; progress < 1; progress += Speed)
            {
                _islandCanvasGroup.alpha = progress;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            
            for (var progress = 1f; progress > 0; progress -= Speed * 2)
            {
                _disturbanceCanvasGroup.alpha = progress;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            
            _disturbanceCanvasGroup.gameObject.SetActive(false);
        }
        
        private IEnumerator SinkIslandAnimation()
        {
            _disturbanceCanvasGroup.gameObject.SetActive(true);
            
            for (var progress = 0f; progress < 1; progress += Speed)
            {
                _islandCanvasGroup.alpha = 1f - progress;
                _disturbanceCanvasGroup.alpha = progress;
                
                yield return new WaitForSecondsRealtime(0.01f);
            }
            
            _islandCanvasGroup.gameObject.SetActive(false);
        }
    }
}
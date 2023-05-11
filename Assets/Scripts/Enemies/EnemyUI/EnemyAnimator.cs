using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.EnemyUI
{
    public class EnemyAnimator : MonoBehaviour
    {
        private const float AnimationSpeed = 0.3f;
        
        [SerializeField] private Image _image;

        private int _currentSpriteIndex = 0;

        private EnemyAnimation _idleAnimation;

        public void Init(string enemyName)
        {
            var spritePack = EnemyLoader.Current.GetEnemySpritePack(enemyName);
            _idleAnimation = new EnemyAnimation(spritePack.idleSprites, true);
        }

        private void Start()
        {
            Play(_idleAnimation);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void Play(EnemyAnimation enemyAnimation)
        {
            StopAllCoroutines();
            StartCoroutine(Animate(enemyAnimation));
        }

        private IEnumerator Animate(EnemyAnimation enemyAnimation)
        {
            _currentSpriteIndex = 0;
            
            while (true)
            {
                _image.sprite = enemyAnimation.sprites[_currentSpriteIndex];
                _currentSpriteIndex++;
                
                yield return new WaitForSecondsRealtime(AnimationSpeed);
                
                if (_currentSpriteIndex < enemyAnimation.sprites.Length) continue;
                
                if (enemyAnimation.toLoop)
                    _currentSpriteIndex = 0;
                else
                    yield break;
            }
        }

        private class EnemyAnimation
        {
            public readonly Sprite[] sprites;
            public readonly bool toLoop;

            public EnemyAnimation(Sprite[] sprites, bool toLoop)
            {
                this.sprites = sprites;
                this.toLoop = toLoop;
            }
        }
    }
}
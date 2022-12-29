using System;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public abstract class InGameItem : MonoBehaviour
    {
        public string Title { get; set; }
        public string Description { get; set; }

        protected void Awake()
        {
            BattleEvents.Current.OnBeginPlayerTurn += OnBeginPlayerTurn;
            BattleEvents.Current.OnEndPlayerTurn += OnEndPlayerTurn;
            BattleEvents.Current.OnBeginEnemyTurn += OnBeginEnemyTurn;
            BattleEvents.Current.OnEndEnemyTurn += OnEndEnemyTurn;

            Activate();
        }
        
        protected abstract void Activate();

        protected virtual void OnBeginPlayerTurn()
        {
        }
        
        protected virtual void OnEndPlayerTurn()
        {
        }
        
        protected virtual void OnBeginEnemyTurn()
        {
        }
        
        protected virtual void OnEndEnemyTurn()
        {
        }

        protected virtual void OnDestroy()
        {
            BattleEvents.Current.OnBeginPlayerTurn -= OnBeginPlayerTurn;
            BattleEvents.Current.OnEndPlayerTurn -= OnEndPlayerTurn;
            BattleEvents.Current.OnBeginEnemyTurn -= OnBeginEnemyTurn;
            BattleEvents.Current.OnEndEnemyTurn -= OnEndEnemyTurn;
        }
    }
}

using System;
using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        protected void Awake()
        {
            BattleEvents.Current.OnBeginPlayerTurn += OnBeginPlayerTurn;
            BattleEvents.Current.OnEndPlayerTurn += OnEndPlayerTurn;
            BattleEvents.Current.OnBeginEnemyTurn += OnBeginEnemyTurn;
            BattleEvents.Current.OnEndEnemyTurn += OnEndEnemyTurn;

            Activate();
        }

        protected virtual void Activate()
        {
        }

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

        protected void OnDestroy()
        {
            BattleEvents.Current.OnBeginPlayerTurn -= OnBeginPlayerTurn;
            BattleEvents.Current.OnEndPlayerTurn -= OnEndPlayerTurn;
            BattleEvents.Current.OnBeginEnemyTurn -= OnBeginEnemyTurn;
            BattleEvents.Current.OnEndEnemyTurn -= OnEndEnemyTurn;
        }
    }
}

using System;
using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        protected void Awake()
        {
            GameEvents.current.OnBeginPlayerTurn += OnBeginPlayerTurn;
            GameEvents.current.OnEndPlayerTurn += OnEndPlayerTurn;
            GameEvents.current.OnBeginEnemyTurn += OnBeginEnemyTurn;
            GameEvents.current.OnEndEnemyTurn += OnEndEnemyTurn;

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
            GameEvents.current.OnBeginPlayerTurn -= OnBeginPlayerTurn;
            GameEvents.current.OnEndPlayerTurn -= OnEndPlayerTurn;
            GameEvents.current.OnBeginEnemyTurn -= OnBeginEnemyTurn;
            GameEvents.current.OnEndEnemyTurn -= OnEndEnemyTurn;
        }
    }
}

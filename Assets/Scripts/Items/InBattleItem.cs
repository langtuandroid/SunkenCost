using System;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public abstract class InBattleItem : MonoBehaviour
    {
        protected void Awake()
        {
            Activate();
        }
        
        protected abstract void Activate();

        protected virtual void OnDestroy()
        {
        }
    }
}

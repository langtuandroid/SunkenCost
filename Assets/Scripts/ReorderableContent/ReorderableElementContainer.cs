using System;
using UnityEngine;

namespace ReorderableContent
{
    public class ReorderableElementContainer : MonoBehaviour
    {
        public event Action OnChildrenChanged;

        private void OnTransformChildrenChanged()
        {
            OnChildrenChanged?.Invoke();
        }
    }
}
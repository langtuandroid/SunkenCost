using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReorderableContent
{
    public interface IReorderableGridEventListener
    {
        public Func<bool> CanGrabElementsFunc();
        public void ElementsOrderChangedByDrag();
        public void ElementsRefreshed(List<Transform> listElements);
    }
}
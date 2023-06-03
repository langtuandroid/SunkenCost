using System.Collections.Generic;
using UnityEngine;

namespace ReorderableContent
{
    public interface IReorderableGridEventListener
    {
        public void ElementsOrderChangedByDrag();
        public void ElementsRefreshed(List<Transform> listElements);
    }
}
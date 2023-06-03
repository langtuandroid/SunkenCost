using System;

namespace ReorderableContent
{
    public interface IMergeableReorderableEventListener : IReorderableElementEventListener
    {
        public Func<ReorderableElement, bool> GetIfCanMerge();
        public void StartMerge();
        public void FinaliseMerge(ReorderableElement element);
        public void CancelMerge();
    }
}
using System;

namespace ReorderableContent
{
    public interface IMergeableReorderableEventListener : IReorderableElementEventListener
    {
        public Func<ReorderableElement, bool> GetIfCanMerge();
        public void OfferMerge(ReorderableElement element);
        public void FinaliseMerge();
        public void CancelMerge();
    }
}
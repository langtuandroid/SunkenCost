namespace ReorderableContent
{
    public interface IReorderableElementEventListener
    {
        public void Grabbed();
        public void HoveringOverList(ReorderableGrid listHoveringOver);
        public void Released();
    }
}
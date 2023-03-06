namespace Deck
{
    public abstract class CommonDesign : Design
    {
        protected override int GetRarity()
        {
            return 1;
        }
    }
    
    public abstract class UncommonDesign : Design
    {
        protected override int GetRarity()
        {
            return 2;
        }
    }
    
    public abstract class RareDesign : Design
    {
        protected override int GetRarity()
        {
            return 3;
        }
    }
}
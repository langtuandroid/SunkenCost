namespace OfferScreen
{
    public class BuyPlankOffer : UpgradeOffer
    {
        protected override void Buy()
        {
            OfferManager.Current.BuyPlank(Cost);
        }
    }
}
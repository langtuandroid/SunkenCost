namespace OfferScreen
{
    public class BuyMoveOffer : UpgradeOffer
    {
        protected override void Buy()
        {
            OfferManager.Current.BuyMove(Cost);
        }
    }
}
using Designs;
using Items;
using UnityEngine;

namespace Disturbances
{
    public class CardDisturbance : Disturbance
    {
        public Design Design { get; private set; }
        
        public CardDisturbance(DisturbanceAsset disturbanceAsset, int modifier, Design design) 
            : base(disturbanceAsset, modifier)
        {
            Design = design;
        }
        
        public override string GetAdditionalTitle()
        {
            return base.GetDescription() + Design.Title;
        }

        public override string GetDescription()
        {
            return DesignLoader.GetDescription(Design);
        }
        
        public override Sprite GetSprite()
        {
            return Design.Sprite;
        }
    }
}
using UnityEngine;

namespace Disturbances
{
    public enum DisturbanceType
    {
        // Normal
        None = 0,
        GoldRush = 100,
        Heart = 200,
        UpgradeCard = 300,
        Card = 400,
        Item = 500,
        Move = 600,

        // Elite
        EliteItem = 1000,
        EliteCard = 1200,
    }
    
    public class Disturbance
    {
        private readonly DisturbanceAsset _disturbanceAsset;
        public DisturbanceType DisturbanceType => _disturbanceAsset.disturbanceType;
        
        public int Modifier { get; private set; }
        public bool IsElite => (int)DisturbanceType >= 1000;

        public Disturbance(DisturbanceAsset disturbanceAsset, int modifier)
        {
            _disturbanceAsset = disturbanceAsset;
            Modifier = modifier;
        }

        public string GetTitle()
        {
            return _disturbanceAsset.title + "  " + GetAdditionalTitle();
        }
        

        public virtual string GetAdditionalTitle()
        {
            return "";
        }

        public virtual string GetDescription()
        {
            return _disturbanceAsset.description.Replace("@", _disturbanceAsset.amount.ToString());
        }

        public virtual Sprite GetSprite()
        {
            return _disturbanceAsset.sprite;
        }
    }
}
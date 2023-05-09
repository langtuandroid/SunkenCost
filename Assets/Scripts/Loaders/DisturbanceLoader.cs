using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disturbances
{
    public class DisturbanceLoader : MonoBehaviour
    {
        private static DisturbanceLoader _current;
        private readonly Dictionary<DisturbanceType, DisturbanceAsset> _disturbances = new Dictionary<DisturbanceType, DisturbanceAsset>();

        private void Awake()
        {
            if (_current)
            {
                Destroy(gameObject);
                return;
            }
        
            _current = this;
        }
        
        private void Start()
        {
            LoadDisturbanceAssets();
        }

        public static void LoadDisturbanceAssets()
        {
            _current._disturbances.Clear();
            var disturbances = Extensions.LoadScriptableObjects<DisturbanceAsset>();

            foreach (var disturbance in disturbances)
            {
                _current._disturbances.Add(disturbance.disturbanceType, disturbance);
            }
        }

        public static DisturbanceAsset GetDisturbance(DisturbanceType disturbanceType)
        {
            return _current._disturbances[disturbanceType];
        }

        public static void ExecuteEndOfBattleDisturbanceAction(Disturbance disturbance)
        {
            //TODO: FIX THIS

            switch (disturbance.DisturbanceType)
            {
                case DisturbanceType.GoldRush:
                    RunProgress.PlayerStats.Gold += disturbance.Modifier;
                    break;
                case DisturbanceType.Heart:
                    RunProgress.PlayerStats.Heal(disturbance.Modifier);
                    break;
                case DisturbanceType.None:
                    break;
                case DisturbanceType.UpgradeCard:
                    break;
                case DisturbanceType.Card:
                    case DisturbanceType.EliteCard:
                        if (!(disturbance is CardDisturbance cardDisturbance)) throw new Exception();
                        var rewardCard = cardDisturbance.Design;
                        rewardCard.SetCost(0);
                        RunProgress.OfferStorage.RewardDesignOffers.Add(cardDisturbance.Design);
                    break;
                case DisturbanceType.Item:
                    case DisturbanceType.EliteItem:
                        if (!(disturbance is ItemDisturbance itemDisturbance)) throw new Exception();
                        RunProgress.ItemInventory.AddItem(itemDisturbance.ItemInstance);
                        break;
                case DisturbanceType.MaxHealth:
                    RunProgress.PlayerStats.MaxHealth += disturbance.Modifier;
                    RunProgress.PlayerStats.Heal(disturbance.Modifier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static void ModifyDisturbanceAsset(DisturbanceType disturbanceType, int amount)
        {
            var disturbance = GetDisturbance(disturbanceType);

            disturbance.ModifyAmount(amount);
        }
    }
}

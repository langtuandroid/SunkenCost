using System;
using System.Collections.Generic;
using System.Linq;
using Designs;
using Items;
using Pickups.Varnishes;
using UnityEngine;

[CreateAssetMenu(fileName = "RunthroughStartingConfig", menuName = "RunthroughStartingConfig", order = 0)]
public class RunthroughStartingConfig : ScriptableObject
{
    [field: SerializeField] public bool IsActive { get; private set; }

    [field: Header("Starting Player Stats")]
    [field: SerializeField] public int MaxPlanks { get; private set; } = 3;
    [field: SerializeField] public int MaxHealth { get; private set; } = 3;
    [field: SerializeField] public int StartingGold { get; private set; } = 10;
    [field: SerializeField] public int? MovesPerTurn { get; private set; } = null;
    [field: SerializeField] public int TurnsPerBattle { get; private set; } = 5;
    [field: SerializeField] public int DesignOffersPerBattle { get; private set; } = 3;
    [field: SerializeField] public int ItemOffersPerBattle { get; private set; } = 0;
    [field: SerializeField] public int RewardOffersPerBattle { get; set; } = 3;
    [field: SerializeField] public int ReRollCost { get; private set; } = 1;
    
    [Space(25)]
    
    [SerializeField] private StartingDesign[] _startingDeck;
    [SerializeField] private ItemAsset[] _startingItems;

    [field: SerializeField] public EnemyType[] StartingEnemies { get; private set; }

    public List<Design> GetStartingDesigns()
    {
        return _startingDeck.Select(d => d.Instantiate()).ToList();
    }
    
    public IEnumerable<ItemInstance> GetStartingItems()
    {
        var matchingAssets = _startingItems.Select(ia => 
            ItemLoader.ItemAssetToTypeDict.FirstOrDefault(i => i.Key.title == ia.title).Key);

        return matchingAssets.Select(ia => new ItemInstance(ia, ia.modifier));
    }

    [Serializable]
    private class StartingDesign
    {
        [SerializeField] private DesignAsset _designAsset;
        [SerializeField] private int _level;
        [SerializeField] private List<VarnishType> _varnishes = new List<VarnishType>();
        
        public Design Instantiate()
        {
            var matchingAsset = DesignLoader.AllDesignAssets.FirstOrDefault
                (da => da.title == _designAsset.title);

            var design = DesignFactory.InstantiateDesign(matchingAsset, _level);
            
            foreach (var varnishType in _varnishes)
            {
                design.AddVarnish(varnishType);
            }

            return design;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pickups;
using Pickups.Varnishes;
using UnityEngine;
using Varnishes;

namespace Loaders
{
    public class VarnishLoader : MonoBehaviour
    {
        private static VarnishLoader _current;
        
        public static ReadOnlyDictionary<VarnishAsset, Type> VarnishAssetToTypeDict;
        public static ReadOnlyCollection<VarnishAsset> ShopVarnishAssets;
        public static ReadOnlyCollection<VarnishAsset> EliteVarnishAssets;
        
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
            // Add all the Varnish classes to a dictionary with their class name as the key
            var varnishClasses = new Dictionary<string, Type>();
            var varnishEnumerable = Extensions.GetAllChildrenOfClassOrNull<Varnish>();
            foreach (var type in varnishEnumerable)
            {
                // Remove "Varnish" from the end of the class name
                var varnishName = type.Name.Remove(type.Name.Length - 7, 7);
                varnishClasses.Add(varnishName, type);
            }
        
            // Add all the VarnishAssets to another dictionary - their names must match!
            var varnishAssetsEnumerable = Extensions.LoadScriptableObjects<VarnishAsset>();
            var varnishDictionary = new Dictionary<VarnishAsset, Type>();
            foreach (var varnishAsset in varnishAssetsEnumerable)
            {
                var (key, value) = varnishClasses.FirstOrDefault
                    (i => i.Key == varnishAsset.name);

                if (key == null)
                {
                    Debug.Log("ERROR: Couldn't find a varnish class called " + varnishAsset.name);
                    return;
                }
            
                varnishDictionary.Add(varnishAsset, value);
            }
        
            VarnishAssetToTypeDict = new ReadOnlyDictionary<VarnishAsset, Type>(varnishDictionary);

            var allVarnishAssets = VarnishAssetToTypeDict.Select(kvp => kvp.Key).ToArray();
            ShopVarnishAssets = allVarnishAssets.GetReadonlyCollection
                (va => va.rarity != Rarity.ElitePickup);

            EliteVarnishAssets  = allVarnishAssets.GetReadonlyCollection
                (va => va.rarity == Rarity.ElitePickup);
        }

        public static Type GetVarnishByTypeEnum(VarnishType varnishType)
        {
            return VarnishAssetToTypeDict.First(kvp => kvp.Key.varnishType == varnishType).Value;
        }
    }
}
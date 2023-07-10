using Items;
using Pickups;
using Pickups.Varnishes;
using UnityEngine;

namespace Varnishes
{
    [CreateAssetMenu(menuName = "Varnish")]
    public class VarnishAsset: RarityPickupAsset
    {
        public VarnishType varnishType;
    }
}
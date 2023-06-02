using Items;
using Pickups;
using Pickups.Varnishes;
using UnityEngine;

namespace Varnishes
{
    [CreateAssetMenu(menuName = "Varnish")]
    public class VarnishAsset: PickupAsset
    {
        public VarnishType varnishType;
    }
}
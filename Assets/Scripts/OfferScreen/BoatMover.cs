using UnityEngine;

namespace OfferScreen
{
    public class BoatMover : MonoBehaviour
    {
        [SerializeField] private Transform boatTransform;

        public void OnTransformChildrenChanged()
        {
            var childCount = transform.childCount;

            var boatPosition = boatTransform.localPosition;
            boatPosition = childCount < 5 
                ? new Vector3(230 * childCount - 550, boatPosition.y, 0) 
                : new Vector3(550, boatPosition.y, 0);
            boatTransform.localPosition = boatPosition;
        }
    }
}
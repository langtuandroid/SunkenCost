using System;
using UnityEngine;

namespace BattleScreen.BattleBoard
{
    public class BoardPositioner : MonoBehaviour
    {
        [SerializeField] private RectTransform _island;
        [SerializeField] private RectTransform _boat;

        private void Start()
        {
            UpdatePositions();
        }

        private void OnTransformChildrenChanged()
        {
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            var islandLocalPosition = _island.localPosition;
            var islandDiameter = _island.rect.width;
            var newIslandPosition =
                new Vector3(-Board.Current.BoardSize / 2f - islandDiameter / 2, islandLocalPosition.y, 0);
            _island.localPosition = newIslandPosition;
            
            var boatLocalPosition = _boat.localPosition;
            var boatWidth = _boat.rect.width;
            var newBoatPosition = 
                new Vector3(Board.Current.BoardSize / 2f + boatWidth / 2, boatLocalPosition.y,0);
            _boat.localPosition = newBoatPosition;
        }
    }
}
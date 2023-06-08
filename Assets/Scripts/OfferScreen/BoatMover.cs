using System;
using System.Collections;
using System.Collections.Generic;
using ReorderableContent;
using UnityEngine;

namespace OfferScreen
{
    public class BoatMover : MonoBehaviour
    {
        [SerializeField] private Transform boatTransform;
        [SerializeField] private ReorderableGrid _reorderableGrid;

        private void Start()
        {
            _reorderableGrid.AddRefreshListener(Refresh);
        }

        public void Refresh(List<ReorderableElement> elements)
        {
            var childCount = transform.childCount;
            
            var boatPosition = boatTransform.localPosition;
            boatPosition = childCount < 5 
                ? new Vector3(125 * childCount, boatPosition.y, 0) 
                : new Vector3(615, boatPosition.y, 0);

            LeanTween.cancel(boatTransform.gameObject);
            LeanTween.moveLocal(boatTransform.gameObject, boatPosition, 0.1f);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventGenerator : MonoBehaviour
{
    [SerializeField] private List<Sprite> _mapEventSprites;
    /*
     * 0 : Coin
     * 1 : Heal
     * 2: Upgrade Card
     */

    [SerializeField] private GameObject _mapEventPrefab;

    [SerializeField] private Transform _mapEventsTransform;

    private void Start()
    {
        for (var i = -1; i <= 1; i++)
        {
            var mapEvent = Instantiate(_mapEventPrefab, _mapEventsTransform);
            mapEvent.transform.localPosition = new Vector3(0, 350 * i, 0);
        }
    }
}

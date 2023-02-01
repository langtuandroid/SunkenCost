using System;
using System.Collections;
using System.Collections.Generic;
using MapScreen;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapEventGenerator : MonoBehaviour
{
    [SerializeField] private List<Sprite> _mapEventSprites;
    /*
     * 0 : Coin
     * 1 : Heal
     * 2: Upgrade Card
     * 3: Specific Card
     */

    [SerializeField] private GameObject _mapEventPrefab;

    [SerializeField] private Transform _mapEventsTransform;
    
    
    private static readonly Dictionary<MapEventType, float> Weightings = new Dictionary<MapEventType, float>()
    {
        {MapEventType.Coin, 0.4f},
        {MapEventType.Heart, 0.3f},
        {MapEventType.UpgradeCard, 0.2f},
        {MapEventType.SpecificCard, 0.1f}
    };

    private void Start()
    {
        for (var i = -1; i <= 1; i++)
        {
            var mapEvent = Instantiate(_mapEventPrefab, _mapEventsTransform);
            mapEvent.transform.localPosition = new Vector3(0, 350 * i, 0);

            var eventType = GenerateMapEvent();
            mapEvent.GetComponent<MapEvent>().EventType = eventType;

            var eventImage = mapEvent.transform.GetChild(0).GetChild(0).GetComponent<Image>();

            switch (eventType)
            {
                case MapEventType.Coin:
                    eventImage.sprite = _mapEventSprites[0];
                    break;
                case MapEventType.Heart:
                    eventImage.sprite = _mapEventSprites[1];
                    break;
                case MapEventType.UpgradeCard:
                    eventImage.sprite = _mapEventSprites[2];
                    break;
                case MapEventType.SpecificCard:
                    eventImage.sprite = _mapEventSprites[3];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private MapEventType GenerateMapEvent()
    {
        var sequence = new[] {
            MapEventType.Coin,
            MapEventType.Heart,
            MapEventType.UpgradeCard,
            MapEventType.SpecificCard
        };
        
        var rand = Random.value;

        foreach (var mapEvent in sequence)
        {
            var weighting = Weightings[mapEvent];
                
            if (rand <= weighting)
                return mapEvent;

            rand -= weighting;
        }

        Debug.Log("Error: no weighting found for " + rand);
        return MapEventType.None;
    }
}

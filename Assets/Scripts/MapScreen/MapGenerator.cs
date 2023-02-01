using System.Collections.Generic;
using UnityEngine;
/*
namespace MapScreen
{
    public static class MapGenerator
    {
        private static readonly Dictionary<OldMapEventType, float> Weightings = new Dictionary<OldMapEventType, float>()
        {
            {OldMapEventType.Battle, 0.5f},
            {OldMapEventType.EliteBattle, 0.08f},
            {OldMapEventType.Shop, 0.17f},
            {OldMapEventType.Mystery, 0.25f}
        };

        private const float ChanceOfNoEvent = 0.3f;

        public static Map GenerateNewMap()
        {
            var map = new Map();

            for (var x = 0; x < Map.SizeX; x++)
            {
                for (var y = 0; y < Map.SizeY; y++)
                {
                    if (map.GetMapEvent(x, y) != OldMapEventType.None)
                    {
                        Debug.Log("Already map event on [" + x + ", " + y + "]");
                    }
                    else
                    {
                        map.SetMapEvent(x, y, GenerateEvent(map, x, y));
                    }
                    
                    Debug.Log(map.GetMapEvent(x, y));
                }
            }

            return map;
        }

        private static OldMapEventType GenerateEvent(Map map, int x, int y)
        {
            var rand = Random.value;

            if (rand <= ChanceOfNoEvent)
                return OldMapEventType.None;

            if (
                map.GetMapEvent(Mathf.Clamp(x - 1, 0, Map.SizeX - 1), y) != OldMapEventType.None ||
                map.GetMapEvent(Mathf.Clamp(x + 1, 0, Map.SizeX - 1), y) != OldMapEventType.None ||
                map.GetMapEvent(x, Mathf.Clamp(y - 1, 0, Map.SizeY - 1)) != OldMapEventType.None ||
                map.GetMapEvent(x, Mathf.Clamp(y + 1, 0, Map.SizeY - 1)) != OldMapEventType.None
            )
            {
                return OldMapEventType.None;
            }

            rand = Random.value;

            var sequence = new[] {
                OldMapEventType.Battle,
                OldMapEventType.EliteBattle,
                OldMapEventType.Shop,
                OldMapEventType.Mystery
            };

            foreach (var mapEvent in sequence)
            {
                var weighting = Weightings[mapEvent];
                
                if (rand <= weighting)
                    return mapEvent;

                rand -= weighting;
            }

            Debug.Log("Error: no weighting found for " + rand);
            return OldMapEventType.None;
        }
    }
}
*/
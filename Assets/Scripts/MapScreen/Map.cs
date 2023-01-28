using System.Drawing;
using UnityEngine;

namespace MapScreen
{
    public enum MapEvent
    {
        None,
        Battle,
        EliteBattle,
        Booty,
        Shop,
        Mystery
    }
    
    public class Map
    {
        public const int SizeX = 10;
        public const int SizeY = 6;
        
        // The map itself
        private MapEvent[,] _map = new MapEvent[SizeX, SizeY];

        public Map()
        {
            for (var x = 0; x < SizeX; x++)
            {
                for (var y = 0; y < SizeY; y++)
                {
                    SetMapEvent(x, y, MapEvent.None);
                }
            }
        }
        
        public void SetMapEvent(int positionX, int positionY, MapEvent mapEvent)
        {
            if (positionX >= SizeX || positionY >= SizeY)
            {
                Debug.Log("Cannot set grid, position is out of bounds. [" + positionX + ", " + positionY + "]");
                return;
            }
            
            _map[positionX, positionY] = mapEvent;
        }

        public MapEvent GetMapEvent(int positionX, int positionY)
        {
            if (positionX >= SizeX || positionY >= SizeY)
            {
                Debug.Log("Cannot get event, position is out of bounds: [" + positionX + ", " + positionY + "]");
                return MapEvent.None;
            }
            
            return _map[positionX, positionY];
        }
    }
}

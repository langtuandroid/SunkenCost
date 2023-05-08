using UnityEngine;

namespace Loaders
{
    public class PersistantSingletonsContainer : MonoBehaviour
    {
        private static PersistantSingletonsContainer _current;
    
        private void Awake()
        {
            if (_current)
            {
                Destroy(gameObject);
                return;
            }

            _current = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

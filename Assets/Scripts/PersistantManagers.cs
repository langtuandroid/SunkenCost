using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantManagers : MonoBehaviour
{
    private static PersistantManagers _current;
    
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

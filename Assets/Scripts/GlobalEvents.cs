using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents current;

    public event Action OnVolumeChange;
    public event Action OnLoadedLevel;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        DontDestroyOnLoad(gameObject);
    }

    public void VolumeChanged()
    {
        OnVolumeChange?.Invoke();
    }

    public void LoadedLevel()
    {
        OnLoadedLevel?.Invoke();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Debug = UnityEngine.Debug;

public class Music : MonoBehaviour
{
    public static Music current;
    private StudioEventEmitter _eventEmitter;
    private Bus _musicBus;
    private Bus _sfxBus;
    
    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        _eventEmitter = GetComponent<StudioEventEmitter>();

        FMODUnity.RuntimeManager.StudioSystem.getBus("{df92f17a-9369-4f29-a0fb-b7f6a6a348c0}", out _musicBus);
        FMODUnity.RuntimeManager.StudioSystem.getBus("{81666bcf-891e-46de-9680-c1de59ccce82}", out _sfxBus);
    }

    public void SelectSong(int song)
    {
        _eventEmitter.SetParameter("SongSelect", song);
    }

    public void Play()
    {
        _eventEmitter.Play();
    }

    public void SetSfxVolume(float volume)
    {
        _sfxBus.setVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        _musicBus.setVolume(volume);
    }
}

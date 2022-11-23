using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public float MusicVolume { get; private set; } = 0.5f;
    public float SfxVolume { get; private set; } = 0.5f;

    public static MusicManager current;

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

    private void Start()
    {
        Music.current.SelectSong(0);
        Music.current.SetMusicVolume(MusicVolume);
        Music.current.SetSfxVolume(SfxVolume);
        Music.current.Play();
    }

    public void MusicVolumeChange(float value)
    {
        MusicVolume = value;
        Music.current.SetMusicVolume(MusicVolume);
    }
    
    public void SfxVolumeChange(float value)
    {
        SfxVolume = value;
        Music.current.SetSfxVolume(SfxVolume);
    }
}

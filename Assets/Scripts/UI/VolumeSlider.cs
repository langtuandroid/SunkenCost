using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    
    void Start()
    {
        musicSlider.value = MusicManager.current.MusicVolume;
        sfxSlider.value = MusicManager.current.SfxVolume;
    }

    public void MusicSliderValueChanged(float value)
    {
        MusicManager.current.MusicVolumeChange(value);
    }
    
    public void SfxSliderValueChanged(float value)
    {
        MusicManager.current.SfxVolumeChange(value);
    }
}

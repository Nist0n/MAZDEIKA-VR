using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSfxVolume();
        }
    }

    private void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", MathF.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void SetSfxVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", MathF.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        musicSlider.value = musicVolume;

        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        SFXSlider.value = sfxVolume;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    public List<Sound> music, sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("Music 1");
    }

    public void PlayMusic(string soundName)
    {
        Sound s = music.Find(x => x.name == soundName);

        if (s != null)
        {
            musicSource.clip = s.audio;
            musicSource.Play();
        }
    }

    public void PlaySFX(string soundName)
    {
        Sound s = sounds.Find(x => x.name == soundName);

        if (s != null)
        {
            sfxSource.PlayOneShot(s.audio);
        }
    }
}

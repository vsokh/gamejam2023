using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects instance;

    public AudioSource src;
    public AudioSource disconnectSrc;

    public void Play()
    {
        src.Play();
    }
    public void DisconnectPlay()
    {
        disconnectSrc.Play();
    }
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
}

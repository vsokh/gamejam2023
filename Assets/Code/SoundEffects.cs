using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects instance;

    public AudioSource src;
    public AudioSource disconnectSrc;
    public AudioSource closeSrc;

    public int scale = 0;

    public bool showTutor = true;

    void Update()
    {
        Slider slider = FindObjectOfType<Slider>();
        if (slider != null) scale = Mathf.CeilToInt(slider.value);
    }

    public void Play()
    {
        src.Play();
    }
    public void DisconnectPlay()
    {
        disconnectSrc.Play();
    }

    public void ClosePlay()
    {
        closeSrc.Play();
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

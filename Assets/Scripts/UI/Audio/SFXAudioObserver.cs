using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISFXVolumeObserver
{
    void OnSFXVolumeChanged(float newSFXVolume);
}

[RequireComponent(typeof(AudioSource))]
public class SFXAudioObserver : MonoBehaviour, ISFXVolumeObserver
{
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        GameManager.Instance.RegisterSFXVolumeObserver(this);
    }

    public void OnSFXVolumeChanged(float newSFXVolume)
    {
        audioSrc.volume = newSFXVolume;
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnregisterSFXVolumeObserver(this);
    }
}

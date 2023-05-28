using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBGMVolumeObserver
{
    void OnBGMVolumeChanged(float newBGMVolume);
}

[RequireComponent(typeof(AudioSource))]
public class BGMAudioObserver : MonoBehaviour, IBGMVolumeObserver
{
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        GameManager.Instance.RegisterBGMVolumeObserver(this);
        OnBGMVolumeChanged(GameManager.Instance.BGMVolume);
    }

    public void OnBGMVolumeChanged(float newBGMVolume)
    {
        audioSrc.volume = newBGMVolume;
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnregisterBGMVolumeObserver(this);
    }
}

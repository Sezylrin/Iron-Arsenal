using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSounds : MonoBehaviour
{
    [SerializeField] private AudioClip basicWeapon;
    [SerializeField] private AudioClip shotgun;
    [SerializeField] private AudioClip flamethrower;
    [SerializeField] private AudioClip rocketLauncher;

    private AudioSource flamethrowerAudioSource;

    private void Awake()
    {
        flamethrowerAudioSource = gameObject.AddComponent<AudioSource>();
        flamethrowerAudioSource.clip = flamethrower;
        flamethrowerAudioSource.loop = true;
    }

    public void ShootWeapon(int weaponType)
    {
        switch (weaponType)
        {
            case 0:
            case 2:
            case 3:
            case 4:
                PlaySound(basicWeapon);
                StopFlamethrowerIfPlaying();
                break;
            case 1:
                PlaySound(shotgun);
                StopFlamethrowerIfPlaying();
                break;
            case 5:
                PlaySound(rocketLauncher);
                StopFlamethrowerIfPlaying();
                break;
            case 6:
                StartFlamethrowerIfNotPlaying();
                break;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        GameObject audioSourceGameObject = new GameObject("AudioSource_" + clip.name);
        audioSourceGameObject.transform.SetParent(this.transform);

        AudioSource audioSource = audioSourceGameObject.AddComponent<AudioSource>();
        SFXAudioObserver sfxAudioObserver = audioSourceGameObject.AddComponent<SFXAudioObserver>();

        audioSource.clip = clip;
        audioSource.Play();

        Destroy(audioSourceGameObject, clip.length);
    }

    private void StartFlamethrowerIfNotPlaying()
    {
        if (!flamethrowerAudioSource.isPlaying)
        {
            flamethrowerAudioSource.Play();
        }
    }

    private void StopFlamethrowerIfPlaying()
    {
        if (flamethrowerAudioSource.isPlaying)
        {
            flamethrowerAudioSource.Stop();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SFXAudioObserver))]
public class UIClick : MonoBehaviour
{
    private AudioSource audioSrc;
    private Button btn;

    [SerializeField] private AudioClip clickSound;

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
        audioSrc.clip = clickSound;
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        audioSrc.Play();
    }
}

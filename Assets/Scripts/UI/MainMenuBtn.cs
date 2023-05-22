using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class MainMenuBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image btnImage;
    private AudioSource audioSrc;
    private Button btn;

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private bool playClickSound = true;

    private void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(HandleClickSound);
        audioSrc = GetComponent<AudioSource>();
        btnImage = GetComponent<Image>();
        btnImage.color = new Color(17f / 255f, 64f / 255f, 94f / 255f, 0f); ;
    }

    private void HandleClickSound()
    {
        if (!playClickSound) return;
        audioSrc.clip = clickSound;
        audioSrc.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSrc.clip = hoverSound;
        audioSrc.Play();
        Color newColor = btnImage.color;
        newColor.a = 1f;
        btnImage.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color newColor = btnImage.color;
        newColor.a = 0f;
        btnImage.color = newColor;
    }
}

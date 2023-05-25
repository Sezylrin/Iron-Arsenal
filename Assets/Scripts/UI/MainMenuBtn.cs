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
    [SerializeField] private ButtonAction action;

    public enum ButtonAction
    {
        PlayGame,
        QuitGame,
        PauseGame,
        ResumeGame,
        DisplaySettings,
        LoadMainMenu,
    }

    private void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(HandleClick);
        audioSrc = GetComponent<AudioSource>();
        btnImage = GetComponent<Image>();
        btnImage.color = new Color(17f / 255f, 64f / 255f, 94f / 255f, 0f);
        audioSrc.clip = hoverSound;
    }
    
    private void HandleClick()
    {
        GameManager.Instance.PlayClickSound();
        switch (action)
        {
            case ButtonAction.PlayGame:
                GameManager.Instance.PlayGame();
                break;
            case ButtonAction.QuitGame:
                GameManager.Instance.QuitGame();
                break;
            case ButtonAction.PauseGame:
                GameManager.Instance.PauseGame();
                break;
            case ButtonAction.ResumeGame:
                GameManager.Instance.ResumeGame();
                break;
            case ButtonAction.DisplaySettings:
                GameManager.Instance.HandleDisplaySettings();
                break;
            case ButtonAction.LoadMainMenu:
                GameManager.Instance.LoadMainMenu();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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

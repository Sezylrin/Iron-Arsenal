using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image btnImage;

    private void Start()
    {
        btnImage = GetComponent<Image>();
        btnImage.color = new Color(0.36f, 0.38f, 1f, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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

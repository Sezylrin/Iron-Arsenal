using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [SerializeField] private Sprite activeImg;
    [SerializeField] private Sprite inactiveImg;

    public void ClickTab()
    {
        gameObject.GetComponent<Image>().sprite = activeImg;
    }

    public void DeactivateTab()
    {
        gameObject.GetComponent<Image>().sprite = inactiveImg;
    }
}

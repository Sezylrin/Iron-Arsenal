using UnityEngine;
using UnityEngine.UI;

public class GameManagerClick : MonoBehaviour
{
    private Button btn;

    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        GameManager.Instance.PlayClickSound();
    }
}

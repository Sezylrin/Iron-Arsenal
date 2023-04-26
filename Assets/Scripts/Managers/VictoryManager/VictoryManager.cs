using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private Button playAgainBtn;

    private void Start()
    {
        playAgainBtn.onClick.AddListener(HandlePlayAgain);
    }

    private void HandlePlayAgain()
    {
        Loader.Load(Scene.Game);
    }
}

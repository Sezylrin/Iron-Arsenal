using UnityEngine.SceneManagement;
public enum SceneState
{
    MainMenu,
    Game,
    Defeat,
    Victory,
}

public static class Loader
{
    public static void Load(SceneState scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
    }
}
using UnityEngine.SceneManagement;
public enum SceneState
{
    MainMenu,
    Game,
    Defeat,
    Victory,
    Tutorial,
}

public static class Loader
{
    public static void Load(SceneState scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
    }
}
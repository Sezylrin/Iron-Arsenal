using UnityEngine.SceneManagement;
public enum Scene
{
    MainMenu,
    Game,
    Defeat,
    Victory,
}

public static class Loader
{
    public static void Load(Scene scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
    }
}
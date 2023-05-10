using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrentSelection
{
    Playing,
    Paused,
    Settings
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    [Header("SFX Bars")]
    [SerializeField] private GameObject SFX20;
    [SerializeField] private GameObject SFX40;
    [SerializeField] private GameObject SFX60;
    [SerializeField] private GameObject SFX80;
    [SerializeField] private GameObject SFX100;

    [Header("BGM Bars")]
    [SerializeField] private GameObject BGM20;
    [SerializeField] private GameObject BGM40;
    [SerializeField] private GameObject BGM60;
    [SerializeField] private GameObject BGM80;
    [SerializeField] private GameObject BGM100;

    [Header("Volume")]
    public int SFXVolume = 100;
    public int BGMVolume = 100;
    
    private LevelManager levelManager;
    public CurrentSelection currentSelection = CurrentSelection.Playing;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (LevelManager.Instance != null)
        {
            levelManager = LevelManager.Instance;
        }
    }

    private void Update()
    {
        if (currentSelection != CurrentSelection.Playing)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentSelection == CurrentSelection.Playing)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != SceneState.Game.ToString()) return;
            pauseMenu.SetActive(true);
            currentSelection = CurrentSelection.Paused;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && currentSelection == CurrentSelection.Paused)
        {
            currentSelection = CurrentSelection.Playing;
            pauseMenu.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentSelection == CurrentSelection.Settings)
        {
            HandleExitSettings();
        }
    }

    public void PlayGame()
    {
        currentSelection = CurrentSelection.Playing;
        Loader.Load(SceneState.Game);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void HandleResume()
    {
        currentSelection = CurrentSelection.Playing;
        pauseMenu.SetActive(false);
    }

    public void HandleDisplaySettings()
    {
        settingsMenu.SetActive(false);
        currentSelection = CurrentSelection.Settings;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void HandleQuit()
    {
        pauseMenu.SetActive(false);
        Loader.Load(SceneState.MainMenu);
        DestroyImmediate(this);
    }

    public void IncreaseSFX()
    {
        if (SFXVolume < 100)
        {
            SFXVolume += 20;
            if (SFXVolume > 100)
            {
                SFXVolume = 100;
            }
            UpdateSFXBars();
        }
    }

    public void DecreaseSFX()
    {
        if (SFXVolume > 0)
        {
            SFXVolume -= 20;
            if (SFXVolume < 0)
            {
                SFXVolume = 0;
            }
            UpdateSFXBars();
        }
    }

    public void IncreaseBGM()
    {
        if (BGMVolume < 100)
        {
            BGMVolume += 20;
            if (BGMVolume > 100)
            {
                BGMVolume = 100;
            }
            UpdateBGMBars();
        }
    }

    public void DecreaseBGM()
    {
        if (BGMVolume > 0)
        {
            BGMVolume -= 20;
            if (BGMVolume < 0)
            {
                BGMVolume = 0;
            }
            UpdateBGMBars();
        }
    }

    private void UpdateSFXBars()
    {
        SFX20.SetActive(SFXVolume >= 20);
        SFX40.SetActive(SFXVolume >= 40);
        SFX60.SetActive(SFXVolume >= 60);
        SFX80.SetActive(SFXVolume >= 80);
        SFX100.SetActive(SFXVolume >= 100);
    }

    private void UpdateBGMBars()
    {
        BGM20.SetActive(BGMVolume >= 20);
        BGM40.SetActive(BGMVolume >= 40);
        BGM60.SetActive(BGMVolume >= 60);
        BGM80.SetActive(BGMVolume >= 80);
        BGM100.SetActive(BGMVolume >= 100);
    }

    public void HandleSaveSettings()
    {
        currentSelection = CurrentSelection.Paused;
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void HandleExitSettings()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        settingsMenu.SetActive(false);
        if (currentScene.name == SceneState.Game.ToString())
        {
            currentSelection = CurrentSelection.Paused;
            pauseMenu.SetActive(true);
        }
    }

    public void HandleDeath()
    {
        Loader.Load(SceneState.Defeat);
    }

    public void HandleVictory()
    {
        Loader.Load(SceneState.Victory);
    }
}

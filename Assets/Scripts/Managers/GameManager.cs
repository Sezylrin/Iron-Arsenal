using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

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

    [Header("Sound")]
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSrc;
    private float _sfxVolume = 1f;
    public float SFXVolume
    {
        get { return _sfxVolume; }
        set
        {
            _sfxVolume = value;
            NotifySFXVolumeObservers();
        }
    }

    private float _bgmVolume = 1f;
    public float BGMVolume
    {
        get { return _bgmVolume; }
        set
        {
            _bgmVolume = value;
            NotifyBGMVolumeObservers();
        }
    }

    private List<ISFXVolumeObserver> sfxVolumeObservers = new List<ISFXVolumeObserver>();
    private List<IBGMVolumeObserver> bgmVolumeObservers = new List<IBGMVolumeObserver>();

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
    }

    private void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
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
            levelManager.PlayPauseSound();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && currentSelection == CurrentSelection.Paused)
        {
            currentSelection = CurrentSelection.Playing;
            pauseMenu.SetActive(false);
            levelManager.PlayGameSound();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentSelection == CurrentSelection.Settings)
        {
            HandleExitSettings();
        }
    }

    public void LoadMainMenu()
    {
        Loader.Load(SceneState.MainMenu);
    }

    public void PlayClickSound()
    {
        audioSrc.clip = clickSound;
        audioSrc.Play();
    }

    public void PlayGame()
    {
        currentSelection = CurrentSelection.Playing;
        StartCoroutine(LoadPlayScene());
    }

    IEnumerator LoadPlayScene()
    {
        yield return new WaitForSeconds(0.5f);
        Loader.Load(SceneState.Game);
    }

    public void QuitGame()
    {
        StartCoroutine(LoadQuitGame());
    }

    IEnumerator LoadQuitGame()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void PauseGame()
    {
        currentSelection = CurrentSelection.Paused;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        currentSelection = CurrentSelection.Playing;
        Time.timeScale = 1;
    }

    public void HandleResume()
    {
        currentSelection = CurrentSelection.Playing;
        pauseMenu.SetActive(false);
        levelManager.PlayGameSound();
    }

    public void HandleDisplaySettings()
    {
        currentSelection = CurrentSelection.Settings;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void HandleQuit()
    {
        pauseMenu.SetActive(false);
        Loader.Load(SceneState.MainMenu);
    }

    public void IncreaseSFX()
    {
        if (SFXVolume < 1f)
        {
            SFXVolume += 0.2f;
            if (SFXVolume > 1f)
            {
                SFXVolume = 1f;
            }
            UpdateSFXBars();
        }
    }

    public void DecreaseSFX()
    {
        if (SFXVolume > 0f)
        {
            SFXVolume -= 0.2f;
            if (SFXVolume < 0f)
            {
                SFXVolume = 0f;
            }
            UpdateSFXBars();
        }
    }

    public void IncreaseBGM()
    {
        if (BGMVolume < 1f)
        {
            BGMVolume += 0.2f;
            if (BGMVolume > 1f)
            {
                BGMVolume = 1f;
            }
            UpdateBGMBars();
        }
    }

    public void DecreaseBGM()
    {
        if (BGMVolume > 0f)
        {
            BGMVolume -= 0.2f;
            if (BGMVolume < 0f)
            {
                BGMVolume = 0f;
            }
            UpdateBGMBars();
        }
    }

    private void UpdateSFXBars()
    {
        SFX20.SetActive(SFXVolume >= 0.2f);
        SFX40.SetActive(SFXVolume >= 0.4f);
        SFX60.SetActive(SFXVolume >= 0.6f);
        SFX80.SetActive(SFXVolume >= 0.8f);
        SFX100.SetActive(SFXVolume >= 1f);
    }

    private void UpdateBGMBars()
    {
        BGM20.SetActive(BGMVolume >= 0.2f);
        BGM40.SetActive(BGMVolume >= 0.4f);
        BGM60.SetActive(BGMVolume >= 0.6f);
        BGM80.SetActive(BGMVolume >= 0.8f);
        BGM100.SetActive(BGMVolume >= 1f);
    }

    public void HandleSaveSettings()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        settingsMenu.SetActive(false);
        if (currentScene.name == SceneState.Game.ToString())
        {
            currentSelection = CurrentSelection.Paused;
            pauseMenu.SetActive(true);
        }
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

    public void RegisterSFXVolumeObserver(ISFXVolumeObserver observer)
    {
        if (!sfxVolumeObservers.Contains(observer))
        {
            sfxVolumeObservers.Add(observer);
        }
    }

    public void UnregisterSFXVolumeObserver(ISFXVolumeObserver observer)
    {
        if (sfxVolumeObservers.Contains(observer))
        {
            sfxVolumeObservers.Remove(observer);
        }
    }

    public void RegisterBGMVolumeObserver(IBGMVolumeObserver observer)
    {
        if (!bgmVolumeObservers.Contains(observer))
        {
            bgmVolumeObservers.Add(observer);
        }
    }

    public void UnregisterBGMVolumeObserver(IBGMVolumeObserver observer)
    {
        if (bgmVolumeObservers.Contains(observer))
        {
            bgmVolumeObservers.Remove(observer);
        }
    }

    private void NotifySFXVolumeObservers()
    {
        audioSrc.volume = SFXVolume;
        foreach (ISFXVolumeObserver observer in sfxVolumeObservers)
        {
            observer.OnSFXVolumeChanged(SFXVolume);
        }
    }

    private void NotifyBGMVolumeObservers()
    {
        foreach (IBGMVolumeObserver observer in bgmVolumeObservers)
        {
            observer.OnBGMVolumeChanged(BGMVolume);
        }
    }
}

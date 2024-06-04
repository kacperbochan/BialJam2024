using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private FMODUnity.EventReference musicEvent;
    private FMOD.Studio.EventInstance musicInstance;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer) StartMusicIfNotStartedYet();

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        OnSceneLoaded();
    }

    private bool musicStarted = false;
    public void StartMusicIfNotStartedYet()
    {
        if (!musicStarted)
        {
            musicStarted = true;
            StartCoroutine(StartMusic());
        }
    }
    private IEnumerator StartMusic()
    {
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    private const string LEVEL_SCENE_NAME = "LevelScene";
    private const string MENU_SCENE_NAME = "MenuScene";
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == LEVEL_SCENE_NAME)
        {
            GoToGame();
        }
        else if (SceneManager.GetActiveScene().name == MENU_SCENE_NAME)
        {
            GoToMenu();
        }
    }

    public void GoToMenu()
    {
        musicInstance.setParameterByName("state", 0);
    }

    public void GoToGame()
    {
        musicInstance.setParameterByName("state", 1);
    }

    public void GoToFinale() //after finale comes menu again
    {
        musicInstance.setParameterByName("state", 2);
    }

    public void GravityFlipOn()
    {
        musicInstance.setParameterByName("gravity", 1f);
    }

    public void GravityFlipOff()
    {
        musicInstance.setParameterByName("gravity", 0f);
    }
}

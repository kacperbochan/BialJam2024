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
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        OnSceneLoaded();
    }


    private const string LEVEL_SCENE_NAME = "LevelScene";
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == LEVEL_SCENE_NAME)
        {
            GoToStart();
        }
    }

    public void GoToStart()
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

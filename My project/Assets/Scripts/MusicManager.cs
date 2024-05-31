using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    // singleton
    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        // MusicManager should be on every scene for testing purposes
        // this code makes it survive scene changes, but if another MusicManager already lives, it kills itself so there's only one at a time
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    [SerializeField] private FMODUnity.EventReference musicEvent;
    private FMOD.Studio.EventInstance musicInstance;

    private void Start()
    {
        // start music
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }


    private const string LEVEL_SCENE_NAME = "LevelScene";
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
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

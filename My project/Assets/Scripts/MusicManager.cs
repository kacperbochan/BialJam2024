using UnityEngine;

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
    }
}

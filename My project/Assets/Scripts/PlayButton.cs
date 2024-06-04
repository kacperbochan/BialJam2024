using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AsyncOperation sceneLoad;
    private const string LEVEL_SCENE_NAME = "LevelScene";

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            Play();
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            Play();
        }
    }

    private void Play()
    {
        animator.SetTrigger("clicked");
        MusicManager.Instance.StartMusicIfNotStartedYet();
        sceneLoad = SceneManager.LoadSceneAsync(LEVEL_SCENE_NAME);
        sceneLoad.allowSceneActivation = false;
    }
    public void SwitchScene()
    {
        sceneLoad.allowSceneActivation = true;
    }
}

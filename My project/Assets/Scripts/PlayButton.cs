using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private AsyncOperation sceneLoad;
    [SerializeField] private PlayButtonImage playButtonImage;
    private const string LEVEL_SCENE_NAME = "LevelScene";

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            GetComponent<Button>().enabled = false;
            Play();
        });
    }

    private void Play()
    {
        playButtonImage.GetComponent<Animator>().SetTrigger("clicked");
        playButtonImage.OnDonePlayButtonAnimation += PlayButtonImage_OnDonePlayButtonAnimation;
        MusicManager.Instance.StartMusicIfNotStartedYet();
        sceneLoad = SceneManager.LoadSceneAsync(LEVEL_SCENE_NAME);
        sceneLoad.allowSceneActivation = false;
    }

    private void PlayButtonImage_OnDonePlayButtonAnimation(object sender, System.EventArgs e)
    {
        SwitchScene();
    }

    public void SwitchScene()
    {
        MusicManager.Instance.GoToGame();
        sceneLoad.allowSceneActivation = true;
    }
}

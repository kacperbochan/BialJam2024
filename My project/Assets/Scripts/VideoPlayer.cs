using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoPlayer : MonoBehaviour
{
    private void Start()
    {
        GetComponent<UnityEngine.Video.VideoPlayer>().loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(UnityEngine.Video.VideoPlayer source)
    {
        SceneManager.LoadScene("CreditsScene");
    }
}

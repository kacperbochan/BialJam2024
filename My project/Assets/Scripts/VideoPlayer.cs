using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoPlayer : MonoBehaviour
{
    private void Start()
    {
        UnityEngine.Video.VideoPlayer videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "ending_animation.mp4");
        videoPlayer.Play();
        GetComponent<UnityEngine.Video.VideoPlayer>().loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(UnityEngine.Video.VideoPlayer source)
    {
        SceneManager.LoadScene("CreditsScene");
    }
}

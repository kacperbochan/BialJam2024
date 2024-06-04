using UnityEngine;

public class PlayButtonImage : MonoBehaviour
{
    [SerializeField] private PlayButton playButton;
    public void StartGame()
    {
        MusicManager.Instance.GoToGame();
        playButton.SwitchScene();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonImage : MonoBehaviour
{
    private const string LEVEL_SCENE_NAME = "LevelScene";
    public void StartGame()
    {
        SceneManager.LoadScene(LEVEL_SCENE_NAME);
    }
}

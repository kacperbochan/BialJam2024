using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button CreditsButton;
    private const string LEVEL_SCENE_NAME = "LevelScene";
    private const string CREDITS_SCENE_NAME = "CreditsScene";

    private void Start()
    {
        PlayButton.onClick.AddListener(() => {
            SceneManager.LoadScene(LEVEL_SCENE_NAME);
        });
        QuitButton.onClick.AddListener(() =>
        {
            Debug.Log("quitting");
            Application.Quit();
        });
        CreditsButton.onClick.AddListener(() => {
            SceneManager.LoadScene(CREDITS_SCENE_NAME);
        });
    }
}

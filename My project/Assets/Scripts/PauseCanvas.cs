using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button QuitButton;

    private void Start()
    {
        ResumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Resume();
        });
        MenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MenuScene");
        });
        QuitButton.onClick.AddListener(() =>
        {
            Debug.Log("quitting");
            Application.Quit();
        });
    }
}

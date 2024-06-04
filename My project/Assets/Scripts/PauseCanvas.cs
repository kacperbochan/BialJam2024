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
        
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QuitButton.gameObject.SetActive(false);
        }
        else
        {
            QuitButton.onClick.AddListener(() =>
            {
                Debug.Log("quitting");
                Application.Quit();
            });
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
        ResumeButton.Select();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

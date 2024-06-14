using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private ConfirmUI confirmUI;

    private void Start()
    {
        ResumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Resume();
        });
        MenuButton.onClick.AddListener(() =>
        {
            Hide();
            confirmUI.Show(() => {
                SceneManager.LoadScene("MenuScene");
            }, () => {
                Show();
            });
        });
        
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QuitButton.gameObject.SetActive(false);
        }
        else
        {
            QuitButton.onClick.AddListener(() =>
            {
                Hide();
                confirmUI.Show(() => {
                    Debug.Log("quitting");
                    Application.Quit();
                }, () => {
                    Show();
                });
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

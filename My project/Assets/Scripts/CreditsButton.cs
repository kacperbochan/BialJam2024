using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    private const string MENU_SCENE_NAME = "MenuScene";
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GoToMenu();
        });
        GetComponent<Button>().Select();
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }
}

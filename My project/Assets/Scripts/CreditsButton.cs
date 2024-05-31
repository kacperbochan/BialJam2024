using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    private const string MENU_SCENE_NAME = "MenuScene";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GoToMenu();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GoToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;
    public void Show(Action yesAction, Action noAction)
    {
        gameObject.SetActive(true);
        YesButton.onClick.AddListener(() =>
        {
            yesAction();
            //Hide(); //since yesAction is menu or quit, we don't need to hide, and last frame of game before menu looks better with a confirmation UI
        });
        NoButton.onClick.AddListener(() =>
        {
            noAction();
            Hide();
        });
        NoButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

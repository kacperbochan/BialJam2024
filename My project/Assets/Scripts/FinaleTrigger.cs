using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinaleTrigger : MonoBehaviour
{
    private readonly HashSet<Collider2D> colliders = new();
    private const int TOTAL_PLAYERS = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);
        if (colliders.Count == TOTAL_PLAYERS)
        {
            StartFinale();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
    }
    private void StartFinale()
    {
        SceneManager.LoadScene("CreditsScene");
        MusicManager.Instance.GoToFinale();
    }
}

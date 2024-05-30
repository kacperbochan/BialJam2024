using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private readonly HashSet<Collider2D> colliders = new();
    private const int TOTAL_PLAYERS = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);
        if (colliders.Count == TOTAL_PLAYERS)
        {
            NextLevel();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
    }
    private void NextLevel()
    {
        CameraManager.Instance.NextStage();
        gameObject.SetActive(false);
    }
}

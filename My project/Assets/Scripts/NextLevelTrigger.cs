using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private Transform wallToDisable;
    private HashSet<Collider2D> colliders;
    private const int TOTAL_PLAYERS = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);
        if (colliders.Count > TOTAL_PLAYERS)
        {

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
    }
    private void NextLevel()
    {

    }
}

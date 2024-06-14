using System;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private readonly HashSet<Collider2D> colliders = new();
    private const int TOTAL_PLAYERS = 2;
    [SerializeField] private int targetLevel = 2;
    public static event EventHandler OnNextLevel;
    private bool triggered = false;
    public static void ResetStaticData()
    {
        OnNextLevel = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);
        if (colliders.Count == TOTAL_PLAYERS && !triggered)
        {
            NextLevel();
            triggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
    }
    private void NextLevel()
    {
        OnNextLevel?.Invoke(this, EventArgs.Empty);
        CameraManager.Instance.GoToStage(targetLevel);
        //gameObject.SetActive(false);
    }
}

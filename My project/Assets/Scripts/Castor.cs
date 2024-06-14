using System;
using UnityEngine;

public class Castor : MonoBehaviour
{
    public static event EventHandler OnCastorCollision;
    private static float lastCastorEvent = Mathf.NegativeInfinity;
    [SerializeField] private float castorEventCooldown = 5.0f;
    public static void ResetStaticData()
    {
        OnCastorCollision = null;
        lastCastorEvent = Mathf.NegativeInfinity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (TimeSince(lastCastorEvent) > castorEventCooldown)
        {
            OnCastorCollision?.Invoke(this, EventArgs.Empty);
            lastCastorEvent = Time.time;
        }
    }
    private float TimeSince(float timePoint)
    {
        return Time.time - timePoint;
    }
}

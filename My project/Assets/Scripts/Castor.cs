using System;
using UnityEngine;

public class Castor : MonoBehaviour
{
    public static event EventHandler OnCastorCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCastorCollision?.Invoke(this, EventArgs.Empty);
    }
}

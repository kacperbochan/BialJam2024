using System;
using UnityEngine;

public class PlayButtonImage : MonoBehaviour
{
    public event EventHandler OnDonePlayButtonAnimation;
    public void DoneAnimation()
    {
        OnDonePlayButtonAnimation?.Invoke(this, EventArgs.Empty);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWithGravity : MonoBehaviour
{
    private void Start()
    {
        Player2.Instance.OnGravityFlip += Player2_OnGravityFlip;
    }

    private void Player2_OnGravityFlip(object sender, System.EventArgs e)
    {
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }
}

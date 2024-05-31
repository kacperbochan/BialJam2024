using UnityEngine;

public class Player1Visual : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Player1.Instance.OnPlayer1Jump += Player1_OnPlayer1Jump;
    }

    private void Player1_OnPlayer1Jump(object sender, System.EventArgs e)
    {
        animator.SetTrigger("Jump");
    }

    private void Update()
    {
        animator.SetFloat("HorizontalSpeed", Player1.Instance.GetComponent<Rigidbody2D>().velocity.x);
        animator.SetFloat("VerticalSpeed", Player1.Instance.GetComponent<Rigidbody2D>().velocity.y);
    }
}

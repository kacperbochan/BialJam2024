using UnityEngine;

public class Player1Visual : MonoBehaviour
{
    [SerializeField] private GameObject artistFire;
    private void Start()
    {
        Player1.Instance.OnPlayer1Jump += Player1_OnPlayer1Jump;
        Player1.Instance.OnPlayer1BurnOn += Player1_OnPlayer1BurnOn;
        Player1.Instance.OnPlayer1BurnOff += Player1_OnPlayer1BurnOff;
    }
    private void Player1_OnPlayer1Jump(object sender, System.EventArgs e)
    {
        GetComponent<Animator>().SetTrigger("Jump");
    }
    private void Player1_OnPlayer1BurnOn(object sender, System.EventArgs e)
    {
        artistFire.SetActive(true);
    }
    private void Player1_OnPlayer1BurnOff(object sender, System.EventArgs e)
    {
        artistFire.SetActive(false);
    }
    private void Update()
    {
        GetComponent<Animator>().SetFloat("HorizontalSpeed", Player1.Instance.GetComponent<Rigidbody2D>().velocity.x);
        GetComponent<Animator>().SetFloat("VerticalSpeed", Player1.Instance.GetComponent<Rigidbody2D>().velocity.y);
    }
}

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference player1JumpEvent;
    [SerializeField] private FMODUnity.EventReference player2JumpEvent;
    [SerializeField] private FMODUnity.EventReference player2CreateEvent;
    [SerializeField] private FMODUnity.EventReference player2GravityFlipEvent;

    private void Start()
    {
        PlayerInput.Instance.OnPlayer1Jump += PlayerInput_OnPlayer1Jump;
        PlayerInput.Instance.OnPlayer2Jump += PlayerInput_OnPlayer2Jump;
        PlayerInput.Instance.OnPlayer2Create += PlayerInput_OnPlayer2Create;
        PlayerInput.Instance.OnPlayer2GravityFlip += PlayerInput_OnPlayer2GravityFlip;
    }


    private void PlayerInput_OnPlayer1Jump(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(player1JumpEvent, transform.position);
    }

    private void PlayerInput_OnPlayer2Jump(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(player2JumpEvent, transform.position);
    }

    private void PlayerInput_OnPlayer2Create(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(player2CreateEvent, transform.position);
    }
    private void PlayerInput_OnPlayer2GravityFlip(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(player2GravityFlipEvent, transform.position);
    }
}

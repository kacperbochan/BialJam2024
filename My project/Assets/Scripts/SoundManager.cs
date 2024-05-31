using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference player1JumpEvent;
    [SerializeField] private FMODUnity.EventReference player1BurnEvent;
    [SerializeField] private FMODUnity.EventReference player2JumpEvent;
    [SerializeField] private FMODUnity.EventReference player2CreateEvent;
    [SerializeField] private FMODUnity.EventReference player2GravityFlipEvent;

    private void Start()
    {
        //TO DO: subscribe events here
    }

    //syntax: FMODUnity.RuntimeManager.PlayOneShot(player1JumpEvent, transform.position);
}

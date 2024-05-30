using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference jumpEvent;

    private void Start()
    {
        PlayerInput.Instance.OnJumpAction += PlyaerInput_OnJumpAction;
    }

    private void PlyaerInput_OnJumpAction(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(jumpEvent, transform.position);
    }
}

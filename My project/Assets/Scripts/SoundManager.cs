using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference burnEvent;
    [SerializeField] private FMODUnity.EventReference createEvent;
    [SerializeField] private FMODUnity.EventReference transitionEvent;
    [SerializeField] private FMODUnity.EventReference castorEvent;

    private void Start()
    {
        Removable.OnAnyBurn += Removable_OnAnyBurn;
        Player2.Instance.OnBuild += Player2_OnBuild;
        NextLevelTrigger.OnNextLevel += NextLevelTrigger_OnNextLevel;
        Castor.OnCastorCollision += Castor_OnCastorCollision;
    }

    private void Castor_OnCastorCollision(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(castorEvent, transform.position);
    }

    private void NextLevelTrigger_OnNextLevel(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(transitionEvent, transform.position);
    }

    private void Player2_OnBuild(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(createEvent, transform.position);
    }

    private void Removable_OnAnyBurn(object sender, System.EventArgs e)
    {
        FMODUnity.RuntimeManager.PlayOneShot(burnEvent, transform.position);
    }

}

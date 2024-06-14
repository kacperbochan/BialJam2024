using UnityEngine;

public class BurnText : MonoBehaviour
{
    private void Start()
    {
        Removable.OnAnyBurn += Removable_OnAnyBurn;
    }
    private void Removable_OnAnyBurn(object sender, System.EventArgs e)
    {
        GetComponent<Animator>().SetTrigger("burn");
    }
}

using System;
using UnityEngine;

public class Removable : MonoBehaviour
{
    public bool built;
    public static event EventHandler OnAnyBurn;
    [SerializeField] private GameObject burnParticle;

    private void Awake()
    {
        gameObject.SetActive(built);
    }

    public void Burn()
    {
        built = false;
        gameObject.SetActive(built);
        OnAnyBurn?.Invoke(this, EventArgs.Empty);

        burnParticle.SetActive(true);
        GameObject burnParticleInst = Instantiate(burnParticle, transform.position, transform.rotation);

        Cascade cascadeParent = gameObject.GetComponentInParent<Cascade>();
        if (cascadeParent != null)
        {
            foreach(Removable removable in cascadeParent.GetComponentsInChildren<Removable>())
            {
                removable.Burn();
            }
        }
    }

    public void Build()
    {
        built = true;
        gameObject.SetActive(built);

        burnParticle.SetActive(true);
    }
}

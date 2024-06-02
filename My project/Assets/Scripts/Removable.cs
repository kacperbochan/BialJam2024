using System;
using System.Collections.Generic;
using UnityEngine;

public class Removable : MonoBehaviour
{
    public bool built;
    public static event EventHandler OnAnyBurn;
    [SerializeField] private GameObject burnParticle;    

    private void Awake()
    {
        //gameObject.SetActive(built);
        gameObject.layer = (built)?LayerMask.NameToLayer("WorldPlatforms"): LayerMask.NameToLayer("IgnoreRaycast");

        GetComponent<BoxCollider2D>().isTrigger = !built;
        GetComponent<SpriteRenderer>().enabled = built;
    }

    public void Burn()
    {
        Cascade cascadeParent = gameObject.GetComponentInParent<Cascade>();
        if (cascadeParent != null)
        {
            foreach(Removable removable in cascadeParent.GetComponentsInChildren<Removable>())
            {
                removable.Disable();
            }
        }
        else
        {
            Disable();
        }
    }

    private void Disable()
    {
        if (built)
        {
            built = false;
            gameObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
            //gameObject.SetActive(built);
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().enabled = false;

            OnAnyBurn?.Invoke(this, EventArgs.Empty);

            burnParticle.SetActive(true);
            GameObject burnParticleInst = Instantiate(burnParticle, transform.position, transform.rotation);
        }
    }

    public void Build()
    {

        built = true;
        gameObject.layer = LayerMask.NameToLayer("WorldPlatforms");


        //gameObject.SetActive(built);
        GetComponent<BoxCollider2D>().isTrigger = false;
        GetComponent<SpriteRenderer>().enabled = true;

        if (colliders.Count > 0) Burn();

        burnParticle.SetActive(false);
    }

    private readonly HashSet<Collider2D> colliders = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Removable : MonoBehaviour
{
    public bool built;
    public static event EventHandler OnAnyBurn;
    [SerializeField] private GameObject burnParticle;
    public static void ResetStaticData()
    {
        OnAnyBurn = null;
    }
    private void Awake()
    {
        //gameObject.SetActive(built);
        gameObject.layer = (built)?LayerMask.NameToLayer("WorldPlatforms"): LayerMask.NameToLayer("Ignore Raycast");

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
        OnAnyBurn?.Invoke(this, EventArgs.Empty);
    }
    private void Disable()
    {
        if (built)
        {
            built = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            //gameObject.SetActive(built);
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().enabled = false;

            burnParticle.SetActive(true);
            Instantiate(burnParticle, transform.position, transform.rotation);
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

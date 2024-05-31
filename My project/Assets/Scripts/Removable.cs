using UnityEngine;

public class Removable : MonoBehaviour
{
    public bool built;

    private void Awake()
    {
        gameObject.SetActive(built);
    }

    public void Burn()
    {
        built = false;
        gameObject.SetActive(built);

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
    }

    public void Destroy()
    {
        if (!built) return;
        this.Burn();
        Cascade.Destroy(gameObject);
    }
}

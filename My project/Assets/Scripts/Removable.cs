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
    }

    public void Build()
    {
        built = true;
        gameObject.SetActive(built);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cascade : MonoBehaviour
{
    public Dictionary<string, ArrayList> Cascades = new Dictionary<string, ArrayList>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destroy(Removable obj)
    {
        foreach(KeyValuePair<string, ArrayList> key_value in Cascades)
        {
            ArrayList arr = key_value.Value;

            bool match = false;
            foreach (GameObject gameObject in arr)
            {
                if (gameObject == obj)
                {
                    match = true; break;
                }
            }

            if(match)
            {
                foreach(Removable gameObject in arr)
                {
                    gameObject.Burn();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool done = false;
    private int items= 2;
    private int count = 0;
    public bool first = false;
    private LevelThreeBis levelThree;

    private void Start()
    {
        levelThree = GameObject.FindObjectOfType<LevelThreeBis>();
    }
    void Update()
    {
        done = (count == items);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<GroceriesToCatalog>() != null)
        {
            count++;
            if (!first && levelThree.getTaskIndex()==6)
            {
                first = true;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GroceriesToCatalog>() != null)
        {
            count--;
        }
    }
}

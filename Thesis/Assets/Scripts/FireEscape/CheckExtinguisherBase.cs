using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckExtinguisherBase : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inBase = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "extinguisher")
            inBase = true;
    }
}

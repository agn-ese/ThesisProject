using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrashcan : MonoBehaviour
{
    public bool PlayerIn= false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIn = true;    
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIn = false;
        }
    }
}

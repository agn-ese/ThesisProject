using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ExtinguisherRaycast : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform origin;
    private bool hittingTrashcan = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        // Perform the raycast from the custom origin in the custom direction
        if (Physics.Raycast(origin.position, origin.forward, out hit, Mathf.Infinity))
        {
            if(hit.collider.name == "Trashcan")
                hittingTrashcan = true;

        } else 
            hittingTrashcan=false;

        Debug.DrawRay(origin.position, origin.forward);
    }

    public bool IsHittingTrashcan()
    {
        return hittingTrashcan;
    }
}

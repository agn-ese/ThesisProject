
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CheckCatalog : MonoBehaviour
{
    private Transform origin;
    Action pressingButton;

    [SerializeField] private AudioSource catalog;
    [SerializeField] private AudioSource uncatalog;
    [SerializeField] private GameObject Scan_Right;
    [SerializeField] private GameObject Scan_Left;
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;

    private bool left = false;
    private bool right = false;
    bool start = false;
    private bool firstLeft = true;
    private bool firstRight = true; 

    private TaskManagerBis taskManager;

    public bool breadCart = false;
    public bool ketchup = false;

    private LevelThreeBis levelThree;
    void Start()
    {
        taskManager = GameObject.FindObjectOfType<TaskManagerBis>();
        levelThree = GameObject.FindObjectOfType<LevelThreeBis>();
        taskManager.level2FinishedBis += canStart;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (start)
        {
            bool triggerValueLeft;
            bool triggerValueRight;
            left = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValueLeft);
            right = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValueRight);
            if (left && triggerValueLeft && firstLeft)
            {
                Scan_Left.SetActive(true);
                origin = Left;
                Debug.Log("left");
                firstLeft = false;
                checkCatalog();
            }
            else if (!triggerValueLeft || !left)
            {
                Scan_Left.SetActive(false);
                Debug.Log("Left off");
                firstLeft = true;
            }


            if (right && triggerValueRight && firstRight)
            {
                Scan_Right.SetActive(true);
                origin = Right;
                firstRight = false;
                checkCatalog();
            }
            else if (!right || !triggerValueRight)
            {
                Scan_Right.SetActive(false);
                firstRight = true;
            }
        }
    }

    public void checkCatalog()
    {

        RaycastHit hit;

        // Perform the raycast from the custom origin in the custom direction
        if (Physics.Raycast(origin.position, origin.forward, out hit, 10))
        {
            if (hit.collider.CompareTag("BreadAndCereal") || hit.collider.CompareTag("Drinks") || hit.collider.CompareTag("Sauces") || hit.collider.CompareTag("SodaUntagged")|| hit.collider.CompareTag("BreadUntagged"))
            {
                if (hit.transform.gameObject.GetComponent<GroceriesToCatalog>() != null)
                {
                    uncatalog.Play();
                } else
                {
                    catalog.Play();
                }
                Debug.Log("HIT");
                if(hit.transform.name == "BreadCart" && levelThree.getTaskIndex()==5)
                    breadCart = true;
                if (hit.transform.name == "ketchup" && levelThree.getTaskIndex()==3)
                    ketchup = true;

            }
            Debug.Log("No hit1");

        } else
        {
            Debug.Log("No hit2");
        }
        //Debug.DrawRay(origin.position, origin.forward);

    }

    public void canStart()
    {
        start = true;
    }

}

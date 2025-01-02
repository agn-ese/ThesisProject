using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ChangeTarget : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    private SpatialAwarenessManager SpatialAwarenessManager;
    private int i = 0;
    public Action targetChanged;
    // Start is called before the first frame update
    void Start()
    {
        SpatialAwarenessManager = GameObject.FindFirstObjectByType<SpatialAwarenessManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (i < targets.Length)
        {
            if (targets[i].layer != LayerMask.NameToLayer("Useful"))
            {
                targets[i].layer = LayerMask.NameToLayer("Useful");
                if (targets[i].transform.childCount > 0 && targets[i].name != "bread" && targets[i].name == "drinks" && targets[i].name == "sauces")
                {
                    for (int j = 0; j < targets[i].transform.childCount; j++)
                    {
                        if (targets[i].transform.GetChild(j).name != "SocketInteractor")
                            targets[i].transform.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Useful");
                    }
                }
                targets[i].AddComponent(typeof(TargetPlaceholder));
                SpatialAwarenessManager.NeverLooked = true;
            }
        }


    }

    public void changeTarget()
    {
        if (i < targets.Length)
        {
            targets[i].layer = 0;
            TargetPlaceholder targetPlaceholder = targets[i].GetComponent<TargetPlaceholder>();

            // Check if the TargetPlaceholder component exists, then destroy it
            if (targetPlaceholder != null)
            {
                Destroy(targetPlaceholder);
            }
            if (i != targets.Length - 1)
            {
                i++;
                targetChanged.Invoke();
                SpatialAwarenessManager.NeverLooked = false;
            }
        }
    }

    public int getIndex()
    {
        return i;
    }

    public Transform GetTarget()
    {
        return targets[i].transform;
    }

    public void SetIndex(int index)
    {
        targets[i].layer = 0;
        TargetPlaceholder targetPlaceholder = targets[i].GetComponent<TargetPlaceholder>();

        // Check if the TargetPlaceholder component exists, then destroy it
        if (targetPlaceholder != null)
        {
            Destroy(targetPlaceholder);
        }
        i = index;  
    }
}

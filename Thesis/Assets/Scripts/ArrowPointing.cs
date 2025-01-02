using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointing : MonoBehaviour
{
    private Transform target;
    private Transform arrow;
    private Transform cameraObj;
    private Vector3 position;
    private Vector3 neutralPosition = new Vector3(0.5f, 0.2f, .5f); //the position the arrow has when it's not moving towards the target
    private Vector3 currentVelocity = Vector3.zero;
    private float speedRotation = 1.65f;
    private float dist;
    private GameObject raycastManager;
    private int renderQueue = 3000;  // to make the arrow render after everything
    void Start()
    {
        setPriority(renderQueue);

        arrow = transform.GetChild(0);
        cameraObj = Camera.main.transform;
        neutralPosition = Camera.main.ScreenToWorldPoint(new Vector3(neutralPosition.x * Screen.width, Screen.height * neutralPosition.y, neutralPosition.z));
        transform.parent = cameraObj;
        arrow.position = neutralPosition;

    }


    // Update is called once per frame
    void Update()
    {

        float singleStep = speedRotation * Time.deltaTime;
        Vector3 position;
        TargetPlaceholder placeholder = GameObject.FindObjectOfType<TargetPlaceholder>();
        if (placeholder == null) return;
        target = placeholder.transform;
        position = target.position;
        dist = Vector3.Distance(cameraObj.position, position);
        //hasLooked = true;
        arrow.gameObject.SetActive(true);
        Vector3 targetDirection = target.position - arrow.position;


        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(arrow.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        arrow.rotation = Quaternion.LookRotation(newDirection);

    }

    private void setPriority(int renderQueue)
    {
        Renderer renderer = transform.GetChild(0).GetComponent<Renderer>();

        // Set the render queue of the object's material
        if (renderer != null && renderer.material != null)
        {
            renderer.material.renderQueue = renderQueue;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class circleTarget : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    private Camera _camera;
    //private Camera UICamera;
    private RectTransform imageRect;
    public Vector2 sizeMultiplier = new Vector2(1f, 1f);
    private RaycastManager raycastManager;
    private Image image;
    private ChangeTarget changeTarget;
    void Start()
    {
        //target = GetComponent<Transform>();
        _camera = Camera.main;
        imageRect = transform.parent.GetComponent<RectTransform>();
        raycastManager = transform.parent.GetComponentInParent<RaycastManager>();
        image = GetComponent<Image>();
        image.enabled = false;
        transform.parent.SetParent(_camera.transform, false);
        
        changeTarget = GameObject.FindObjectOfType<ChangeTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        Bounds objectBounds;
        if (raycastManager.getHitObject() != null)
        {
            if(target != raycastManager.getHitObject().transform)
                target = raycastManager.getHitObject().transform;
            if (target != null)
            {

                // Keep the image positioned at the target's world position (relative to the canvas)
                imageRect.position = target.position;

                // Get the bounds of the target object to determine its size
                if (target.GetComponent<Renderer>() == null)
                {
                    objectBounds = target.GetComponentInChildren<Renderer>().bounds;
                }
                else
                {
                    objectBounds = target.GetComponent<Renderer>().bounds;
                }
                Vector3 objectSize = objectBounds.size;

                // Scale the image based on the object's size and the sizeMultiplier
                Vector3 objectScreenMin = _camera.WorldToScreenPoint(objectBounds.min);
                Vector3 objectScreenMax = _camera.WorldToScreenPoint(objectBounds.max);
                Vector2 objectScreenSize = objectScreenMax - objectScreenMin;
                if (objectScreenSize.x > objectScreenSize.y)
                    objectScreenSize.y = objectScreenSize.x;
                else
                    objectScreenSize.x = objectScreenSize.y;
                imageRect.sizeDelta = new Vector2(objectScreenSize.x * sizeMultiplier.x, objectScreenSize.y * sizeMultiplier.y);
;

                // Enable the image 
                image.enabled = true;
           
            }
        } else
        {
            image.enabled = false;
        }
        
    }

}

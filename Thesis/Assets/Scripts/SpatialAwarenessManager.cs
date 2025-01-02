using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAwarenessManager : MonoBehaviour
{
    private GameObject arrow;
    private GameObject circle;
    private RaycastManager raycastManager;
    private bool cond = false; // cond tracks whether the user is close to the object or in sight
    public bool NeverLooked = true;
    private GameObject UICamera;
    private bool inAction = true;
    private Transform target;
    private ChangeTarget changeTarget;
    // Start is called before the first frame update
    void Start()
    {
        arrow = GameObject.Find("ArrowParent");
        circle = GameObject.Find("CircleTarget");
        UICamera = GameObject.Find("UICamera"); //Camera used for rendering just the circle
        UICamera.transform.parent = Camera.main.transform;
        changeTarget = GameObject.FindObjectOfType<ChangeTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inAction)
        {
            target = changeTarget.GetTarget();
            if (target != null)
            {
                cond = Vector3.Distance(Camera.main.transform.position, target.position) < 1.7f;
                if (!RaycastManager._hit)
                {

                    arrow.SetActive(true);
                    circle.SetActive(false);
                }
                else
                {
                    arrow.SetActive(false);
                        circle.SetActive(true);

                }
            }
        }

    }

    public void turnOff()
    {
        inAction = false;
        arrow.SetActive(false);
        circle.SetActive(false);
        
    }

    public void turnOn()
    {

        inAction = true;
    }
}

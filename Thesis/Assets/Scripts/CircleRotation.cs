
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform image;
    private float rotationSpeed = 50f; 
    void Start()
    {
        image = transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        image.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}

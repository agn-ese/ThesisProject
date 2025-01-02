using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeightScale : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshPro text;
    private float weight;
    private float currentWeight = 0;
    private float targetWeight = 0;
    private float duration = 2f; // Duration for the weight change animation
    private float elapsedTime = 0;
    private bool isIncreasing = false;
    private bool isDecreasing = false;
    public bool objectOnScale = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isIncreasing || isDecreasing)
        {
            // Increment time
            elapsedTime += Time.deltaTime;

            // Gradually change the current weight based on time
            currentWeight = Mathf.Lerp(currentWeight, targetWeight, elapsedTime / duration);

            // Update the UI display with rounded weight for realism
            text.text = currentWeight.ToString("F1") + " kg";

            // Stop the simulation when the weight reaches the target
            if (Mathf.Abs(currentWeight - targetWeight) < 0.01f)
            {
                currentWeight = targetWeight;
                text.text = currentWeight.ToString("F1") + " kg";
                isIncreasing = false;
                isDecreasing = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            targetWeight = other.attachedRigidbody.mass;
            elapsedTime = 0;
            isIncreasing = true;
            isDecreasing = false; // Stop any decreasing animation
            if(!objectOnScale) 
                objectOnScale = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            targetWeight = 0;
            // Reset the timer and enable the decreasing animation
            elapsedTime = 0;
            isDecreasing = true;
            isIncreasing = false; // Stop any increasing animation
            objectOnScale = false;
        }
    }
}

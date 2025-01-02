using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLight : MonoBehaviour
{
    private GameObject[] lightObjects;
    private int lightObjectsNum;
    private int count = 0;
    private bool FilledWithLightObjects = false;
    [SerializeField] AudioSource sourceRight;
    [SerializeField] AudioSource sourceWrong;
    public bool firstObjectIn = false;
    private LevelTwoBis levelTwo;

    void Start()
    {
        lightObjects = GameObject.FindGameObjectsWithTag("Light");
        levelTwo = GameObject.FindObjectOfType<LevelTwoBis>();
        lightObjectsNum = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (count == lightObjectsNum)
        {
            FilledWithLightObjects = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            count++;
            sourceRight.Play();
            if(!firstObjectIn && levelTwo.GetTaskIndex()==2) 
                firstObjectIn = true;
        }
        if (other.CompareTag("Normal") || other.CompareTag("Heavy"))
        {
            sourceWrong.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            count--;
        }
    }

    public bool FilledLightBox()
    {
        return FilledWithLightObjects;
    }
}

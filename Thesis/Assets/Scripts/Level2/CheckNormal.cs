using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNormal : MonoBehaviour
{
    private GameObject[] Objects;
    private int ObjectsNum;
    private int count = 0;
    private bool FilledWithObjects = false;
    [SerializeField] AudioSource sourceRight;
    [SerializeField] AudioSource sourceWrong;


    void Start()
    {
        Objects = GameObject.FindGameObjectsWithTag("Normal");
        ObjectsNum = Objects.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (count == ObjectsNum)
        {
            FilledWithObjects = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Normal"))
        {
            count++;
            sourceRight.Play();
        }
        if (other.CompareTag("Light") || other.CompareTag("Heavy"))
        {
            sourceWrong.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Normal"))
        {
            count--;
        }
    }

    public bool FilledNormalBox()
    {
        return FilledWithObjects;
    }
}

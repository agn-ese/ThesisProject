using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHeavy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] heavyObjects;
    private int heavyObjectsNum;
    private int count = 0;
    private bool FilledWithHeavyObjects = false;
    [SerializeField] AudioSource sourceRight;
    [SerializeField] AudioSource sourceWrong;

    void Start()
    {
        heavyObjects = GameObject.FindGameObjectsWithTag("Heavy");
        heavyObjectsNum = heavyObjects.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (count == heavyObjectsNum)
        {
            FilledWithHeavyObjects = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heavy"))
        {
            count++;
            sourceRight.Play();
        }
        if (other.CompareTag("Light") || other.CompareTag("Normal"))
        {
            sourceWrong.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Heavy"))
        {
            count--;
        }
    }

    public bool FilledHeavyBox()
    {
        return FilledWithHeavyObjects;
    }


}

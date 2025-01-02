using System.Collections;
using UnityEngine;

public class ShelvesCheckSauces : MonoBehaviour
{
    // Start is called before the first frame update
    private int num;
    private int counter = 0;
    private int numItems;
    public bool done = false;
    [SerializeField] private GameObject Correct;
    [SerializeField] private GameObject Wrong;
    private string targetTag = "Sauces";
    public bool ketchup = false;
    public bool firstIn = false;
    private LevelThreeBis levelThree;
    void Start()
    {
        num = GameObject.FindGameObjectsWithTag(targetTag).Length - 1;
        levelThree = GameObject.FindObjectOfType<LevelThreeBis>();
    }

    // Update is called once per frame
    void Update()
    {
        done = (counter == num);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.CompareTag(targetTag))
            {
                counter++;
                if (counter == 1 && !firstIn && levelThree.getTaskIndex() == 1)
                {
                    firstIn = true;
                }
                Correct.SetActive(true);
                StartCoroutine(ShowCorrectFeedback());
            }
            else
            {
                Wrong.SetActive(true);
                StartCoroutine(ShowWrongFeedback());
            }
        }
    

        if(other.transform.name == "ketchup" && levelThree.getTaskIndex()==4)
            ketchup = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.CompareTag(targetTag))
            {
                counter--;
                Correct.SetActive(false);
            }
            else
            {
                Wrong.SetActive(false);
            }
        }
    }

    // Coroutine to show the green (Correct) indicator for a few seconds
    IEnumerator ShowCorrectFeedback()
    {
        Correct.SetActive(true);
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        Correct.SetActive(false);
    }

    // Coroutine to show the red (Wrong) indicator for a few seconds
    IEnumerator ShowWrongFeedback()
    {
        Wrong.SetActive(true);
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        Wrong.SetActive(false);
    }
}

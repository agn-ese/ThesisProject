using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TaskManagers;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LevelThreeBis : MonoBehaviour
{
    public TaskManagerBis taskManager;
    private Level level;
    [SerializeField] private List<Task> tasks;
    private int Task = 20;
    private bool notStarted = true;
    private ShelvesCheck shelvesCheck;
    [SerializeField] private GameObject[] itemsInCart;
    [SerializeField] private GameObject ketchup;
    private bool sodaGrab = false;
    private ShelvesCheck drinksCheck;
    private ShelvesCheckBread breadCheck;
    private ShelvesCheckSauces saucesCheck;
    private CheckCatalog catalogCheck;
    private CheckoutTrigger triggerCheck;
    [SerializeField] private GameObject bread;
    [SerializeField] private Animator previousDoorCloses;
    [SerializeField] private RoomBoundaryCheck roomBoundaryCheck;
    [SerializeField] private RoomBoundaryCheck roomBoundaryCheck2;

    private LevelTwoBis levelTwo;

    float startTime;
    float taskStartTime;
    public List<float> taskTimes = new List<float>();
    public float totalTime;
    private float currentTime = 0;
    private bool ended = false;

    void Start()
    {
        shelvesCheck = GameObject.FindObjectOfType<ShelvesCheck>();
        drinksCheck = GameObject.FindObjectOfType<ShelvesCheck>();
        breadCheck = GameObject.FindObjectOfType<ShelvesCheckBread>();
        saucesCheck = GameObject.FindObjectOfType<ShelvesCheckSauces>();
        catalogCheck = GameObject.FindObjectOfType<CheckCatalog>();
        triggerCheck = GameObject.FindObjectOfType<CheckoutTrigger>();
        levelTwo = GameObject.FindObjectOfType<LevelTwoBis>();
        taskManager.level2FinishedBis += addTask;
    }

    // Update is called once per frame
    void Update()
    {


        if (!notStarted)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 180 && !ended)
            {
                totalTime = currentTime;
                taskManager.endExperience();
                Task = 20;
                ended = true;
            }
            switch (Task)
            {
                case 0:

                    if (sodaGrab)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[0].completeTask();
                        tasks[1].StartTask();
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;
                case 1:
                    previousDoorCloses.SetBool("open", false);
                    if (drinksCheck.FirstIn || breadCheck.firstIn || saucesCheck.firstIn)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[1].completeTask();
                        tasks[2].StartTask();
                        taskStartTime = Time.time;
                        Task++;
                    }

                    break;
                case 2: // put everything away
                    if (drinksCheck.done && breadCheck.done && saucesCheck.done)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[2].completeTask();
                        tasks[3].StartTask();
                        taskStartTime = Time.time;
                        ketchup.GetComponent<XRGrabInteractable>().enabled = true;
                        Task++;
                    }

                    break;
                case 3: // scan ketchup
                    if (catalogCheck.ketchup)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[3].completeTask();
                        tasks[4].StartTask();
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;
                case 4:
                    if (saucesCheck.ketchup)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        bread.GetComponent<XRGrabInteractable>().enabled = true;
                        tasks[4].completeTask();
                        tasks[5].StartTask();
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;

                case 5: // take bread and scan it
                    if (catalogCheck.breadCart)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[5].completeTask();
                        tasks[6].StartTask();
                        triggerCheck.enabled = true;
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;
                case 6:
                    if (triggerCheck.first)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[6].completeTask();
                        tasks[7].StartTask();
                        foreach (GameObject item in itemsInCart)
                        {
                            item.SetActive(true);
                        }
                        totalTime = Time.time - startTime;
                        Task++;
                    }
                    break;

                case 7: //last
                    if (drinksCheck.LastSodaIn && triggerCheck.done)
                    {
                        tasks[7].completeTask();
                        taskManager.level3FinishedBis.Invoke();
                        Task = 20;
                    }
                    break;
            }
        }
    }

    public void addTask()
    {
        if (taskManager.GetCurrentLevelIndex() == 2)
        {
            notStarted = false;
            roomBoundaryCheck.enabled = true;
            roomBoundaryCheck2.enabled = false;
            level = taskManager.GetCurrentLevel();
            tasks = level.GetTasks();
            if (tasks.Count > 0)
            {
                startTime = Time.time;
                taskStartTime = startTime;
                tasks[0].StartTask();
                Task = 0;
                Debug.Log("Level 3 started");
            }
        }
    }

    public void sodaGrabbed()
    {
        if (!sodaGrab && Task == 0)
            sodaGrab = true;
    }

    public int getTaskIndex()
    {
        return Task;
    }

}

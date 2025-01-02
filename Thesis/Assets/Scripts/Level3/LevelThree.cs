using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TaskManagers;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LevelThree : MonoBehaviour
{
    public TaskManager taskManager;
    private Level level;
    [SerializeField] private List<Task> tasks;
    private int Task = 20;
    private bool notStarted = true;
    private SpeechManager speechManager;
    private SpatialAwarenessManager spatialAwarenessManager;
    private ShelvesCheck shelvesCheck;
    private ChangeTarget changeTarget;
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

    private LevelTwo levelTwo;

    float startTime;
    float taskStartTime;
    public List<float> taskTimes = new List<float>();
    public float totalTime;

    void Start()
    {
        speechManager = GameObject.FindObjectOfType<SpeechManager>();
        shelvesCheck = GameObject.FindObjectOfType<ShelvesCheck>();
        changeTarget = GameObject.FindObjectOfType<ChangeTarget>();
        drinksCheck = GameObject.FindObjectOfType<ShelvesCheck>();
        breadCheck = GameObject.FindObjectOfType<ShelvesCheckBread>();
        saucesCheck = GameObject.FindObjectOfType<ShelvesCheckSauces>();
        catalogCheck = GameObject.FindObjectOfType<CheckCatalog>();
        triggerCheck = GameObject.FindObjectOfType<CheckoutTrigger>();
        levelTwo = GameObject.FindObjectOfType<LevelTwo>();
        spatialAwarenessManager = GameObject.FindObjectOfType<SpatialAwarenessManager>();
        taskManager.level2Finished += addTask;
    }

    // Update is called once per frame
    void Update()
    {


        if (!notStarted)
        {
            switch (Task)
            {
                case 0:
                    levelTwo.enabled = false;
                    if (speechManager.ClipEnded() == true)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[0].completeTask();
                        tasks[1].StartTask();
                        Task++;
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                    }
                    break;
                case 1:
                    previousDoorCloses.SetBool("open", false);
                    if (sodaGrab)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[1].completeTask();
                        tasks[2].StartTask();
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;
                case 2:
                    if (drinksCheck.FirstIn || breadCheck.firstIn || saucesCheck.firstIn)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[2].completeTask();
                        tasks[3].StartTask();
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                        Task++;
                    }

                    break;
                case 3: // put everything away
                    if(speechManager.ClipEnded() == true)
                    {
                        speechManager.Stop();
                        spatialAwarenessManager.turnOff();
                        spatialAwarenessManager.enabled = false;
                    }
                    if(drinksCheck.done && breadCheck.done && saucesCheck.done)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        spatialAwarenessManager.enabled = true;
                        spatialAwarenessManager.turnOn();
                        tasks[3].completeTask();
                        tasks[4].StartTask();
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                        Task++;
                    }

                    break;
                case 4: // look at cart
                    if (speechManager.ClipEnded())
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        ketchup.GetComponent<XRGrabInteractable>().enabled = true;
                        speechManager.Stop();
                        tasks[4].completeTask();
                        tasks[5].StartTask();
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;
                case 5: // scan ketchup
                    if (catalogCheck.ketchup)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[5].completeTask();
                        tasks[6].StartTask();
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;
                case 6:
                    if (saucesCheck.ketchup)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        bread.GetComponent<XRGrabInteractable>().enabled = true;
                        tasks[6].completeTask();
                        tasks[7].StartTask();
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;

                case 7: // take bread and scan it
                    if (catalogCheck.breadCart)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[7].completeTask();
                        tasks[8].StartTask();

                        changeTarget.changeTarget();
                        triggerCheck.enabled = true;
                        taskStartTime = Time.time;
                        Task++;
                    }
                    break;
                case 8:
                    if (speechManager.ClipEnded())
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[8].completeTask();
                        tasks[9].StartTask();
                        changeTarget.changeTarget();
                        taskStartTime = Time.time;
                        Task++;
                    }

                    break;
                case 9: //put object on checkout
                    if (triggerCheck.first)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[8].completeTask();
                        tasks[9].StartTask();
                        changeTarget.changeTarget();
                        foreach (GameObject item in itemsInCart)
                        {
                            item.SetActive(true);
                        }
                        totalTime = Time.time - startTime;
                        Task++;
                    }
                    break;

                case 10: //last
                    if (speechManager.ClipEnded() == true)
                    {
                        spatialAwarenessManager.turnOff();
                        spatialAwarenessManager.enabled = false;
                    }
                    if (drinksCheck.LastSodaIn && triggerCheck.done)
                    {
                        speechManager.Stop();
                        tasks[10].completeTask();
                        //changeTarget.changeTarget();
                        taskManager.level3Finished.Invoke();
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
        if(!sodaGrab && Task ==1)
            sodaGrab = true;
    }

    public int getTaskIndex()
    {
        return Task;
    }

}

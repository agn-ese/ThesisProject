using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskManagers;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class LevelTwoBis : MonoBehaviour
{
    public TaskManagerBis taskManager;
    private Level level;
    [SerializeField] private List<Task> tasks;
    [SerializeField] private CheckLight lightObjects;
    [SerializeField] private CheckNormal normalObjects;
    [SerializeField] private CheckHeavy heavyObjects;
    [SerializeField] private Transform[] otherCubes;
    public int Task = 20;
    private bool notStarted = true;
    private int numLightObjects;
    private int numNormalObjects;
    private int numHeavyObjects;
    private CheckLight lightObjectsCheck;
    private bool objectGrabbed = false;
    private WeightScale scale;
    private bool firstSoundPlayed = false;
    private bool audioToPlay = false;
    //scene 
    [SerializeField] private Animator DoorOpens;
    [SerializeField] private Animator PreviousDoorCloses;
    [SerializeField] private GameObject LevelOne;
    private bool justStarted = false;
    private FireScene2 levelOne;
    [SerializeField] private TeleportationArea TeleportationAreaLevelThree;
    [SerializeField] private RoomBoundaryCheck roomBoundaryCheck;
    [SerializeField] private RoomBoundaryCheck roomBoundaryCheck1;

    private LevelThreeBis levelThree;

    float startTime;
    float taskStartTime;
    public List<float> taskTimes = new List<float>();
    public float totalTime;
    private float currentTime= 0;
    void Start()
    {
        numLightObjects = GameObject.FindGameObjectsWithTag("Light").Length;
        numNormalObjects = GameObject.FindGameObjectsWithTag("Normal").Length;
        numHeavyObjects = GameObject.FindGameObjectsWithTag("Heavy").Length;
        lightObjectsCheck = GameObject.FindObjectOfType<CheckLight>();
        scale = GameObject.FindObjectOfType<WeightScale>();
        levelThree = GameObject.FindObjectOfType<LevelThreeBis>();
        levelOne = GameObject.FindObjectOfType<FireScene2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (notStarted)
            taskManager.level1FinishedBis += addTask;

        if (!notStarted)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 180)
            {
                taskManager.NextLevel();
                totalTime = currentTime;
                Task = 20;
            }

            switch (Task)
            {

                case 0: //grab object

                    if (objectGrabbed)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[0].completeTask();
                        tasks[1].StartTask();
                        Task++;
                        taskStartTime = Time.time;
                    }
                    break;
                case 1: //object on scale
                    if (scale.objectOnScale)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[1].completeTask();
                        tasks[2].StartTask();
                        Task++;
                        taskStartTime = Time.time;
                        //audioToPlay = true;
                    }
                    break;
                case 2: //object in container
                    if (lightObjectsCheck.firstObjectIn)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[2].completeTask();
                        tasks[3].StartTask();
                        foreach (Transform cube in otherCubes)
                        {
                            cube.gameObject.SetActive(true);
                            cube.gameObject.GetComponent<XRGrabInteractable>().enabled = true;
                        }
                        //audioToPlay = true;
                        Task++;
                        taskStartTime = Time.time;
                    }
                    break;
                case 3: //put everything away
                    bool finished = false;

                    if (lightObjects.FilledLightBox() && normalObjects.FilledNormalBox() && heavyObjects.FilledHeavyBox())
                    {
                        finished = true;
                    }
                    if (finished)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[3].completeTask();
                        tasks[4].StartTask();
                        Task++;
                        totalTime = Time.time - startTime;
                        finished = false;

                    }
                    break;
                case 4: //last
                    levelThree.enabled = true;

                    tasks[4].completeTask();
                    Task = 15;
                    taskManager.updateCurrentLevel();
                    taskManager.level2FinishedBis.Invoke();
                    DoorOpens.SetBool("open", true);
                    TeleportationAreaLevelThree.enabled = true;
                    break;

            }
        }
    }

    public void addTask()
    {
        if (taskManager.GetCurrentLevelIndex() == 1)
        {
            notStarted = false;
            roomBoundaryCheck.enabled = true;
            roomBoundaryCheck1.enabled = false;
            level = taskManager.GetCurrentLevel();
            tasks = level.GetTasks();
            Debug.Log("Level 2 started");
            if (tasks.Count > 0)
            {
                tasks[0].StartTask();
                startTime = Time.time;
                taskStartTime = startTime;
                Task = 0;
                justStarted = true;
            }
        }
    }

    public void grabbed()
    {
        if (!objectGrabbed)
            objectGrabbed = true;
    }

    public void ungrabbed()
    {
        if (objectGrabbed)
            objectGrabbed = false;
    }

    public int GetTaskIndex()
    {
        return Task;
    }
}

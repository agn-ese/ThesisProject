using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskManagers;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class LevelTwo : MonoBehaviour
{
    public TaskManager taskManager;
    private Level level;
    private SpeechManager speechManager;
    [SerializeField] private List<Task> tasks;
    [SerializeField] private CheckLight lightObjects;
    [SerializeField] private CheckNormal normalObjects;
    [SerializeField] private CheckHeavy heavyObjects;
    [SerializeField] private Transform exampleCube;
    [SerializeField] private Transform[] otherCubes;
    public int Task = 20;
    private bool notStarted = true;
    private int numLightObjects;
    private int numNormalObjects;
    private int numHeavyObjects;
    private CheckLight lightObjectsCheck;
    private bool objectGrabbed = false;
    private WeightScale scale;
    private ChangeTarget changeTarget;
    private bool firstSoundPlayed = false;
    private SpatialAwarenessManager spatialAwarenessManager;
    private bool audioToPlay = false;
    //scene 
    [SerializeField] private Animator DoorOpens;
    [SerializeField] private Animator PreviousDoorCloses;
    [SerializeField] private GameObject LevelOne;
    private bool justStarted = false;
    private FireScene levelOne;
    [SerializeField] private TeleportationArea TeleportationAreaLevelThree;
    [SerializeField] private RoomBoundaryCheck roomBoundaryCheck;
    [SerializeField] private RoomBoundaryCheck roomBoundaryCheck1;

    private LevelThree levelThree;

    float startTime;
    float taskStartTime;
    public List<float> taskTimes = new List<float>();
    public float totalTime;
    void Start()
    {
        spatialAwarenessManager = GameObject.FindObjectOfType<SpatialAwarenessManager>();
        speechManager = GameObject.FindObjectOfType<SpeechManager>();
        numLightObjects = GameObject.FindGameObjectsWithTag("Light").Length;
        numNormalObjects = GameObject.FindGameObjectsWithTag("Normal").Length;
        numHeavyObjects = GameObject.FindGameObjectsWithTag("Heavy").Length;
        lightObjectsCheck = GameObject.FindObjectOfType<CheckLight>();
        scale = GameObject.FindObjectOfType<WeightScale>();
        changeTarget = GameObject.FindObjectOfType<ChangeTarget>();
        levelThree = GameObject.FindObjectOfType<LevelThree>();
        levelOne = GameObject.FindObjectOfType<FireScene>();
    }

    // Update is called once per frame
    void Update()
    {
        if (notStarted) 
            taskManager.level1Finished += addTask;

        if (!notStarted)
        {

            switch (Task)
            {
                case 0: //start
                    Debug.Log("clip " + speechManager.getClip() + speechManager.ClipEnded());
                    levelOne.enabled = false;
                    if (justStarted)
                    {
                        changeTarget.changeTarget();
                        justStarted = false;
                        //speechManager.setClip(6);
                    }
                    if (speechManager.ClipEnded() )
                    {
                        taskTimes.Add(Time.time-taskStartTime);
                        tasks[0].completeTask();
                        tasks[1].StartTask();
                        changeTarget.changeTarget();
                        Task++;
                        taskStartTime = Time.time;
                    }
                    break;
                case 1: //weight scale
                   if (speechManager.ClipEnded() == true)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        PreviousDoorCloses.SetBool("open", false);
                        //LevelOne.SetActive(false);
                        tasks[1].completeTask();
                        tasks[2].StartTask();
                        changeTarget.changeTarget();
                        Task++;
                        taskStartTime= Time.time;
                    }
                    break;
                case 2: //light container
                    if (speechManager.ClipEnded())
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[2].completeTask();
                        tasks[3].StartTask();
                        changeTarget.changeTarget();
                        Task++;
                        taskStartTime= Time.time;
                    }
                    break;
                case 3: //normal container
                    if (speechManager.ClipEnded())
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[3].completeTask();
                        tasks[4].StartTask();
                        changeTarget.changeTarget();
                        Task++;
                        taskStartTime = Time.time;
                    }
                    break;
                case 4: //heavy container

                    if (speechManager.ClipEnded())
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[4].completeTask();
                        tasks[5].StartTask();
                        Task++;
                        audioToPlay = true;
                        //changeTarget.changeTarget();
                        taskStartTime = Time.time;
                    }
                    break;
                case 5: //sound correct
                    if (audioToPlay)
                    {
                        speechManager.increaseClip();
                        audioToPlay = false;
                    }

                    if (speechManager.ClipEnded() && !audioToPlay)
                    {
                        if (!firstSoundPlayed)
                        {
                            speechManager.increaseClip();
                            firstSoundPlayed = true;
                        }


                    }
                    if(speechManager.ClipEnded() &&  firstSoundPlayed )
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[5].completeTask();
                        tasks[6].StartTask();
                        firstSoundPlayed = false;
                        Task++;
                        //changeTarget.changeTarget();
                        audioToPlay = true;
                        taskStartTime = Time.time;
                    }

                    break;
                case 6: //sound wrong
                    if (audioToPlay)
                    {
                        speechManager.increaseClip();
                        audioToPlay = false;
                    }
                    if (speechManager.ClipEnded() && !audioToPlay)
                    {
                        if (!firstSoundPlayed)
                        {
                            speechManager.increaseClip();
                            firstSoundPlayed = true;
                        }
                    }
                    if(speechManager.ClipEnded() && firstSoundPlayed)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        tasks[6].completeTask();
                        tasks[7].StartTask();
                        foreach (Transform cube in otherCubes)
                        {
                            cube.gameObject.SetActive(false);
                        }
                        exampleCube.gameObject.SetActive(true);
                        changeTarget.changeTarget();
                        Task++;
                        taskStartTime = Time.time;
                        //audioToPlay = true;
                    }
                    break;
                case 7: //grab object

                    if (objectGrabbed)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[7].completeTask();
                        tasks[8].StartTask();
                        changeTarget.changeTarget();
                        Task++;
                        taskStartTime = Time.time;
                        //audioToPlay = true;
                    }
                    break;
                case 8: //object on scale
                    if (scale.objectOnScale)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[8].completeTask();
                        tasks[9].StartTask();
                        changeTarget.changeTarget();
                        Task++;
                        taskStartTime = Time.time;
                        //audioToPlay = true;
                    }
                    break;
                case 9: //object in container
                    if (lightObjectsCheck.firstObjectIn)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        tasks[9].completeTask();
                        tasks[10].StartTask();
                        changeTarget.changeTarget();
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
                case 10: //put everything away
                    bool finished = false;
                    if (speechManager.ClipEnded())
                    {
                        spatialAwarenessManager.turnOff();
                        spatialAwarenessManager.enabled = false;
                    }
                    if (lightObjects.FilledLightBox() && normalObjects.FilledNormalBox() && heavyObjects.FilledHeavyBox())
                    {
                        finished = true;
                    }
                    if (finished)
                    {
                        taskTimes.Add(Time.time - taskStartTime);
                        speechManager.Stop();
                        spatialAwarenessManager.enabled = true;
                        spatialAwarenessManager.turnOn();
                        tasks[9].completeTask();
                        tasks[10].StartTask();
                        changeTarget.changeTarget();
                        
                        //audioToPlay = true;
                        Task++;
                        totalTime = Time.time - startTime;
                        finished = false;
                        
                    }
                    break;
                case 11: //last
                    levelThree.enabled = true;
                    if (speechManager.ClipEnded())
                    {
                        tasks[10].completeTask();
                        Task = 15;
                        taskManager.updateCurrentLevel();
                        taskManager.level2Finished.Invoke();
                        changeTarget.changeTarget();
                        DoorOpens.SetBool("open", true);
                        TeleportationAreaLevelThree.enabled = true;
                       // this.enabled = false;
                    }
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
                //Task task = tasks[0];
                //StartCoroutine(CheckTaskStatus(task));
            }
        }
    }

    public void grabbed()
    {
        if(!objectGrabbed) 
            objectGrabbed = true;
    }

    public void ungrabbed()
    {
        if(objectGrabbed)
            objectGrabbed = false;
    }

    public int GetTaskIndex()
    {
        return Task;
    }
}

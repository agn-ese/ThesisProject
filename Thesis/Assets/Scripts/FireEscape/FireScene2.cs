using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskManagers;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class FireScene2 : MonoBehaviour
{
    // Start is called before the first frame update
    public TaskManagerBis taskManager;
    private Level level;
    [SerializeField] private List<Task> tasks;
    int Task = 0;
    [SerializeField] private ParticleSystem fire;
    private bool buttonPressed = false;
    private float timeHeld = 0;
    [SerializeField] private Animator DoorOpens;
    [SerializeField] private GameObject fireExtinguisherBase;
    [SerializeField] private ExtinguisherRaycast _ExtinguisherRaycast;
    [SerializeField] private Transform Extinguisher;
    private bool first = true;

    private Collider extinguisherCollider;
    private LevelTwoBis levelTwo;
    private TriggerTrashcan triggerTrashcan;
    [SerializeField] private TeleportationArea levelTwoTeleportation;
    float startTime;
    float taskStartTime;
    public List<float> taskTimes = new List<float>();
    public float totalTime;
    private float currentTime = 0;
    private bool start = false;

    void Start()
    {
        levelTwo = GameObject.FindObjectOfType<LevelTwoBis>();
        taskManager.level1StartedBis += addTask;
        triggerTrashcan = GameObject.FindObjectOfType<TriggerTrashcan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 180)
            {
                taskManager.NextLevel();
                totalTime = currentTime;
                start = false;
                Task = 20;
            }
        }

        switch (Task)
        {

            case 2:
                if (buttonPressed && _ExtinguisherRaycast.IsHittingTrashcan() && timeHeld < 5f && triggerTrashcan.PlayerIn)
                {
                    // Increment the timer while the key is pressed
                    timeHeld += Time.deltaTime;
                    Debug.Log(timeHeld);
                }
                if (timeHeld > 5)
                {
                    timeHeld = 5;
                }

                if (timeHeld == 5)
                {
                    fire.Stop();
                    taskTimes.Add(Time.time - taskStartTime);
                    tasks[2].completeTask();
                    tasks[3].StartTask();
                    Task++;
                    taskStartTime = Time.time;
                }
                break;
            case 3:
                fireExtinguisherBase.gameObject.SetActive(true);
                if (fireExtinguisherBase.GetComponent<CheckExtinguisherBase>().inBase && Extinguisher.position.y <= 0)
                {
                    taskTimes.Add(Time.time - taskStartTime);
                    tasks[3].completeTask();
                    tasks[4].StartTask();
                    Task++;
                    totalTime = Time.time - startTime;
                }
                break;
            case 4:
                levelTwo.enabled = true;
                DoorOpens.SetBool("open", true);
                levelTwoTeleportation.enabled = true;
                taskManager.updateCurrentLevel();
                taskManager.level1FinishedBis.Invoke();
                Task = 8;
                start = false;
                break;

            default:
                break;
        }

    }

    public void addTask()
    {
        if (taskManager.GetCurrentLevelIndex() == 0)
        {
            level = taskManager.GetCurrentLevel();
            tasks = level.GetTasks();
            if (tasks.Count > 0)
            {
                tasks[0].StartTask();
                Task task = tasks[0];
                StartCoroutine(CheckTaskStatus(task));
            }
            startTime = Time.time;
            taskStartTime = startTime;
        }
    }

    public void AlarmPressed()
    {
        if (tasks[0].GetStatus() == Status.OnGoing)
        {
            taskTimes.Add(Time.time - taskStartTime);
            tasks[0].completeTask();
            tasks[1].StartTask();
            Task++;
            taskStartTime = Time.time;
        }
        else
        {
            Debug.Log("Not the right moment to press the alarm");
        }
    }

    private IEnumerator CheckTaskStatus(Task task)
    {
        while (task.GetStatus() == Status.OnGoing)
        {
            yield return null;
        }
    }


    private IEnumerator wait()
    {
        yield return new WaitForSeconds(10);
    }

    public void ButtonPressed()
    {
        if (Task == 2)
            buttonPressed = true;
        else
            buttonPressed = false;

    }

    public void ButtonReleased()
    {
        buttonPressed = false;
    }


    public void GetExtinguisher()
    {
        if (Task == 1)
        {
            taskTimes.Add(Time.time - taskStartTime);
            tasks[1].completeTask();
            tasks[2].StartTask();
            Task++;
            taskStartTime = Time.time;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskManagers;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class FireScene : MonoBehaviour
{
    // Start is called before the first frame update
    public  TaskManager taskManager ;
    private Level level;
    private SpeechManager speechManager ;
    [SerializeField] private List<Task> tasks; 
    int Task = 0;
    [SerializeField] private ParticleSystem fire;
    private bool buttonPressed = false;
    private float timeHeld = 0;
    [SerializeField] private Animator DoorOpens;
    [SerializeField] private GameObject fireExtinguisherBase;
    [SerializeField] private ExtinguisherRaycast _ExtinguisherRaycast;
    [SerializeField] private Transform Extinguisher;
    private ChangeTarget changeTarget ;
    private bool first = true;

    private Collider extinguisherCollider;
    public Collider sphereCastCollider;
    private LevelTwo levelTwo;
    private TriggerTrashcan triggerTrashcan;
    [SerializeField] private TeleportationArea levelTwoTeleportation;
    float startTime;
    float taskStartTime;
    public List<float> taskTimes = new List<float>();
    public float totalTime;

    void Start()
    {
        changeTarget = GameObject.FindObjectOfType<ChangeTarget>();
        speechManager = GameObject.FindObjectOfType<SpeechManager>();
        levelTwo = GameObject.FindObjectOfType<LevelTwo>();
        taskManager.level1Started += addTask;
        triggerTrashcan = GameObject.FindObjectOfType<TriggerTrashcan>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (Task) {
            case 0:
                if (first /*&& RaycastManager._hit*/)
                {
                    speechManager.playAudio();
                    first = false;
                }
                if (speechManager.ClipEnded())
                {
                    tasks[0].completeTask();
                    tasks[1].StartTask();
                    Task++;
                    changeTarget.changeTarget();
                    taskTimes.Add(Time.time - taskStartTime);

                    taskStartTime = Time.time;
                }
                break;

            case 3:
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
                    speechManager.Stop();
                    taskTimes.Add(Time.time - taskStartTime);
                    //timeHeld = 0.0f; 
                    tasks[3].completeTask();
                    tasks[4].StartTask();
                    Task++;
                    changeTarget.changeTarget();
                    taskStartTime = Time.time;
                }
                break;
            case 4:
                fireExtinguisherBase.gameObject.SetActive(true);
                if (fireExtinguisherBase.GetComponent<CheckExtinguisherBase>().inBase && Extinguisher.position.y <= 0)
                {
                    taskTimes.Add(Time.time - taskStartTime);
                    speechManager.Stop();
                    tasks[4].completeTask();
                    tasks[5].StartTask();
                    

                    Task++;
                    changeTarget.changeTarget();
                    totalTime = Time.time - startTime;
                }
                break;
            case 5:
                levelTwo.enabled = true;
                if (speechManager.ClipEnded())
                {

                    DoorOpens.SetBool("open", true);
                    levelTwoTeleportation.enabled = true;
                    taskManager.updateCurrentLevel();
                    taskManager.level1Finished.Invoke();
                    Task = 8;
                    //changeTarget.changeTarget();

                    //this.enabled = false;
                }


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
        if (tasks[1].GetStatus() == Status.OnGoing)
        {
            taskTimes.Add(Time.time - taskStartTime);
            speechManager.Stop();
            tasks[1].completeTask();
            tasks[2].StartTask();
            Task++;
            changeTarget.changeTarget();
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
        if(Task==3)
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
        if (Task == 2)
        {
            taskTimes.Add(Time.time - taskStartTime);
            speechManager.Stop();
            tasks[2].completeTask();
            tasks[3].StartTask();
            Task++;
            changeTarget.changeTarget();
            taskStartTime = Time.time;  
        }
    }


}

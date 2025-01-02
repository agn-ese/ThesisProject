using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskManagers;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
public class TaskManagerBis : MonoBehaviour
{
    public static TaskManagerBis instance;
    private List<Level> levels = new List<Level>();
    private int currentLevelIndex = 0;
    public Action level1StartedBis;
    public Action level1FinishedBis, level2FinishedBis, level3FinishedBis;
    private SpeechManager speechManager;
    private ChangeTarget changeTarget;

    [SerializeField] private GameObject TheEndCanvas;
    [SerializeField] private AudioSource lastAudio;
    [SerializeField] private GameObject canTeleport;
    [SerializeField] private GameObject canMove;


    //positions for respawn
    [SerializeField] private Transform level2;
    [SerializeField] private Transform level3;
    [SerializeField] private Transform _player;
    [SerializeField] private TeleportationArea leveltwo, levelthree;
    private FireScene2 levelOne;
    private LevelTwoBis levelTwo;
    private LevelThreeBis levelThree;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        levelOne = GameObject.FindObjectOfType<FireScene2>();
        levelTwo = GameObject.FindObjectOfType<LevelTwoBis>();
        levelThree = GameObject.FindObjectOfType<LevelThreeBis>();
        level3FinishedBis += endExperience;


        Level level1 = new Level(1);
        level1.AddTask(new Task("Alarm", "Press the alarm"));
        level1.AddTask(new Task("Fire extinguisher", "Find the fire extinguisher"));
        level1.AddTask(new Task("Extinguish the fire", "Extinguish the fire"));
        level1.AddTask(new Task("Clean", "Bring the extinguisher back to its position"));
        level1.AddTask(new Task("End first level", "End first level"));
        level1.StartLevel();
        levels.Add(level1);

        Level level2 = new Level(2);
        level2.AddTask(new Task("Grab object", "Grab object on table"));
        level2.AddTask(new Task("Object on scale", "Put the object on the scale"));
        level2.AddTask(new Task("Put object in container", "Put object in container"));
        level2.AddTask(new Task("All the objects", "Put all the objects away"));
        level2.AddTask(new Task("Last task", "last task"));
        levels.Add(level2);
        Level level3 = new Level(3);
        level3.AddTask(new Task("Tidy", "find object not in the right place"));
        level3.AddTask(new Task("Put", "Put object in the right place"));
        level3.AddTask(new Task("Put everything away", "Put everything away")); //no spatialmanager
        level3.AddTask(new Task("Take object", "Take object and scan it"));
        level3.AddTask(new Task("Checkout", "Put object on checkout")); // not catalogized
        level3.AddTask(new Task("Other object in cart", "Do the same thing"));
        level3.AddTask(new Task("Scanned object", "Scanned"));
        level3.AddTask(new Task("Put oject away", "Find place for that object"));
        level3.AddTask(new Task("Finish", "Do the same for the rest of the experience"));
        //level3.AddTask(new Task("Grazie per aver partecipato", "Grazie per aver partecipato")); //suono indipendente
        levels.Add(level3);
        level1StartedBis.Invoke();

    }

    private void Update()
    {

    }

    public void updateCurrentLevel()
    {
        currentLevelIndex++;
    }

    public Level GetCurrentLevel()
    {
        Level CurrentLevel = levels[currentLevelIndex];
        return CurrentLevel;
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public void endExperience()
    {
        SaveFile.instance.SavePlayerData("NoHelp", levelOne.taskTimes, levelTwo.taskTimes, levelThree.taskTimes, levelOne.totalTime, levelTwo.totalTime, levelThree.totalTime);
        lastAudio.Play();
        canTeleport.gameObject.SetActive(false);
        canMove.gameObject.SetActive(false);
        TheEndCanvas.SetActive(true);
    }


    public void NextLevel()
    {

        if (currentLevelIndex == 0)
        {

            updateCurrentLevel();
            _player.position = level2.position;
            level1FinishedBis.Invoke();
            leveltwo.enabled = true;
        }
        else if (currentLevelIndex == 1)
        {
            updateCurrentLevel();
            _player.position = level3.position;
            level2FinishedBis.Invoke();
            levelthree.enabled = true;
        }
    }

}


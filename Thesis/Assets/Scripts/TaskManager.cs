using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskManagers;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
public class TaskManager : MonoBehaviour
{
    public static TaskManager instance; 
    private List<Level> levels = new List<Level>();
    private int currentLevelIndex = 0;
    public Action level1Started;
    public Action level1Finished, level2Finished, level3Finished;
    private SpeechManager speechManager;
    private ChangeTarget changeTarget;

    [SerializeField]private GameObject TheEndCanvas;
    [SerializeField] private AudioSource lastAudio;
    [SerializeField] private GameObject canTeleport;
    [SerializeField] private GameObject canMove;

    private SpatialAwarenessManager spatialAwarenessManager;


    //positions for respawn
    [SerializeField] private Transform level2;
    [SerializeField] private Transform level3;
    [SerializeField] private Transform _player;
    [SerializeField] private TeleportationArea leveltwo, levelthree;
    private FireScene levelOne;
    private LevelTwo levelTwo;
    private LevelThree levelThree;



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
        levelOne = GameObject.FindObjectOfType<FireScene>();
        levelTwo = GameObject.FindObjectOfType<LevelTwo>();
        levelThree = GameObject.FindObjectOfType<LevelThree>();
        level3Finished += endExperience;
        speechManager = GameObject.FindObjectOfType<SpeechManager>();
        spatialAwarenessManager = GameObject.FindObjectOfType<SpatialAwarenessManager>();
        changeTarget = GameObject.FindObjectOfType<ChangeTarget>();

        Level level1 = new Level(1);
        level1.AddTask(new Task("Fire", "Find the fire"));
        level1.AddTask(new Task("Alarm", "Press the alarm"));
        level1.AddTask(new Task("Fire extinguisher", "Find the fire extinguisher"));
        level1.AddTask(new Task("Extinguish the fire", "Extinguish the fire"));
        level1.AddTask(new Task("Clean", "Bring the extinguisher back to its position"));
        level1.AddTask(new Task("End first level", "End first level"));
        level1.StartLevel();
        levels.Add(level1);
        
        Level level2 = new Level(2);
        level2.AddTask(new Task("Start", "Start tutorial, look at objects"));
        level2.AddTask(new Task("Weight Scale", "Look at weight scale"));
        level2.AddTask(new Task("Light container", " Look at light container"));
        level2.AddTask(new Task("Normal container", "Look at normal container"));
        level2.AddTask(new Task("Heavy container", "Look at hevy container"));
        level2.AddTask(new Task("SoundCorrect", "Show sound of correct answers"));
        level2.AddTask(new Task("SoundWrong", "Show sound of wrong answers"));
        level2.AddTask(new Task("Grab object", "Grab object on table"));
        level2.AddTask(new Task("Object on scale", "Put the object on the scale"));
        level2.AddTask(new Task("Put object in container", "Put object in container"));
        level2.AddTask(new Task("All the objects", "Put all the objects away"));
        level2.AddTask(new Task("Last task","last task"));
        levels.Add(level2);
        Level level3 = new Level(3);
        level3.AddTask(new Task("Beginning", "starting")); // ci sono 3 scaffali bla bla, sei un commesso bla bla
        level3.AddTask(new Task("Tidy", "find object not in the right place"));
        level3.AddTask(new Task("Put", "Put object in the right place"));
        level3.AddTask(new Task("Put everything away", "Put everything away")); //no spatialmanager
        level3.AddTask(new Task("Look at cart", "Find catalogized items"));
        level3.AddTask(new Task("Take object", "Take object and scan it"));
        level3.AddTask(new Task("Checkout", "Put object on checkout")); // not catalogized
        level3.AddTask(new Task("Other object in cart", "Do the same thing"));
        level3.AddTask(new Task("Scanned object", "Scanned"));
        level3.AddTask(new Task("Put oject away", "Find place for that object"));
        level3.AddTask(new Task("Finish", "Do the same for the rest of the experience"));
        //level3.AddTask(new Task("Grazie per aver partecipato", "Grazie per aver partecipato")); //suono indipendente
        levels.Add(level3);
        level1Started.Invoke();

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
        SaveFile.instance.SavePlayerData("A", levelOne.taskTimes, levelTwo.taskTimes,levelThree.taskTimes, levelOne.totalTime, levelTwo.totalTime, levelThree.totalTime);
        speechManager.Stop();
        lastAudio.Play();
        canTeleport.gameObject.SetActive(false);
        canMove.gameObject.SetActive(false);
        TheEndCanvas.SetActive(true);
    }


    public void NextLevel()
    {
        GameObject Last = GameObject.FindObjectOfType<TargetPlaceholder>().gameObject;
        Last.layer = 0;
        if (GameObject.FindObjectOfType<TargetPlaceholder>())
        {
            Destroy(GameObject.FindObjectOfType<TargetPlaceholder>());
        }
        if (currentLevelIndex == 0)
        {

            speechManager.setClip(5);
            changeTarget.SetIndex(5);
            updateCurrentLevel();
            _player.position = level2.position;
            level1Finished.Invoke();
            leveltwo.enabled = true;
        }
        else if (currentLevelIndex == 1)
        {
            speechManager.setClip(20);
            changeTarget.SetIndex(16);
            updateCurrentLevel();
            _player.position = level3.position;
            level2Finished.Invoke();
            levelthree.enabled = true;
        }
        spatialAwarenessManager.NeverLooked = false;
    }

}


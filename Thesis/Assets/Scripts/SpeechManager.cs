using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource source;
    private TaskManager taskManager;
    private float delayBeforeStoppingAudio = 1.5f;
    private float timeOutOfFocus = 0f;
    public int index = 0;
    private bool first = true;
    public bool ended = false;
    private float audioLength;
    private bool firstTimer = false;
    private bool firstAccess = true;
    private bool wasDistracted = false;
    private bool start = true;
    private LevelTwo leveltwo;
    public System.Action actionEnded;
    private float timer2 = 0;
    private List<int> finishedAudios = new List<int>();
    private bool CanCheck = true;


    [SerializeField] private ChangeTarget changeTarget;
    void Start()
    {
        taskManager = GameObject.FindObjectOfType<TaskManager>();
       leveltwo = GameObject.FindObjectOfType<LevelTwo>();
        changeTarget.targetChanged += increaseClip;
        for (int i = 0; i < audioClips.Length; i++)
        {
            finishedAudios.Add(0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (source.clip == null)
            return;
       if (!RaycastManager.first && CanCheck)
            CheckAttention();
        else
        {
            while(timer2 < 3f)
            {
                timer2 += Time.deltaTime;
            }
            if(timer2 >= 3f)
                RaycastManager.first = false;   
        }
       

        if (source.time >= audioClips[index].length && !ended && !source.isPlaying)
        {
            finishedAudios[index] = 1;
            ended = true;
        }

    }

    public void CheckAttention()
    {
            if (!RaycastManager._hit)
            {
                wasDistracted = true;
                timeOutOfFocus += Time.deltaTime;
                if (source.isPlaying && timeOutOfFocus >= delayBeforeStoppingAudio)
                {
                    source.Pause();
                }

            }
            else
            {
                if (!ended)
                {
                    timeOutOfFocus = 0;
                    if (!source.isPlaying && wasDistracted)
                    {
                        if (source.clip == audioClips[index])
                        {
                            playAudio();
                            ended = false;
                        }
                        else
                        {
                            source.clip = audioClips[index];
                            playAudio();
                            ended = false;
                        }
                    }
                    wasDistracted = false;
                }
            }
    }

    public void increaseClip()
    {
        index++;
        ended = false;
        playAudio();
    }


    public bool ClipEnded()
    {
        return (finishedAudios[index] == 1);
    }

    public bool ClipIndexEnded(int i)
    {
        return (finishedAudios[i] == 1);
    }


    public void playAudio()
    {
        if (!source.isPlaying )
        {
            if(source.clip != audioClips[index])
                source.clip = audioClips[index];
            //audioLength = source.clip.length;
            //source.loop = false;
            source.Play();
            ended = false;
            CanCheck = true;
        }
    }

 


    public void CannotStart()
    {
        start = false;
    }

    public void CanStart()
    {
       start = true;
    }

    public void setClip(int index2)
    {
        index = index2;
        source.clip = audioClips[index];
        ended = false;
        source.Play();
        CanCheck = true;
    }

    public int getClip()
    {
        return index;
    }

    public void Stop()
    {
        source.Stop();
        CanCheck = false;
    }
}

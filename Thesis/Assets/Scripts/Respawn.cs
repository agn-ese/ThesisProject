using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform[] respawnPoints;
    private TaskManager taskManager;
    void Start()
    {
        taskManager = GameObject.FindObjectOfType<TaskManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -3)
        {
            switch (taskManager.GetCurrentLevelIndex()) {
                case 0:
                    transform.position = respawnPoints[0].position;
                    break;
                case 1:
                    transform.position = respawnPoints[1].position;
                    break;
                case 2:
                    transform.position = respawnPoints[2].position;
                    break;
            }
        }

    }
}

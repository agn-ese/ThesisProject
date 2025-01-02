using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TaskManagers
{
    public enum Status { NotStarted, OnGoing, Completed };

    public class Task
    {
        private string taskName;
        private string taskDescription;
        private Status status;
        

        public Task(string taskName, string taskDescription)
        {
            this.taskName = taskName;
            this.taskDescription = taskDescription;
            this.status = Status.NotStarted;
        }

        public void completeTask()
        {
            this.status = Status.Completed;
            
        }

        public Status GetStatus()
        {
            return this.status;
        }

        public void StartTask()
        {
            if (this.status == Status.NotStarted) this.status = Status.OnGoing;
        }
    }

    public class Level
    {
        private List<Task> tasks;
        private int levelNumber;
        private Status status;

        public Level(int levelNumber)
        {
            this.tasks = new List<Task>();
            this.levelNumber = levelNumber;
            this.status = Status.NotStarted;
        }

        public void EndLevel()
        {
            status = Status.Completed;
            Debug.Log("LevelFinished");
        }

        public void StartLevel()
        {
            status = Status.OnGoing;
        }

        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public List<Task> GetTasks() { return this.tasks; }


    }

}


using System.Collections.Generic;
using UnityEngine;

public class AI_TaskManager : MonoBehaviour
{
    public static AI_TaskManager Instance;

    private List<ITask> Tasks { get; } = new List<ITask>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddTask(ITask task)
    {
        Tasks.Add(task);
    }

    public bool RemoveTask(ITask task)
    {
        return Tasks.Remove(task);
    }

    public List<ITask> GetUnassignedTasks()
    {
        return Tasks.FindAll(task => task.GetAssignedWorker() == null);
    }
}

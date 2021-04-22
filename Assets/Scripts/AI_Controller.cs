using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public float ThinkUpdateInterval = 1f;
    public ITask currentTask;
    private AI_TaskManager taskManager;
    private CustomAI pathingController;

    private void Start()
    {
        taskManager = AI_TaskManager.Instance;
        pathingController = GetComponent<CustomAI>();

        InvokeRepeating("ThinkLoop", 0f, ThinkUpdateInterval);
    }

    private void ThinkLoop()
    {
        if (currentTask == null)
        {
            GetNewTask();
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTask.GetTargetPosition());
        if (distance >= currentTask.GetMaxDistance() && distance > currentTask.GetMinDistance())
        {
            pathingController.ChangeObjective(currentTask.GetTargetPosition());

            return;
        }

        pathingController.StopPathing();

        if (!IsLookingAtPoint(currentTask.GetTargetPosition()))
        {
            Turn();
            return;
        }

        currentTask.Do();

        if (currentTask.Done())
        {
            currentTask.Destroy();
            currentTask = null;
        }
    }

    private void GetNewTask()
    {
        foreach (ITask task in taskManager.GetUnassignedTasks())
        {
            if (task.AssignWorker(this))
            {
                currentTask = task;
                break;
            }
        }
    }

    private bool IsLookingAtPoint(Vector3 point)
    {
        bool pointIsToTheRight = (point.x - transform.position.x) > 0;
        bool characterIsLookingRight = transform.localScale.x > 0;

        return pointIsToTheRight == characterIsLookingRight;
    }

    private void Turn()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = -1 * Mathf.Abs(currentScale.x);
        transform.localScale = currentScale;
    }
}

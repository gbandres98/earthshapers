using UnityEngine;

public abstract class AI_BaseTask : MonoBehaviour, ITask
{
    protected AI_Controller worker;

    public virtual void Start()
    {
        AI_TaskManager.Instance.AddTask(this);
    }

    public virtual bool AssignWorker(AI_Controller worker)
    {
        if (this.worker != null)
        {
            return false;
        }

        this.worker = worker;
        return true;
    }

    public virtual AI_Controller GetAssignedWorker()
    {
        return worker;
    }

    public virtual void UnassignWorker()
    {
        worker = null;
    }

    public virtual Vector3 GetTargetPosition()
    {
        return transform.position;
    }

    public virtual void Destroy()
    {
        _ = AI_TaskManager.Instance.RemoveTask(this);
        Destroy(gameObject);
    }

    public abstract float GetMinDistance();
    public abstract float GetMaxDistance();

    public abstract void Do();

    public abstract bool Done();

    public abstract string GetDebugTaskDescription();
}

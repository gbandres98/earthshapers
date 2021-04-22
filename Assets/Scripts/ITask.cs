using UnityEngine;
public interface ITask
{
    bool AssignWorker(AI_Controller worker);
    AI_Controller GetAssignedWorker();
    void UnassignWorker();
    Vector3 GetTargetPosition();
    float GetMinDistance();
    float GetMaxDistance();
    void Do();
    bool Done();
    void Destroy();
    string GetDebugTaskDescription();
}

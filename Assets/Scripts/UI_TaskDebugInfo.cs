using UnityEngine.UI;
using UnityEngine;

public class UI_TaskDebugInfo : MonoBehaviour
{
    public AI_Controller ai;
    private Text text;

    private void Start()
    {
        this.text = GetComponent<Text>();
    }

    private void Update()
    {
        if (ai.currentTask != null)
        {
            text.text = ai.currentTask.GetDebugTaskDescription();
        }
        else
        {
            text.text = "No Task";
        }
    }
}

using UnityEngine;

public class UI_CharacterInfo : MonoBehaviour
{
    public Transform playerTransform;

    private void Awake()
    {
    }

    private void Start()
    {
        AI_Controller ai = playerTransform.GetComponent<AI_Controller>();
        if (ai)
        {
            GetComponentInChildren<UI_TaskDebugInfo>().ai = ai;
        }
    }

    private void Update()
    {
        GetComponentInChildren<UI_Inventory>().inventoryItems = playerTransform.GetComponent<Inventory>().inventory;

        Vector3 newPosition = UI_Controller.Instance.WorldToCanvasPoint(playerTransform.transform.position);
        newPosition.z += 360f;
        newPosition.y += 25f;
        transform.position = newPosition;
    }
}

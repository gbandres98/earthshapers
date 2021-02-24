using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterInfo : MonoBehaviour
{
    public Transform playerTransform;

    void Awake() {
    }

    void Start() {
        
    }

    void Update()
    {        
        GetComponentInChildren<UI_Inventory>().inventoryItems = playerTransform.GetComponent<BaseCharacter>().Inventory;
        
        Vector3 newPosition = UI_Controller.Instance.WorldToCanvasPoint(playerTransform.transform.position);
        newPosition.z += 360f;
        newPosition.y += 25f;
        transform.position = newPosition;
    }
}

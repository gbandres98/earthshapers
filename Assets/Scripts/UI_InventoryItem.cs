using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{

    public InventoryItem item;
    Image image;
    Text text;
    
    void Start()
    {
        if (item != null) {
            image = transform.GetChild(0).GetComponent<Image>();
            text = transform.GetChild(1).GetComponent<Text>();

            image.sprite = Resources.Load<Sprite>("Sprites/Items/Dirt_Item");
            image.color = Color.white;
            text.text = item.amount.ToString();
        }   
    }

}

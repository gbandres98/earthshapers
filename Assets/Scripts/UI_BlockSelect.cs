using System.Collections.Generic;
using UnityEngine;

public class UI_BlockSelect : MonoBehaviour
{
    private void Start()
    {
        List<InventoryItem> items = new List<InventoryItem>(Game.InventoryItems.Values);
        items.FindAll(i => i.placeable).ForEach(i => CreateBlockOption(i.itemID));
    }

    private void CreateBlockOption(int itemID)
    {
        UI_BlockSelect_Option option = Instantiate(Resources.Load<UI_BlockSelect_Option>("UI/UI_BlockSelect_Option"));
        option.transform.SetParent(transform);
        option.transform.position += new Vector3(0, 0, 360);
        option.transform.localScale = Vector3.one;
        option.itemID = itemID;
    }
}

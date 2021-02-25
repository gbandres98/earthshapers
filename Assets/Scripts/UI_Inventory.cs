using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    public InventoryItem[] inventoryItems = new InventoryItem[6];

    private void Start()
    {
    }

    private void Update()
    {
        foreach (Transform item in GetComponentInChildren<Transform>())
        {
            Destroy(item.gameObject);
        }

        foreach (InventoryItem item in inventoryItems)
        {
            GameObject inventoryItemUI = Instantiate(Resources.Load("UI/UI_InventoryItem") as GameObject);
            inventoryItemUI.GetComponent<UI_InventoryItem>().item = item;
            inventoryItemUI.transform.SetParent(transform);
            inventoryItemUI.transform.position = new Vector3(0, 0, 360f);
            inventoryItemUI.transform.localScale = Vector3.one;
        }
    }
}

using UnityEngine;
public class InventoryItem
{
    public int itemID;
    public int amount;
    public int stackSize;
    public ToolTypeEnum toolType;
    public bool placeable;
    public Vector2 placingOffset;
    public bool tileable;

    public InventoryItem(int itemID, int amount = 1, int stackSize = 64, ToolTypeEnum toolType = ToolTypeEnum.NONE, bool placeable = false, float placingOffsetX = 0, float placingOffsetY = 0, bool tileable = false)
    {
        this.itemID = itemID;
        this.amount = amount;
        this.stackSize = stackSize;
        this.toolType = toolType;
        this.placeable = placeable;
        this.placingOffset = new Vector2(placingOffsetX, placingOffsetY);
        this.tileable = tileable;
    }

    public InventoryItem(InventoryItem item, int amount)
    {
        this.itemID = item.itemID;
        this.amount = amount;
        this.stackSize = item.stackSize;
        this.toolType = item.toolType;
        this.placeable = item.placeable;
        this.placingOffset = item.placingOffset;
        this.tileable = item.tileable;
    }
}


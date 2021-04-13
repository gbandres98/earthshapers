
public class InventoryItem
{
    public int itemID;
    public int amount;
    public int stackSize;

    public ToolTypeEnum toolType;

    public InventoryItem(int itemID, int amount = 1, int stackSize = 64, ToolTypeEnum toolType = ToolTypeEnum.NONE)
    {
        this.itemID = itemID;
        this.amount = amount;
        this.stackSize = stackSize;
        this.toolType = toolType;
    }

    public InventoryItem(InventoryItem item, int amount)
    {
        this.itemID = item.itemID;
        this.amount = amount;
        this.stackSize = item.stackSize;
        this.toolType = item.toolType;
    }
}


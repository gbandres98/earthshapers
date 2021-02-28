
public class InventoryItem
{
    public int itemID;
    public int amount;
    public int stackSize;

    public ToolTypeEnum toolType;

    public InventoryItem(int itemID, int amount, int stackSize, ToolTypeEnum toolType)
    {
        this.itemID = itemID;
        this.amount = amount;
        this.stackSize = stackSize;
        this.toolType = toolType;
    }

    public InventoryItem(InventoryItem item)
    {
        this.itemID = item.itemID;
        this.amount = item.amount;
        this.stackSize = item.stackSize;
        this.toolType = item.toolType;
    }
}


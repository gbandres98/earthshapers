
public class InventoryItem
{
    public int itemID;
    public int amount;
    public int stackSize;

    public InventoryItem(int itemID, int amount, int stackSize)
    {
        this.itemID = itemID;
        this.amount = amount;
        this.stackSize = stackSize;
    }
}

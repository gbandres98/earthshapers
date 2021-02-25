
public class InventoryItem
{
    public int item_id;
    public int amount;
    public int stackSize;

    public InventoryItem(int item_id, int amount, int stackSize)
    {
        this.item_id = item_id;
        this.amount = amount;
        this.stackSize = stackSize;
    }
}

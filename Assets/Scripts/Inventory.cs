using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly InventoryItem[] inventory = new InventoryItem[6];
    public void AddItem(InventoryItem newItem)
    {
        foreach (InventoryItem item in inventory)
        {
            if (
                (item != null) &&
                (item.itemID == newItem.itemID) &&
                (item.amount < item.stackSize)
                )
            {
                item.amount += newItem.amount;

                if (item.amount > item.stackSize)
                {
                    newItem.amount = item.amount - item.stackSize;
                    item.amount = item.stackSize;
                    AddItem(newItem);
                }

                return;
            }
        }

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = newItem;
                return;
            }
        }
    }

    public bool GiveItem(Inventory reciever, int itemID, int amount)
    {
        int freeSpace = reciever.GetFreeSpace(itemID);
        if (amount > freeSpace && HasItem(itemID, amount))
        {
            reciever.AddItem(new InventoryItem(Game.InventoryItems[itemID], freeSpace));
            RemoveItem(itemID, freeSpace);
        }
        else
        {
            reciever.AddItem(new InventoryItem(Game.InventoryItems[itemID], amount));
            RemoveItem(itemID, amount);
        }
        return true;
    }

    public bool HasItem(int itemID, int amount)
    {
        int totalAmount = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null && inventory[i].itemID == itemID)
            {
                totalAmount += inventory[i].amount;
                if (totalAmount >= amount)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveItem(int itemID, int amount)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null && inventory[i].itemID == itemID)
            {
                inventory[i].amount -= amount;
                if (inventory[i].amount <= 0)
                {
                    inventory[i] = null;
                }

                return;
            }
        }
    }

    public int GetFreeSpace(int itemID)
    {
        int freeSpace = 0;
        int stack = Game.InventoryItems[itemID].stackSize;
        foreach (InventoryItem item in inventory)
        {
            if (item == null)
            {
                freeSpace += stack;
                continue;
            }
            if (item.itemID == itemID)
            {
                freeSpace += item.stackSize - item.amount;
            }
        }
        return freeSpace;
    }
}



using System.Collections.Generic;

public static class Game
{
    public static Dictionary<int, string> Items = new Dictionary<int, string>()
    {
        {0, null},
        {1, "Dirt"},
        {2, "Wood"},
        {3, "TreeWood"},
        {4, "Leaf"},
        {101, "Table"},
        {251, "Shovel"},
        {252, "Axe"}
    };

    public static Dictionary<int, InventoryItem> InventoryItems = new Dictionary<int, InventoryItem>()
    {
     {1, new InventoryItem(1)},
     {2, new InventoryItem(2)},
     {101, new InventoryItem(101)},
     {251, new InventoryItem(251,stackSize: 1,toolType: ToolTypeEnum.SHOVEL)},
     {252, new InventoryItem(252,stackSize: 1,toolType: ToolTypeEnum.AXE)}
    };
}

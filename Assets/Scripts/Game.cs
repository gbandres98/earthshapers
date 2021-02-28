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
     {1, new InventoryItem(1,1,64,ToolTypeEnum.NONE)},
     {2, new InventoryItem(2,1,64,ToolTypeEnum.NONE)},
     {3, new InventoryItem(3,1,64,ToolTypeEnum.NONE)},
     {4, new InventoryItem(4,1,64,ToolTypeEnum.NONE)},
     {101, new InventoryItem(101,1,64,ToolTypeEnum.NONE)},
     {251, new InventoryItem(251,1,1,ToolTypeEnum.SHOVEL)},
     {252, new InventoryItem(252,1,1,ToolTypeEnum.AXE)}
    };
}

﻿using UnityEngine;
using UnityEngine.Tilemaps;
public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance;
    private Tilemap map;

    private void Awake()
    {
        Instance = this;
        map = GetComponent<Tilemap>();
    }

    public BaseBlock GetBlockUnderMouse()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit)
        {
            BaseBlock block = hit.collider.gameObject.GetComponent<BaseBlock>();
            if (block)
            {
                return block;
            }
        }

        return null;
    }

    public bool PlaceBlockUnderMouse(int itemID)
    {
        if (GetBlockUnderMouse())
        {
            return false;
        }
        GameObject block = Resources.Load($"Blocks/{Game.Items[itemID]}") as GameObject;
        Vector3 offset = block.GetComponent<BaseBlock>().placingOffset;
        block.transform.position = GetBlockCenterUnderMouse() + offset;
        GameObject blockInstance = Instantiate(block);
        blockInstance.transform.parent = transform;
        MovementMeshNode n = blockInstance.GetComponent<MovementMeshNode>();
        n.RecalculateNeighbours();
        return true;
    }

    public Vector3 GetBlockCenterUnderMouse()
    {
        Vector3Int cell = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        return map.GetCellCenterWorld(cell);
    }
}

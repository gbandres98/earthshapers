using UnityEngine;
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
        return GetBlockAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public BaseBlock GetBlockAtPosition(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

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
        return PlaceBlock(itemID, GetBlockCenterUnderMouse());
    }

    public bool PlaceBlock(int itemID, Vector3 position)
    {
        if (GetBlockAtPosition(position))
        {
            return false;
        }

        GameObject block = Resources.Load($"Blocks/{Game.Items[itemID]}") as GameObject;
        block.transform.position = position;

        GameObject blockInstance = Instantiate(block);
        blockInstance.transform.parent = transform;

        MovementMeshNode movementMeshNode = blockInstance.GetComponent<MovementMeshNode>();
        if (movementMeshNode)
        {
            movementMeshNode.RecalculateNeighbours();
        }

        return true;
    }

    public Vector3 GetBlockCenterUnderMouse()
    {
        Vector3Int cell = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        return map.GetCellCenterWorld(cell);
    }
}

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

    public void PlaceBlockUnderMouse()
    {
        if (GetBlockUnderMouse())
        {
            return;
        }

        Vector3Int cell = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        GameObject block = Instantiate(Resources.Load("Blocks/Dirt") as GameObject);
        block.name = "Dirt";
        block.transform.parent = transform;

        block.transform.position = map.GetCellCenterWorld(cell);
    }
}

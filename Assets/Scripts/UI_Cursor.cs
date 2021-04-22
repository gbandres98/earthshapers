using UnityEngine;
using System.Collections.Generic;

public class UI_Cursor : MonoBehaviour
{
    public static UI_Cursor Instance;
    public int selectedBlock;
    private Vector3 selectionStart;
    private Vector3 selectionEnd;
    private Vector3 previewStart;
    private Vector3 previewEnd;
    private bool selecting;
    private BlockManager blockManager;
    private SpriteRenderer sprite;
    private readonly List<GameObject> selectionPreviewSprites = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        blockManager = BlockManager.Instance;
    }

    private void Update()
    {
        transform.position = blockManager.GetBlockCenterUnderMouse();

        if (selectedBlock != 0)
        {
            sprite.sprite = Resources.Load<Sprite>($"Sprites/Items/{Game.Items[selectedBlock]}_Item");
            transform.position += (Vector3)Game.InventoryItems[selectedBlock].placingOffset;
        }
        else
        {
            sprite.sprite = null;
        }

        if (selecting)
        {
            DrawSelectionPreview();
        }

        if (Input.GetMouseButtonDown(0) && selectedBlock != 0)
        {
            selectionStart = blockManager.GetBlockCenterUnderMouse();
            selecting = true;
        }

        if (Input.GetMouseButtonUp(0) && selecting)
        {
            selectionEnd = blockManager.GetBlockCenterUnderMouse();
            PlaceGhosts();
            selecting = false;
            selectedBlock = 0;
            ClearSelectionPreview();
        }
    }

    private void DrawSelectionPreview()
    {
        if (selectedBlock == 0 || !Game.InventoryItems[selectedBlock].tileable)
        {
            return;
        }

        Vector3 blockCenterUnderMouse = blockManager.GetBlockCenterUnderMouse();

        if (previewStart == selectionStart && previewEnd == blockCenterUnderMouse)
        {
            return;
        }

        ClearSelectionPreview();

        float minX = Mathf.Min(selectionStart.x, blockCenterUnderMouse.x);
        float maxX = Mathf.Max(selectionStart.x, blockCenterUnderMouse.x);
        float minY = Mathf.Min(selectionStart.y, blockCenterUnderMouse.y);
        float maxY = Mathf.Max(selectionStart.y, blockCenterUnderMouse.y);

        Sprite spriteResource = Resources.Load<Sprite>($"Sprites/Blocks/{Game.Items[selectedBlock]}");

        for (float x = minX; x <= maxX; x++)
        {
            for (float y = minY; y <= maxY; y++)
            {
                GameObject previewSprite = new GameObject("SelectionPreviewSprite");
                SpriteRenderer spriteRenderer = previewSprite.AddComponent<SpriteRenderer>();

                spriteRenderer.sprite = spriteResource;
                spriteRenderer.color = new Color(1, 1, 1, 0.2f);
                spriteRenderer.sortingLayerName = "Front";
                previewSprite.transform.position = new Vector3(x, y, 0);

                selectionPreviewSprites.Add(previewSprite);
            }
        }

        previewStart = selectionStart;
        previewEnd = blockCenterUnderMouse;
    }

    private void ClearSelectionPreview()
    {
        if (selectionPreviewSprites.Count > 0)
        {
            foreach (GameObject sprite in selectionPreviewSprites)
            {
                Destroy(sprite);
            }

            selectionPreviewSprites.Clear();
        }
    }

    private void PlaceGhosts()
    {
        if (selectedBlock == 0)
        {
            return;
        }

        GameObject ghostResource = Resources.Load<GameObject>("Entities/Ghost");

        if (!Game.InventoryItems[selectedBlock].tileable)
        {
            PlaceGhost(ghostResource, selectionEnd.x, selectionEnd.y);
        }

        float minX = Mathf.Min(selectionStart.x, selectionEnd.x);
        float maxX = Mathf.Max(selectionStart.x, selectionEnd.x);
        float minY = Mathf.Min(selectionStart.y, selectionEnd.y);
        float maxY = Mathf.Max(selectionStart.y, selectionEnd.y);

        for (float x = minX; x <= maxX; x++)
        {
            for (float y = minY; y <= maxY; y++)
            {
                PlaceGhost(ghostResource, x, y);
            }
        }
    }

    private void PlaceGhost(GameObject ghostResource, float x, float y)
    {
        GameObject ghost = Instantiate(ghostResource);
        ghost.transform.position = new Vector3(x, y, 0);

        ghost.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Blocks/{Game.Items[selectedBlock]}");

        if (selectedBlock == -1)
        {
            _ = ghost.AddComponent<AI_BreakBlockTask>();
        }
        else
        {
            _ = ghost.AddComponent<AI_PlaceBlockTask>();
            ghost.GetComponent<AI_PlaceBlockTask>().BlockID = selectedBlock;

            ghost.transform.position += (Vector3)Game.InventoryItems[selectedBlock].placingOffset;
        }
    }
}

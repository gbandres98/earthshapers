using UnityEngine;

public class UI_BlockHighlight : MonoBehaviour
{
    public static UI_BlockHighlight Instance;
    private BlockManager blockManager;
    private SpriteRenderer spriteRenderer;
    private bool highlightEnabled = true;

    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        blockManager = BlockManager.Instance;
    }
    private void Update()
    {
        if (highlightEnabled)
        {
            if (blockManager.GetBlockUnderMouse())
            {
                spriteRenderer.enabled = true;
                Vector3 blockCenter = blockManager.GetBlockCenterUnderMouse();
                Vector3 mouseCanvasPosition = UI_Controller.Instance.WorldToCanvasPoint(blockCenter);
                transform.position = mouseCanvasPosition + new Vector3(0, 0, 360);
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    public void SetEnabled(bool enabled)
    {
        spriteRenderer.enabled = enabled;
        this.highlightEnabled = enabled;
    }
}

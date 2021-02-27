using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller Instance;
    private Camera uiCamera;

    private bool characterInfoPanelOpen = false;
    private Transform blockHighlight;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        blockHighlight = GameObject.FindGameObjectWithTag("BlockHighlight").transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OpenCharacterInfoPanel();
        }

        PlaceBlockHighlight();
    }

    private void PlaceBlockHighlight()
    {
        Vector3 blockCenter = BlockManager.Instance.GetBlockCenterUnderMouse();
        Vector3 mouseCanvasPosition = WorldToCanvasPoint(blockCenter);
        blockHighlight.position = mouseCanvasPosition + new Vector3(0, 0, 360);
    }

    private void OpenCharacterInfoPanel()
    {
        if (characterInfoPanelOpen)
        {
            CloseCharacterInfoPanel();
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit && hit.collider.CompareTag("ItemPicker"))
        {
            BaseCharacter character = hit.collider.gameObject.GetComponent<BaseCharacter>();

            GameObject panel = Instantiate(Resources.Load<GameObject>("UI/UI_CharacterInfo"));

            panel.transform.SetParent(transform);
            panel.transform.localScale = Vector3.one;
            panel.GetComponent<UI_CharacterInfo>().playerTransform = character.transform;

            characterInfoPanelOpen = true;
        }
    }

    private void CloseCharacterInfoPanel()
    {
        UI_CharacterInfo panel = GetComponentInChildren<UI_CharacterInfo>();
        Destroy(panel.gameObject);

        characterInfoPanelOpen = false;
    }

    public Vector2 WorldToCanvasPoint(Vector3 worldPoint)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPoint);

        _ = RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPoint, uiCamera, out Vector2 canvasPoint);

        return transform.TransformPoint(canvasPoint);
    }
}

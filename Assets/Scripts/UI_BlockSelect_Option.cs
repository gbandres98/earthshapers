using UnityEngine;
using UnityEngine.UI;

public class UI_BlockSelect_Option : MonoBehaviour
{
    public int itemID;

    public void Start()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Items/{Game.Items[itemID]}_Item");
    }

    public void Select()
    {
        GetComponent<Button>().Select();
        UI_Cursor.Instance.selectedBlock = itemID;
    }
}

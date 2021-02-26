using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{
    public InventoryItem item;
    private Image image;
    private Text text;

    private void Start()
    {
        if (item != null)
        {
            image = transform.GetChild(0).GetComponent<Image>();
            text = transform.GetChild(1).GetComponent<Text>();

            image.sprite = Resources.Load<Sprite>($"Sprites/Items/{Game.Items[item.item_id]}_Item");
            image.color = Color.white;
            text.text = item.amount.ToString();
        }
    }
}

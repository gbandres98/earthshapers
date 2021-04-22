using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public int itemID = 0;
    public int amount = 1;
    public int stackSize = 64;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemPicker"))
        {
            Destroy(gameObject);
            Inventory inventory = other.gameObject.GetComponent<Inventory>();
            inventory.AddItem(new InventoryItem(Game.InventoryItems[itemID], amount));
        }
    }

    private void FixedUpdate()
    {
        GameObject[] itemPickers = GameObject.FindGameObjectsWithTag("ItemPicker");

        col.isTrigger = false;

        foreach (GameObject itemPicker in itemPickers)
        {
            float distance = Vector3.Distance(itemPicker.transform.position, transform.position);
            if (distance < 8.0f)
            {
                col.isTrigger = true;
                Vector3 direction = itemPicker.transform.position - transform.position;
                rb.AddForce(direction.normalized * 200 / distance);
            }
        }
    }
}

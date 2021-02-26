using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public int itemID = 0;
    public int amount = 1;
    public int stackSize = 64;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemPicker"))
        {
            Destroy(gameObject);
            BaseCharacter character = other.gameObject.GetComponent<BaseCharacter>();
            character.AddItem(new InventoryItem(itemID, amount, stackSize));
        }
    }

    private void FixedUpdate()
    {
        GameObject[] itemPickers = GameObject.FindGameObjectsWithTag("ItemPicker");

        foreach (GameObject itemPicker in itemPickers)
        {
            float distance = Vector3.Distance(itemPicker.transform.position, transform.position);
            if (distance < 3.0f)
            {
                Vector3 direction = itemPicker.transform.position - transform.position;
                rb.AddForce(direction.normalized * 80 / distance);
            }
        }
    }
}

﻿using UnityEngine;

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
            Inventory inv = other.gameObject.GetComponent<Inventory>();
            inv.AddItem(new InventoryItem(Game.InventoryItems[itemID], 1));
        }
    }

    private void FixedUpdate()
    {
        GameObject[] itemPickers = GameObject.FindGameObjectsWithTag("ItemPicker");

        foreach (GameObject itemPicker in itemPickers)
        {
            float distance = Vector3.Distance(itemPicker.transform.position, transform.position);
            if (distance < 8.0f)
            {
                Vector3 direction = itemPicker.transform.position - transform.position;
                rb.AddForce(direction.normalized * 80 / distance);
            }
        }
    }
}

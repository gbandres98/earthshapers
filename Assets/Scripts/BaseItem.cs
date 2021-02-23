﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public int item_id = 0;
    public int amount = 1;
    public int stackSize = 64;

    Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ItemPicker"))
        {
            Destroy(gameObject);
            BaseCharacter character = other.gameObject.GetComponent<BaseCharacter>();
            character.AddItem(new InventoryItem(item_id, amount, stackSize));
        }        
    }

    void FixedUpdate()
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    
    public int item_id = 0;
    public float hp = 10.0f;

    public void Damage(float amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        GameObject item = Instantiate(Resources.Load<GameObject>($"Items/{Game.Items[item_id]}_Item"));
        item.transform.position = transform.position;
    }

}

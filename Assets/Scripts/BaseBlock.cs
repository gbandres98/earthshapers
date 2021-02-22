using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    
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
        GameObject item = Instantiate(Resources.Load<GameObject>($"Items/{this.name}_Item"));
        item.transform.position = transform.position;
    }

}

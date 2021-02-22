using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    
    float hp = 10.0f;

    public float Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
            if (hp <= 0)
             {
                 Destroy(gameObject);
             }
        }
    }

}

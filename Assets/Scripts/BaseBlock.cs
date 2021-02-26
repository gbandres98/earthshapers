using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    public int itemID = 0;
    public float hp = 10.0f;
    private float currentHp;
    private SpriteMask spriteMask;
    private new ParticleSystem particleSystem;

    private void Awake()
    {
        spriteMask = GetComponent<SpriteMask>();
        particleSystem = GetComponent<ParticleSystem>();
        currentHp = hp;
    }

    public void Damage(float amount)
    {
        currentHp -= amount;

        particleSystem.Play();

        // spriteIndex = max hp: 0 -> 0hp: 4
        int spriteIndex = (int)Mathf.Floor((1 - (currentHp / hp)) * 5);

        spriteMask.sprite = Resources.Load<Sprite>($"Sprites/Particles/DamagedBlockMask_{spriteIndex}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        if (Game.Items.ContainsKey(itemID))
        {
            GameObject item = Instantiate(Resources.Load<GameObject>($"Items/{Game.Items[itemID]}_Item"));
            item.transform.position = transform.position;
        }
    }
}

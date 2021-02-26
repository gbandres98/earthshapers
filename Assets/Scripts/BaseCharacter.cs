using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public float speed = 300.0f;
    public float runSpeedModifier = 2.0f;
    public LayerMask groundLayer;
    public float jumpForce = 9.0f;
    public float attackCooldown = 1.0f;
    public float attackDamage = 4;

    [HideInInspector]
    public bool isRunning = false;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;
    private float attackCooldownFinishTime;
    public InventoryItem[] Inventory { get; } = new InventoryItem[6];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = CheckGrounded();
    }

    private void FixedUpdate()
    {
        Animate();
    }

    private void Animate()
    {
        animator.SetFloat("xSpeed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("ySpeed", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    public void Move(float direction)
    {
        float normalized = direction * speed * Time.deltaTime;
        if (isRunning)
        {
            normalized *= runSpeedModifier;
        }

        rb.velocity = new Vector2(normalized, rb.velocity.y);

        Vector3 currentScale = transform.localScale;
        bool facingRight = currentScale.x > 0;

        if (facingRight && direction < 0)
        {
            currentScale.x = -1 * Mathf.Abs(currentScale.x);
        }
        else if (!facingRight && direction > 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }

        transform.localScale = currentScale;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void PrimaryAttack()
    {
        if (Time.time < attackCooldownFinishTime)
        {
            return;
        }

        attackCooldownFinishTime = Time.time + attackCooldown;

        BaseBlock target = BlockManager.Instance.GetBlockUnderMouse();
        if (target)
        {
            target.Damage(4);
        }
    }

    public void SecondaryAttack()
    {
        if (HasItem(1) && BlockManager.Instance.PlaceBlockUnderMouse())
        {
            RemoveItem(1);
        }
    }

    public void AddItem(InventoryItem newItem)
    {
        foreach (InventoryItem item in Inventory)
        {
            if (
                (item != null) &&
                (item.item_id == newItem.item_id) &&
                (item.amount < item.stackSize)
                )
            {
                item.amount += newItem.amount;

                if (item.amount > item.stackSize)
                {
                    newItem.amount = item.amount - item.stackSize;
                    item.amount = item.stackSize;
                    AddItem(newItem);
                }

                return;
            }
        }

        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = newItem;
                return;
            }
        }
    }

    public bool HasItem(int item_id)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] != null && Inventory[i].item_id == item_id)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(int item_id)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] != null && Inventory[i].item_id == item_id)
            {
                Inventory[i].amount--;
                if (Inventory[i].amount <= 0)
                {
                    Inventory[i] = null;
                }
            }
        }
    }

    private bool CheckGrounded()
    {
        bool leftGrounded = Physics2D.Raycast(transform.position + (Vector3.left / 2), Vector2.down, 2f, groundLayer);
        bool rightGrounded = Physics2D.Raycast(transform.position + (Vector3.right / 2), Vector2.down, 2f, groundLayer);

        Debug.Log($"Left: {leftGrounded}");
        Debug.Log($"Right: {rightGrounded}");

        return leftGrounded || rightGrounded;
    }
}

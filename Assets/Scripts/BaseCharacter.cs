using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public float speed = 300.0f;
    public float runSpeedModifier = 2.0f;
    public LayerMask groundLayer;
    public float jumpForce = 9.0f;
    public float attackCooldown = 1.0f;
    public float jumpCooldown = 0.5f;
    public float attackDamage = 4;

    [HideInInspector]
    public bool isRunning = false;
    private Rigidbody2D rb;
    private Animator animator;
    public Inventory inventory;
    public bool IsGrounded { get; private set; }
    private float jumpCooldownFinishTime;
    private float attackCooldownFinishTime;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        IsGrounded = CheckGrounded();
    }

    private void FixedUpdate()
    {
        Animate();
    }

    private void Animate()
    {
        animator.SetFloat("xSpeed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("ySpeed", rb.velocity.y);
        animator.SetBool("isGrounded", IsGrounded);
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

#pragma warning disable IDE0058
    public void Jump()
    {
        if (Time.time < jumpCooldownFinishTime)
        {
            return;
        }

        jumpCooldownFinishTime = Time.time + jumpCooldown;

        if (IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
#pragma warning restore IDE0058

    public void AttackBlock(BaseBlock target)
    {
        if (target && (Time.time > attackCooldownFinishTime))
        {
            if (inventory.HasItemType(target.toolTypeNeeded))
            {
                target.Damage(attackDamage * 5);
                return;
            }

            target.Damage(attackDamage);
        }
    }

    public bool PlaceBlock(int itemID, Vector3 position)
    {
        if (Time.time < attackCooldownFinishTime)
        {
            return false;
        }

        attackCooldownFinishTime = Time.time + attackCooldown;

        if (inventory.HasItem(itemID, 1) && BlockManager.Instance.PlaceBlock(itemID, position))
        {
            inventory.RemoveItem(itemID, 1);
            return true;
        }

        return false;
    }
    private bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.35f, groundLayer)
            || Physics2D.Raycast(transform.position + (Vector3.left * 0.48f), Vector2.down, 1.35f, groundLayer)
            || Physics2D.Raycast(transform.position + (Vector3.right * 0.48f), Vector2.down, 1.35f, groundLayer);
    }
}
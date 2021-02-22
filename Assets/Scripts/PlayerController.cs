using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 300.0f;
    public float runSpeedModifier = 2.0f;
    public LayerMask groundLayer;
    public float jumpForce = 9.0f;
    public float attackCooldown = 1.0f;
    public float attackDamage = 4;

    Rigidbody2D rb;
    Animator animator;
    float horizontalInput;
    bool isRunning = false;
    bool isGrounded = false;
    float attackCooldownFinishTime;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if(Input.GetMouseButton(0) && Time.time > attackCooldownFinishTime)
        {
            PrimaryAttack();
        }

        if(Input.GetMouseButton(1) && Time.time > attackCooldownFinishTime)
        {
            SecondaryAttack();
        }
    }

    void FixedUpdate() {
        Move(horizontalInput);
        Animate();
    }

    void Move(float direction)
    {
        float normalized = direction * speed * Time.deltaTime;
        if (isRunning)
            normalized *= runSpeedModifier;

        rb.velocity = new Vector2(normalized, rb.velocity.y);

        Vector3 currentScale = transform.localScale;
        bool facingRight = currentScale.x > 0;

        if(facingRight && direction < 0)
        {
            currentScale.x = -1 * Mathf.Abs(currentScale.x);
        } else if(!facingRight && direction > 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }

        transform.localScale = currentScale;
    }

    void Animate()
    {        
        animator.SetFloat("xSpeed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("ySpeed", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void PrimaryAttack()
    {
        attackCooldownFinishTime = Time.time + attackCooldown;

        BaseBlock target = BlockManager.Instance.GetBlockUnderMouse();
        target.Hp -= 4;
        Debug.Log(target.Hp);
    }

    void SecondaryAttack()
    {
        attackCooldownFinishTime = Time.time + attackCooldown;
        
        BlockManager.Instance.PlaceBlockUnderMouse();
    }
}

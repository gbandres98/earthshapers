using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float runSpeedModifier;
    public LayerMask groundLayer;
    public float jumpForce;

    Rigidbody2D rb;
    Animator animator;
    float horizontalInput;
    bool facingRight = true;
    bool isRunning = false;
    public bool isGrounded = false;

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
        
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, groundLayer);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
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
        if(facingRight && direction < 0)
        {
            currentScale.x = -5;
            facingRight = false;
        } else if(!facingRight && direction > 0)
        {
            currentScale.x = 5;
            facingRight = true;
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
}

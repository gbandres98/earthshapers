using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BaseCharacter character;
    private float horizontalInput;

    private void Start()
    {
        character = GetComponent<BaseCharacter>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        character.isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetButtonDown("Jump"))
        {
            character.Jump();
        }

        if (Input.GetMouseButton(0) && MouseDistanceToPlayer() < 200f)
        {
            character.PrimaryAttack();
        }

        if (Input.GetMouseButton(1) && MouseDistanceToPlayer() > 15f && MouseDistanceToPlayer() < 200f)
        {
            character.SecondaryAttack();
        }
    }

    private void FixedUpdate()
    {
        character.Move(horizontalInput);
    }

    public float MouseDistanceToPlayer()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePos = Input.mousePosition;
        return (mousePos - pos).magnitude;
    }
}

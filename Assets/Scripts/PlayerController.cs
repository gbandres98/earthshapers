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

        if (Input.GetMouseButton(0))
        {
            character.PrimaryAttack();
        }

        if (Input.GetMouseButton(1))
        {
            character.SecondaryAttack();
        }
    }

    private void FixedUpdate()
    {
        character.Move(horizontalInput);
    }
}

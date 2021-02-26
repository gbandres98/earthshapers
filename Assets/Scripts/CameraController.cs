using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool mouseCameraControl = true;
    public bool keyboardCameraControl = true;
    public float cameraSpeed = 0.3f;

    private void Update()
    {
        if (mouseCameraControl)
        {
            HandleMousePosition();
        }

        if (keyboardCameraControl)
        {
            HandleKeyboardInput();
        }
    }

    private void HandleMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;

        if (mousePos.x < 20f)
        {
            transform.position += Vector3.left * cameraSpeed;
        }

        if (mousePos.y < 20f)
        {
            transform.position += Vector3.down * cameraSpeed;
        }

        if (mousePos.x > Screen.width - 20f)
        {
            transform.position += Vector3.right * cameraSpeed;
        }

        if (mousePos.y > Screen.height - 20f)
        {
            transform.position += Vector3.up * cameraSpeed;
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * cameraSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * cameraSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * cameraSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * cameraSpeed;
        }
    }
}

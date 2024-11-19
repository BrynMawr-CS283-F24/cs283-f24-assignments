using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float moveSpeed = 10.0f;


    // Rotation variables
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the game window and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse movement to rotate the camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust camera rotation based on mouse input
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Prevent flipping over

        // Apply rotation
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);

        // Keyboard movement
        float moveForward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // W/S or Up/Down arrows
        float moveRight = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // A/D or Left/Right arrows

        // Apply movement relative to the camera's direction
        Vector3 move = transform.forward * moveForward + transform.right * moveRight;
        transform.position += move;
    }
}

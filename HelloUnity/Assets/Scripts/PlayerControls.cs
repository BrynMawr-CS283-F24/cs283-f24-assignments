using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Parameters to control speed
    public float movementSpeed =100.0f;
    public float turningSpeed = 150.0f;

    // Update is called once per frame
    void Update()
    {
        // Check for W key (move forward)
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }
        // Check for S key (move backward)
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * movementSpeed;
        }

        // Check for A key (turn left)
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -turningSpeed * Time.deltaTime);
        }
        // Check for D key (turn right)
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, turningSpeed * Time.deltaTime);
        }
    }
}

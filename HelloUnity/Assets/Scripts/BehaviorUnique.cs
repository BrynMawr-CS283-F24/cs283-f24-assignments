using UnityEngine;

public class BehaviorUnique : MonoBehaviour
{
    public Transform player; 
    public GameObject laser; 
    public float detectionRadius; // Range within which the player is detected
    public float rotationSpeed; // Speed at which the object rotates to follow the player
    public float fixedTiltAngle; // Fixed tilt angle

    void Start()
    {
        // Ensure the laser is initially disabled
        if (laser != null)
        {
            laser.SetActive(false);
        }

        // Set the initial tilt angle
        SetFixedTiltAngle();
    }

    void Update()
    {
        // Check if the player is within the detection radius
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            if (laser != null && !laser.activeSelf)
            {
                laser.SetActive(true);
            }

            // Calculate the direction to the player and keep only the horizontal component
            Vector3 direction = player.position - transform.position;
            direction.y = 0; 

            // Prevent errors if direction is zero (directly on top)
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                float yRotation = targetRotation.eulerAngles.y;
                Quaternion finalRotation = Quaternion.Euler(fixedTiltAngle, yRotation, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            if (laser != null && laser.activeSelf)
            {
                laser.SetActive(false);
            }
        }
    }

    void SetFixedTiltAngle()
    {
        // Set the initial tilt of the gun to the fixed tilt angle
        Vector3 initialEulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(fixedTiltAngle, initialEulerAngles.y, initialEulerAngles.z);
    }
}

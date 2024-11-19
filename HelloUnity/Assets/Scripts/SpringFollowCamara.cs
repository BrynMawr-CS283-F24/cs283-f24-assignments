using UnityEngine;

public class SpringFollowCamera : MonoBehaviour
{
    // Public parameters for the camera settings
    public Transform target;  // Reference to the player (or target) transform
    public float horizontalDistance;
    public float verticalDistance;
    public float dampConstant;  // Dampening factor
    public float springConstant;  // Spring constant

    private Vector3 velocity = Vector3.zero;  // Camera's current velocity

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for SpringFollowCamera.");
            return;
        }

        // Get the target's position, forward direction, and up direction
        Vector3 tPos = target.position;
        Vector3 tForward = target.forward;
        Vector3 tUp = target.up;

        // Calculate the ideal camera position (the position we want to achieve)
        Vector3 idealEye = tPos - tForward * horizontalDistance + tUp * verticalDistance;

        // Compute the displacement between the current camera position and the ideal position
        Vector3 displacement = transform.position - idealEye;

        // Compute the spring acceleration: 
        // F = -k * x (spring force) - dampConstant * velocity (damping force)
        Vector3 springAccel = (-springConstant * displacement) - (dampConstant * velocity);

        // Update the velocity using the computed spring acceleration
        velocity += springAccel * Time.deltaTime;

        // Update the camera's actual position using the new velocity
        transform.position += velocity * Time.deltaTime;

        // Calculate the direction the camera should look (from camera to the target)
        Vector3 cameraForward = tPos - transform.position;

        // Set the camera's rotation to look at the target
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }
}


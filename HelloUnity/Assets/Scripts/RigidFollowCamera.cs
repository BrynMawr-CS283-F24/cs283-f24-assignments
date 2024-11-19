using UnityEngine;

public class RigidFollowCamera : MonoBehaviour
{
    // Public parameters that can be set in the Unity Inspector
    public Transform target; // Reference to the player (or target) transform
    public float horizontalDistance;
    public float verticalDistance;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for RigidFollowCamera.");
            return;
        }

        // Get the target's position, forward direction, and up direction
        Vector3 tPos = target.position;
        Vector3 tForward = target.forward;
        Vector3 tUp = target.up;

        // Calculate the camera's position offset from the target
        Vector3 eye = tPos - tForward * horizontalDistance + tUp * verticalDistance;

        // Calculate the direction for the camera to look (from the camera position to the target)
        Vector3 cameraForward = tPos - eye;

        // Set the camera's position
        transform.position = eye;

        // Set the camera's rotation to look at the target
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }
}

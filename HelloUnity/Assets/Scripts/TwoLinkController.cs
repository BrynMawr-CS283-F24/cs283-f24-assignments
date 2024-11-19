using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLinkController : MonoBehaviour
{
    public Transform target;          // Target position to reach
    public Transform endEffector;   
    public Transform middleJoint;  
    public Transform baseJoint;       
    // Start is called before the first frame update
    void Start()
    {
        // Initialize joints if they are not already assigned
        if (middleJoint == null && endEffector != null)
        {
            middleJoint = endEffector.parent;
        }
        if (baseJoint == null && middleJoint != null)
        {
            baseJoint = middleJoint.parent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Step 1: Calculate distances (lengths of the segments)
        Vector3 R = target.position - baseJoint.position; 
        float r = R.magnitude; 

        Vector3 L1 = middleJoint.position - baseJoint.position;
        float l1 = L1.magnitude; 

        Vector3 L2 = endEffector.position - middleJoint.position;
        float l2 = L2.magnitude;

        // Check if the target is reachable
        if (r > l1 + l2)
        {
            Debug.LogWarning("Target is out of reach for the current leg configuration.");
            return;
        }

        // Calculate the knee (middle joint) angle using the law of cosines
        float cos = Mathf.Clamp((l1 * l1 + l2 * l2 - r * r) / (2 * l1 * l2), -1f, 1f);
        float elbowAngle = 180.0f - Mathf.Acos(cos) * Mathf.Rad2Deg; // Convert to degrees

        // Apply the knee rotation
        Vector3 rotationAxis = Vector3.Cross(L1, target.position - middleJoint.position).normalized;
        Quaternion kneeRotation = Quaternion.AngleAxis(elbowAngle, rotationAxis);
        middleJoint.rotation = kneeRotation * baseJoint.rotation; // Apply the knee rotation relative to the baseJoint

        //  Calculate and apply the hip (base joint) rotation to point towards the target
        Vector3 E = target.position - endEffector.position; // Vector from foot to target
        Vector3 cross = Vector3.Cross(R, E); // Axis of rotation
        float dot = Vector3.Dot(R, E);

        // Calculate the hip angle using atan2 for smooth movement
        float phi = Mathf.Atan2(cross.magnitude, Vector3.Dot(R, R) + dot) * Mathf.Rad2Deg;

        // Apply the rotation to the hip (base joint)
        Quaternion hipRotation = Quaternion.AngleAxis(phi, cross.normalized);
        baseJoint.rotation = hipRotation * baseJoint.rotation;

        // Debug:
        Debug.DrawLine(baseJoint.position, middleJoint.position, Color.red);   
        Debug.DrawLine(middleJoint.position, endEffector.position, Color.yellow);
        Debug.DrawLine(endEffector.position, target.position, Color.blue);      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeController : MonoBehaviour
{
    public Transform target;        
    public Transform lookJoint;     
    public Vector3 rotationOffset; 
    void Update()
    {
        // Compute the direction vector from the lookJoint to the target (r)
        Vector3 r = - (target.position - lookJoint.position);  
        Vector3 e = lookJoint.forward;  

        // Compute the cross product r × e 
        Vector3 crossProduct = Vector3.Cross(r, e);
        float crossMagnitude = crossProduct.magnitude;

        // Compute the dot product 
        float dotProduct1 = Vector3.Dot(r, r);
        float dotProduct2 = Vector3.Dot(r, e);

        // Compute the angle of rotation
        float angle = Mathf.Atan2(crossMagnitude, dotProduct1 + dotProduct2) * Mathf.Rad2Deg;

        Vector3 axis = crossProduct.normalized;
        Quaternion axisRotation = Quaternion.AngleAxis(angle, axis);
        lookJoint.parent.rotation = axisRotation * lookJoint.parent.rotation;
        Debug.DrawLine(lookJoint.position, target.position, Color.green);
    }
}

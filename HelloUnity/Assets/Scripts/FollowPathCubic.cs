using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathCubic : MonoBehaviour
{
    public List<Transform> pathPoints; 
    public float segmentDuration = 3.0f; 
    public bool DeCasteljau = false;
    private Vector3 b0, b1, b2, b3;   
    private int currentSegment = 0;     

    void Start()
    {
        if (pathPoints.Count < 2)
        {
            Debug.LogWarning("Not enough points to create a cubic Bezier curve.");
            return;
        }

        // Start following the path along multiple segments
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        for (currentSegment = 1; currentSegment < pathPoints.Count; currentSegment++)
        {
            Debug.Log($"Moving through segment {currentSegment - 1} to {currentSegment}"); // Debugging

            ComputeControlPoints(currentSegment);

            for (float t = 0; t <= 1f; t += Time.deltaTime / segmentDuration)
            {
                Vector3 position = DeCasteljau ? DeCasteljauBezier(t) : CubicBezier(t);
                transform.position = position;

                if (t > 0)
                {
                    Vector3 direction = (CubicBezier(t + 0.01f) - position).normalized;
                    if (direction != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                }

                yield return null;  // Wait for the next frame
            }
        }

        Debug.Log("Finished moving through all segments.");
    }

    void ComputeControlPoints(int segment)
    {
        // Get the endpoints for the current segment
        b0 = pathPoints[segment - 1].position;
        b3 = pathPoints[segment].position;  

        // Compute b1 (control point) for the first segment
        if (segment == 1)
        {
            b1 = b0 + (1f / 6f) * (b3 - b0);  // Special case for the first segment
        }
        else
        {
            b1 = b0 + (1f / 6f) * (pathPoints[segment].position - pathPoints[segment - 2].position);
        }

        // Compute b2 (control point) for the last segment
        if (segment == pathPoints.Count - 1)  // Last segment
        {
            b2 = b3 - (1f / 6f) * (b3 - b0);
        }
        else
        {
            b2 = b3 - (1f / 6f) * (pathPoints[segment + 1].position - pathPoints[segment - 1].position);
        }

        Debug.Log($"Control points for segment {segment - 1} to {segment}: b0 = {b0}, b1 = {b1}, b2 = {b2}, b3 = {b3}");
    }

    Vector3 CubicBezier(float t)
    {
        float u = 1 - t;
        return u * u * u * b0 + 3 * u * u * t * b1 + 3 * u * t * t * b2 + t * t * t * b3;
    }

    Vector3 DeCasteljauBezier(float t)
    {
        // De Casteljau's algorithm for Bezier curve calculation
        Vector3 q0 = Vector3.Lerp(b0, b1, t);
        Vector3 q1 = Vector3.Lerp(b1, b2, t);
        Vector3 q2 = Vector3.Lerp(b2, b3, t);

        Vector3 r0 = Vector3.Lerp(q0, q1, t);
        Vector3 r1 = Vector3.Lerp(q1, q2, t);

        return Vector3.Lerp(r0, r1, t);
    }
}

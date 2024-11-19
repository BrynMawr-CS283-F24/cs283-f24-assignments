using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathLinear : MonoBehaviour
{
    public List<Transform> pathPoints;  // Points specifying the path
    public float duration = 3.0f;       // Time to move between points
    private int currentPoint = 0;       // Tracks the current point index
    private bool isMoving = false;      // Prevents overlapping coroutines

    void Start()
    {
        if (pathPoints.Count > 1)
        {
            StartCoroutine(FollowPath());
        }
        else
        {
            Debug.LogWarning("Not enough points to follow a path.");
        }
    }

    IEnumerator FollowPath()
    {
        while (currentPoint < pathPoints.Count - 1)  // Stop before the last point
        {
            if (!isMoving)
            {
                isMoving = true;

                // Get the start and end points for movement
                Transform start = pathPoints[currentPoint];
                Transform end = pathPoints[currentPoint + 1];  // Move to the next point

                // Move between the current and next point using Lerp
                for (float timer = 0; timer < duration; timer += Time.deltaTime)
                {
                    float u = timer / duration;

                    // Linear interpolation between positions
                    transform.position = Vector3.Lerp(start.position, end.position, u);

                    // Rotate to face the direction of movement
                    Vector3 direction = (end.position - start.position).normalized;
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, u);
                    }

                    yield return null;  // Wait for the next frame
                }

                // After reaching the next point, update the current point index
                currentPoint++;

                // Done moving, allow the next step
                isMoving = false;
            }

            // Wait a frame before moving to the next point
            yield return null;
        }
    }
}

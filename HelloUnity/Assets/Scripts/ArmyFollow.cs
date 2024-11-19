using UnityEngine;

public class ArmyFollow : MonoBehaviour
{
    public Transform player; 
    public float followDistance; // Distance to maintain behind the player

    private Vector3 previousPlayerPosition; 
    private float playerSpeed; 

    void Start()
    {
        if (player != null)
        {
            previousPlayerPosition = player.position; // Initialize with the player's starting position
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the player's speed based on position change
            playerSpeed = (player.position - previousPlayerPosition).magnitude / Time.deltaTime;
            previousPlayerPosition = player.position; // Update the previous position

            // Calculate the target position behind the player
            Vector3 targetPosition = player.position - player.forward * followDistance;

            // Move the army unit towards the target position at the player's speed
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, playerSpeed * Time.deltaTime);

            // Rotate the army unit to match the player's rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, Time.deltaTime * playerSpeed);
        }
    }
}

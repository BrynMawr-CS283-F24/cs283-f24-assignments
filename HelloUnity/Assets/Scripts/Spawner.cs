using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject template; 
    public float spawnRange = 5f; // Radius around the spawner to spawn objects
    public int maxSpawnedObjects = 5; // Maximum number of objects to spawn at a time

    private GameObject[] spawnedObjects;

    void Start()
    {
        // Initialize the array to store spawned objects
        spawnedObjects = new GameObject[maxSpawnedObjects];

        // Spawn the initial objects
        for (int i = 0; i < maxSpawnedObjects; i++)
        {
            SpawnObject(i);
        }
    }

    void Update()
    {
        // Check for picked-up objects and respawn them if necessary
        for (int i = 0; i < maxSpawnedObjects; i++)
        {
            if (spawnedObjects[i] != null && !spawnedObjects[i].activeInHierarchy)
            {
                // Respawn the object at a new random location
                SpawnObject(i);
            }
        }
    }

    private void SpawnObject(int index)
    {
        // Generate a random position within the spawn range
        Vector3 randomPosition = GetRandomPosition();

        // If this is a respawn, set the position and reactivate
        if (spawnedObjects[index] != null)
        {
            spawnedObjects[index].transform.position = randomPosition;
            spawnedObjects[index].SetActive(true);
        }
        else
        {
            // Create a new object if it doesn¡¯t exist
            spawnedObjects[index] = Instantiate(template, randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPosition()
    {
        // Generate a random point within the spawn range
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        // Calculate the new position relative to the Spawner's position
        Vector3 position = transform.position + new Vector3(x, 0, z);

        // Adjust y position to sit on top of the terrain, based on the collider bounds
        if (template.TryGetComponent(out Collider collider))
        {
            position.y += collider.bounds.extents.y;
        }

        return position;
    }
}

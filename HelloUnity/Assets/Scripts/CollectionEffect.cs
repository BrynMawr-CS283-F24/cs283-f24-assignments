using UnityEngine;
using System.Collections;

public class CollectionEffect : MonoBehaviour
{
    public ParticleSystem collectionParticles; // Optional particle effect on collection
    public AudioSource collectionSound; // Optional sound effect on collection
    public float riseDuration = 1.6f; // Duration of the rising effect
    public float riseHeight = 20f; // Height to rise before disappearing

    public void TriggerEffect()
    {
        // Play particles if they exist
        if (collectionParticles != null)
        {
            collectionParticles.Play();
        }

        // Play sound if it exists
        if (collectionSound != null)
        {
            collectionSound.Play();
        }

        // Start the rise-up effect
        StartCoroutine(RiseUpAndDisappear());
    }

    private IEnumerator RiseUpAndDisappear()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, riseHeight, 0);

        while (elapsedTime < riseDuration)
        {
            // Increment elapsed time by the time passed since last frame
            elapsedTime += Time.deltaTime;

            // Lerp position between start and end based on elapsed time
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / riseDuration);

            // Wait for the next frame
            yield return null;
        }

        // Hide the object after the rising effect completes
        gameObject.SetActive(false);
    }
}

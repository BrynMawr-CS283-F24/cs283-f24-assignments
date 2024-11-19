using UnityEngine;
using TMPro;
using System.Collections; // Add this line

public class CollectionGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {

            // Trigger the rise-up effect
            var effect = other.GetComponent<CollectionEffect>();
            if (effect != null)
            {
                effect.TriggerEffect();
            }
            else
            {
                Debug.LogWarning("CollectionEffect component not found on collected object.");
            }

            // Delay hiding the object to allow the effect to complete
            StartCoroutine(HideObjectAfterDelay(other.gameObject, effect.riseDuration));
        }
    }


    private IEnumerator HideObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false); // Hide the object after the effect completes
    }
}



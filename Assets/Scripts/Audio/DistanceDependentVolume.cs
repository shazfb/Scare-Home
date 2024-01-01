using UnityEngine;

public class DistanceDependentVolume : MonoBehaviour
{
    public Transform playerTransform;
    public AudioSource audioSource;
    public float maxDistance = 10f; // Adjust this value based on your desired maximum distance

    void Update()
    {
        if (playerTransform == null || audioSource == null)
            return;

        // Calculate the distance between the audio source and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Normalize the distance to a range between 0 and 1
        float normalizedDistance = Mathf.Clamp01(distanceToPlayer / maxDistance);

        // Set the volume based on the normalized distance
        audioSource.volume = 1f - normalizedDistance; // Invert the value to make closer sounds louder
    }
}

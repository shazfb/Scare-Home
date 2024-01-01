using UnityEngine;

public class DistanceDependentVolume : MonoBehaviour
{
    public Transform playerTransform;
    public AudioSource audioSource;
    public float maxDistance = 10f; 

    public void Update()
    {
        if (playerTransform == null || audioSource == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        float normalizedDistance = Mathf.Clamp01(distanceToPlayer / maxDistance);

        audioSource.volume = 1f - normalizedDistance; 
    }
}

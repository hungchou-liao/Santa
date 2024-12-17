using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlyOutController : MonoBehaviour
{
    [Header("Fly Out Settings")]
    public float delayTime = 1f;           // Delay before the image starts moving
    public float duration = 2f;            // Duration of the fly-out animation
    public Vector3 flyOutDirection = Vector3.up; // Direction to fly out (default is up)
    public float flyOutDistance = 10f;     // Distance to move in the given direction

    [Header("Sound Effect")]
    public AudioClip flyOutSound;          // Sound effect to play
    private AudioSource audioSource;

    private Vector3 startPosition;         // Initial position of the GameObject
    private bool hasStarted = false;

    void Start()
    {
        // Store the starting position
        startPosition = transform.position;

        // Initialize AudioSource if a sound effect is provided
        if (flyOutSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = flyOutSound;
        }

        // Start the fly-out process with a delay
        StartCoroutine(StartFlyOut());
    }

    IEnumerator StartFlyOut()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Play the sound effect if available
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Begin moving the image
        StartCoroutine(FlyOutAnimation());
    }

    IEnumerator FlyOutAnimation()
    {
        hasStarted = true;
        float elapsedTime = 0f;

        // Calculate the target position based on direction and distance
        Vector3 targetPosition = startPosition + (flyOutDirection.normalized * flyOutDistance);

        // Move the GameObject smoothly over the duration
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it reaches the target position
        transform.position = targetPosition;

        // Optionally, disable or destroy the GameObject after flying out
        Debug.Log("Fly-out complete!");
        gameObject.SetActive(false); // Hide the GameObject
    }

    // Optional debug to reset position for testing
    private void OnDisable()
    {
        if (hasStarted)
        {
            transform.position = startPosition;
        }
    }
}
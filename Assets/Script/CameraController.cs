using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float shakeStartDelay = 55f;   // Delay before the first shake starts
    public float shakeDuration = 0.5f;  // Duration of each shake
    public float shakeMagnitude = 0.2f; // Magnitude of each shake

    public float sceneCheckDelay = 60f; // Delay before checking the scene
    public float moveDuration = 2f;     // Duration of the camera move
    public float moveOffsetY = -20f;    // Offset to move the camera down

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;

        // Start the first shake at the beginning
        StartCoroutine(FirstShakeAndCheckScene());
    }

    private IEnumerator FirstShakeAndCheckScene()
    {
        // First Shake at the beginning
        yield return StartCoroutine(Shake());

        // Wait for 65 seconds, then check the scene
        yield return new WaitForSeconds(sceneCheckDelay);

        // Check if any objects tagged as "Present" are still in the scene
        GameObject[] presents = GameObject.FindGameObjectsWithTag("Present");

        if (presents.Length > 0) // If presents are still present
        {
            Debug.Log("Presents found! Shaking camera again and moving down.");

            // Second shake before moving the camera
            yield return StartCoroutine(Shake());

            // Move the camera down
            yield return StartCoroutine(MoveCameraDown());
        }
        else
        {
            Debug.Log("No presents found. Camera stays in position.");
        }
    }

    public IEnumerator Shake()
    {
        Debug.Log("Camera shake started.");
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset the camera to its original position
        transform.position = originalPosition;
        Debug.Log("Camera shake completed.");
    }

    private IEnumerator MoveCameraDown()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y + moveOffsetY, startPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the camera reaches the exact target position
        transform.position = targetPosition;
        Debug.Log("Camera move completed.");
    }
}
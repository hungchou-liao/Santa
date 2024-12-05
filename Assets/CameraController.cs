using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float shakeStartDelay = 0f;   // Delay before the shake starts
    public float shakeDuration = 0.5f;  // Duration of the shake
    public float shakeMagnitude = 0.2f; // Magnitude of the shake

    public IEnumerator Shake()
    {
        // Wait for the specified delay before starting the shake
        if (shakeStartDelay > 0)
        {
            yield return new WaitForSeconds(shakeStartDelay);
        }

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
    }
}
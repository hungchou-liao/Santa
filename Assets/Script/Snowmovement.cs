using UnityEngine;

public class SnowFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera

    void Update()
    {
        // Make the snow particle system follow the camera
        Vector3 newPosition = cameraTransform.position;
        newPosition.y = transform.position.y; // Keep the snow at the same height
        transform.position = newPosition;
    }
}
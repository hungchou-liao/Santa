using UnityEngine;

public class Present : MonoBehaviour
{
    void OnDestroy()
    {
        // Notify the PresentStatusManager when this present is destroyed
        FindObjectOfType<PresentStatusManager>()?.PresentDestroyed();
    }
}
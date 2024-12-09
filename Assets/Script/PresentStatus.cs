using UnityEngine;
using UnityEngine.UI;

public class PresentStatusManager : MonoBehaviour
{
    public Image statusImage;         // The UI Image to display the present status
    public Sprite[] statusSprites;   // Array of sprites for statuses (0/5, 1/5, ..., 5/5)

    private int totalPresents;       // Total number of presents at the start
    private int currentPresents;     // Current number of presents in the scene

    void Start()
    {
        if (statusImage == null || statusSprites == null || statusSprites.Length == 0)
        {
            Debug.LogError("PresentStatusManager is not properly configured. Ensure Status Image and Status Sprites are assigned.");
            return;
        }

        // Initialize the total presents and current presents
        totalPresents = GameObject.FindGameObjectsWithTag("Present").Length;
        currentPresents = totalPresents;

        UpdateStatus(); // Update the initial status
    }

    public void PresentDestroyed()
    {
        if (statusImage == null || statusSprites == null || statusSprites.Length == 0)
        {
            Debug.LogError("PresentStatusManager is not properly configured. Ensure Status Image and Status Sprites are assigned.");
            return;
        }

        // Decrease the present count
        currentPresents--;

        // Ensure the count does not go below zero
        currentPresents = Mathf.Clamp(currentPresents, 0, totalPresents);

        // Update the status display
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (statusImage == null || statusSprites == null || statusSprites.Length == 0)
        {
            Debug.LogError("PresentStatusManager is not properly configured. Ensure Status Image and Status Sprites are assigned.");
            return;
        }

        // Calculate the index for the statusSprites array
        int index = totalPresents - currentPresents;

        // Ensure the index is valid
        if (index >= 0 && index < statusSprites.Length)
        {
            statusImage.sprite = statusSprites[index];
        }
        else
        {
            Debug.LogWarning("Index out of range for statusSprites!");
        }
    }
}
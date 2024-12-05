using System.Collections;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer; // Reference to the background SpriteRenderer
    public Sprite initialBackground;         // First background image
    public Sprite nextBackground;            // Second background image
    public float changeAfterSeconds = 30f;   // Time in seconds to switch the background
    public AudioClip changeSound;            // Sound to play when the background changes
    public CameraController cameraController; // Reference to the CameraController

    private AudioSource audioSource;         // AudioSource for playing sound
    private bool hasChanged = false;         // Flag to ensure the background changes only once

    void Start()
    {
        if (backgroundRenderer == null)
        {
            Debug.LogError("BackgroundRenderer is not assigned!");
            return;
        }

        // Add or get an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (initialBackground != null)
        {
            backgroundRenderer.sprite = initialBackground; // Set the initial background
        }

        // Start the timer to change the background
        StartCoroutine(ChangeBackgroundAfterTime());
    }

    private IEnumerator ChangeBackgroundAfterTime()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(changeAfterSeconds);

        // Change to the next background and trigger the effects
        if (!hasChanged && nextBackground != null)
        {
            backgroundRenderer.sprite = nextBackground;
            hasChanged = true; // Ensure it only changes once
            Debug.Log("Background has been changed to the next image.");

            // Play the sound effect
            PlayChangeSound();

            // Trigger the shake effect
            if (cameraController != null)
            {
                StartCoroutine(cameraController.Shake());
            }
        }
    }

    private void PlayChangeSound()
    {
        if (changeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(changeSound);
        }
    }
}
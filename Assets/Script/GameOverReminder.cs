using UnityEngine;
using System.Collections;

public class GameoverReminder : MonoBehaviour
{
    [Header("UI Animation Settings")]
    public float checkDelay = 65f;      // Delay before checking for presents
    public float enterDuration = 0.5f; // Duration for fade-in
    public float stayDuration = 5f;    // Duration to stay visible
    public float exitDuration = 0.5f;  // Duration for fade-out
    public AudioClip gameOverSound;    // Sound effect for game over

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool gameOverTriggered = false; // Ensures it triggers only once

    void Start()
    {
        // Add CanvasGroup for fading
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // Start invisible

        // Add AudioSource for playing sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start the game over check coroutine
        StartCoroutine(CheckForGameOver());
    }

    IEnumerator CheckForGameOver()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(checkDelay);

        // Check if there are no "Present" objects in the scene
        GameObject[] presents = GameObject.FindGameObjectsWithTag("Present");
        if (presents.Length == 0 && !gameOverTriggered) // Trigger if no presents exist
        {
            Debug.Log("Game Over: No presents found!");
            gameOverTriggered = true; // Prevent multiple triggers
            StartCoroutine(ShowGameOverMessage());
        }
    }

    IEnumerator ShowGameOverMessage()
    {
        // Play game over sound if provided
        PlayGameOverSound();

        // Fade in the Game Over message
        yield return Fade(0, 1, enterDuration);

        // Stay visible for the duration
        yield return new WaitForSeconds(stayDuration);

        // Fade out the Game Over message
        yield return Fade(1, 0, exitDuration);

        // Optionally, deactivate the GameObject
        gameObject.SetActive(false);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            audioSource.clip = gameOverSound;
            audioSource.Play();
        }
    }
}
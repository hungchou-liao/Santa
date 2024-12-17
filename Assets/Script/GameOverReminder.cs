using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverReminder : MonoBehaviour
{
    [Header("UI Animation Settings")]
    public float enterDuration = 0.5f;  // Duration for fade-in
    public float stayDuration = 2f;     // Duration to stay visible
    public float exitDuration = 0.5f;   // Duration for fade-out
    public AudioClip enterSound;        // Sound effect for entering

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

        // Start monitoring for presents
        StartCoroutine(CheckForGameOver());
    }

    IEnumerator CheckForGameOver()
    {
        while (!gameOverTriggered)
        {
            // Check if no "Present" objects exist in the scene
            GameObject[] presents = GameObject.FindGameObjectsWithTag("Present");
            if (presents.Length == 0)
            {
                Debug.Log("Game Over: No presents remain!");
                gameOverTriggered = true; // Prevent multiple triggers
                StartCoroutine(ShowGameOver());
            }

            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }

    IEnumerator ShowGameOver()
    {
        // Play enter sound if provided
        PlayEnterSound();

        // Fade in the Game Over message
        yield return Fade(0, 1, enterDuration);

        // Stay visible
        yield return new WaitForSeconds(stayDuration);

        // Fade out
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

    void PlayEnterSound()
    {
        if (enterSound != null)
        {
            audioSource.clip = enterSound;
            audioSource.Play();
        }
    }
}
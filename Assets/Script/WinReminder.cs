using UnityEngine;
using System.Collections;

public class WinReminder : MonoBehaviour
{
    [Header("UI Animation Settings")]
    public float checkDelay = 60f;      // Delay before checking for presents
    public float enterDuration = 0.5f;  // Duration for fade-in
    public float stayDuration = 5f;     // Duration to stay visible
    public float exitDuration = 0.5f;   // Duration for fade-out
    public AudioClip winSound;          // Sound effect for winning

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool winTriggered = false;  // Ensures it triggers only once

    void Start()
    {
        // Add CanvasGroup for fading
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // Start invisible

        // Add AudioSource for playing sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start the win-checking coroutine
        StartCoroutine(CheckForWin());
    }

    IEnumerator CheckForWin()
    {
        // Wait for the specified delay (60 seconds)
        yield return new WaitForSeconds(checkDelay);

        // Check if any "Present" objects exist in the scene
        GameObject[] presents = GameObject.FindGameObjectsWithTag("Present");
        if (presents.Length > 0 && !winTriggered)
        {
            Debug.Log("Win: Presents still exist!");
            winTriggered = true; // Prevent multiple triggers
            StartCoroutine(ShowWinMessage());
        }
    }

    IEnumerator ShowWinMessage()
    {
        // Play win sound if provided
        PlayWinSound();

        // Fade in the Win message
        yield return Fade(0, 1, enterDuration);

        // Stay visible for the duration
        yield return new WaitForSeconds(stayDuration);

        // Fade out the Win message
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

    void PlayWinSound()
    {
        if (winSound != null)
        {
            audioSource.clip = winSound;
            audioSource.Play();
        }
    }
}
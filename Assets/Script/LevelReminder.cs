using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelReminder : MonoBehaviour
{
    public float enterDelay = 1f;        // Delay before fade-in starts
    public float enterDuration = 0.5f;  // Duration for fade-in
    public float stayDuration = 2f;     // Duration to stay visible
    public float exitDuration = 0.5f;   // Duration for fade-out
    public AudioClip enterSound;        // Sound effect for entering

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    void Start()
    {
        // Ensure the image has a CanvasGroup for fading
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // Start invisible

        // Add AudioSource component for playing sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start the fade effect with enter delay
        StartCoroutine(FadeEffect());
    }

    IEnumerator FadeEffect()
    {
        // Wait for the enter delay
        yield return new WaitForSeconds(enterDelay);

        // Play enter sound
        PlayEnterSound();

        // Fade in
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
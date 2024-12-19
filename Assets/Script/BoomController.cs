using UnityEngine;
using System.Collections;

public class BoomController : MonoBehaviour
{
    public float enterDelay = 1f;        // Delay before scaling and fade-in
    public float scaleDuration = 0.5f;  // Duration for scaling from A to B
    public float stayDuration = 2f;     // Duration for the image to stay visible
    public float exitDuration = 2f;     // Duration for fade-out
    public Vector3 scaleStart = Vector3.one * 0.5f;  // Initial scale (A)
    public Vector3 scaleEnd = Vector3.one;           // Final scale (B)
    public AudioClip boomSound;          // Sound effect for scaling

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    void Start()
    {
        // Initialize CanvasGroup for fading
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // Start invisible

        // Add AudioSource for sound effects
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Set the initial scale
        transform.localScale = scaleStart;

        // Start the Boom effect
        StartCoroutine(BoomEffect());
    }

    IEnumerator BoomEffect()
    {
        // Wait for the enter delay
        yield return new WaitForSeconds(enterDelay);

        // Play Boom sound
        PlayBoomSound();

        // Scale from A to B
        yield return ScaleOverTime(scaleStart, scaleEnd, scaleDuration);

        // Ensure fully visible before staying
        canvasGroup.alpha = 1f;

        // Stay visible for the specified duration
        yield return new WaitForSeconds(stayDuration);

        // Fade out
        yield return FadeOut(exitDuration);

        // Optionally, deactivate the GameObject
        gameObject.SetActive(false);
    }

    IEnumerator ScaleOverTime(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = to; // Ensure exact end scale
    }

    IEnumerator FadeOut(float duration)
    {
        float elapsed = 0f;
        float startAlpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0; // Ensure full fade-out
    }

    void PlayBoomSound()
    {
        if (boomSound != null)
        {
            audioSource.clip = boomSound;
            audioSource.Play();
        }
    }
}
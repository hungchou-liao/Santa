using System.Collections;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public Sprite defaultFloorImage;     // The initial floor sprite
    public Sprite boomedFloorImage;      // Floor image when no present is found
    public Sprite brokenFloorImage;      // Floor image when present is found

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    public float checkDelay = 65f;       // Delay before checking for presents

    void Start()
    {
        // Initialize components
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Set the default floor image and enable the collider
        spriteRenderer.sprite = defaultFloorImage;
        boxCollider.enabled = true;

        // Start the coroutine to check the scene
        StartCoroutine(CheckSceneAndUpdateFloor());
    }

    private IEnumerator CheckSceneAndUpdateFloor()
    {
        // Wait for the specified delay (65 seconds)
        yield return new WaitForSeconds(checkDelay);

        // Check if any objects tagged as "Present" are still in the scene
        GameObject[] presents = GameObject.FindGameObjectsWithTag("Present");

        if (presents.Length > 0)
        {
            // If there are presents, change to the broken floor and disable the collider
            spriteRenderer.sprite = brokenFloorImage;
            boxCollider.enabled = false; // Santa and presents will fall
        }
        else
        {
            // If no presents are found, change to the boomed floor and keep the collider
            spriteRenderer.sprite = boomedFloorImage;
            boxCollider.enabled = true; // The collider remains
        }
    }
}
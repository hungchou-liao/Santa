using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaController : MonoBehaviour
{
    public float moveSpeed = 5f;         // Speed for horizontal movement
    public float jumpForce = 10f;        // Force for jumping
    public GameObject snowballPrefab;    // Reference to the Snowball Prefab
    public float throwForce = 10f;       // Force to throw the snowball
    public float throwCooldown = 0.5f;   // Cooldown time between snowball throws

    private Rigidbody2D rb;              // Reference to the Rigidbody2D component
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private bool canJump = true;         // Flag to control jumping
    private float nextThrowTime = 0f;    // Time tracker for snowball cooldown

    void Start()
    {
        // Get the Rigidbody2D and SpriteRenderer components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Horizontal movement
        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        // Flip Santa's sprite based on direction
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }

        // Jumping (only allowed once until landing)
        if (canJump && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false; // Disable further jumps until landing
        }

        // Throw snowball on left mouse click, with cooldown
        if (Input.GetMouseButtonDown(0) && Time.time >= nextThrowTime)
        {
            // Check the direction of the mouse position relative to Santa
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x < transform.position.x)
            {
                spriteRenderer.flipX = true; // Face left
            }
            else
            {
                spriteRenderer.flipX = false; // Face right
            }

            // Throw the snowball
            ThrowSnowball(mousePosition);
            nextThrowTime = Time.time + throwCooldown; // Set next allowable throw time
        }
    }

    // Function to throw a snowball towards the specified target position
    private void ThrowSnowball(Vector3 targetPosition)
    {
        // Determine spawn position based on the direction Santa is facing
        Vector3 spawnPosition = transform.position + (spriteRenderer.flipX ? Vector3.left : Vector3.right) * 0.5f;

        // Instantiate the snowball at the calculated position
        GameObject snowball = Instantiate(snowballPrefab, spawnPosition, Quaternion.identity);

        // Calculate direction based on the target position (mouse position)
        Vector2 direction = (targetPosition - transform.position).normalized;

        // Get the Rigidbody2D of the snowball and apply force in the calculated direction
        Rigidbody2D snowballRb = snowball.GetComponent<Rigidbody2D>();
        snowballRb.AddForce(direction * throwForce, ForceMode2D.Impulse);
    }

    // Reset canJump when Santa is touching the ground
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f) // Check if the collision is from below
            {
                canJump = true;
                break; // Exit the loop once we confirm it's grounded
            }
        }
    }

    // Disable jumping when Santa is no longer touching the ground
    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
    }
}
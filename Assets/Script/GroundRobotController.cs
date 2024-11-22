using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundRobotController : MonoBehaviour
{
    public float moveSpeed = 2f;                    // Speed of the robot
    public int health = 3;                          // Health for the robot (3 hits needed to destroy it)
    public float escapeDistance = 5f;               // Additional distance beyond screen boundary for escape

    public Image[] lifeImages;                      // Array of Image components for life indicators

    private GameObject targetPresent = null;        // The specific present the robot is targeting
    private bool hasPresent = false;                // Flag to check if the robot is carrying a present
    private Vector3 escapeDirection;                // Direction to move the present away from the pickup position
    private SpriteRenderer spriteRenderer;          // Reference to the SpriteRenderer for flipping

    // Screen boundary limits
    private float screenLeft = -16f;
    private float screenRight = 16f;
    private float screenTop = 12f;
    private float screenBottom = -12f;

    void Start()
    {
        // Get the SpriteRenderer component for flipping the robot
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Initialize the life display to full health at the start
        UpdateLifeDisplay();

        // Find the closest present at the start
        FindClosestPresent();
    }

    void UpdateLifeDisplay()
    {
        // Show or hide each life indicator based on the current health
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].enabled = i < health; // Enable the image if within health range, disable if beyond health range
        }
    }

    void Update()
    {
        if (targetPresent != null)
        {
            if (hasPresent)
            {
                // Move in the escape direction with the present attached
                MoveInEscapeDirection();

                // Check if the robot has moved beyond the extended screen bounds
                IsBeyondExtendedBounds(transform.position); // Robot will destroy itself here
            }
            else
            {
                // Move towards the target present if the robot doesn't have it
                MoveTowards(targetPresent.transform.position);
            }
        }
        else
        {
            // If there is no present, look for the closest one
            FindClosestPresent();
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void MoveInEscapeDirection()
    {
        // Move in the direction opposite from where the present was picked up
        transform.position += escapeDirection * moveSpeed * Time.deltaTime;
    }

    bool IsBeyondExtendedBounds(Vector3 position)
    {
        // Check if the position is outside the screen boundaries plus escapeDistance
        if (position.x < screenLeft - escapeDistance ||
            position.x > screenRight + escapeDistance ||
            position.y < screenBottom - escapeDistance ||
            position.y > screenTop + escapeDistance)
        {
            // Destroy the robot once it's out of bounds
            Destroy(gameObject);
            Debug.Log("GroundRobot disappeared after moving out of bounds!");
            return true;
        }
        return false;
    }

    void FindClosestPresent()
    {
        // Find all presents in the scene
        GameObject[] presents = GameObject.FindGameObjectsWithTag("Present");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject present in presents)
        {
            float distance = Vector2.Distance(transform.position, present.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetPresent = present; // Only target the closest present
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Present") && !hasPresent)
        {
            // Only interact with the specific present that the robot collides with
            if (collision.gameObject == targetPresent)
            {
                // Attach the present to the robot
                targetPresent.transform.SetParent(transform); // Make the present a child of the robot
                hasPresent = true;

                // Calculate the escape direction (opposite direction from where it came)
                escapeDirection = (transform.position - targetPresent.transform.position).normalized;

                Debug.Log("GroundRobot attached to present!");
            }
        }
        else if (collision.gameObject.CompareTag("Snowball"))
        {
            // Reduce health by 1 for each snowball hit
            health -= 1;
            Debug.Log("GroundRobot hit by snowball! Remaining health: " + health);

            // Update the life display after taking damage
            UpdateLifeDisplay();

            // Destroy the snowball upon collision with the robot
            Destroy(collision.gameObject);

            // Drop the present when hit by a snowball
            DropPresent();

            // Check if health is zero or below
            if (health <= 0)
            {
                // Destroy the robot if health reaches zero
                Destroy(gameObject);
                Debug.Log("GroundRobot destroyed!");
            }
        }
    }

    // Method to drop the present without destroying it
    private void DropPresent()
    {
        if (hasPresent)
        {
            // Detach the present from the robot
            targetPresent.transform.SetParent(null); // Remove parent relationship
            targetPresent = null;                    // Clear the reference to the present
            hasPresent = false;                      // Reset the hasPresent flag

            // Ensure the present stays in its current position
            Debug.Log("Present dropped by the robot!");
        }
    }

    // Method to flip the robot's direction (no longer called on snowball hit)
    private void FlipDirection()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
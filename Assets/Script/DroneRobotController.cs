using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{
    public float moveSpeed = 3f;                     // Speed of the drone
    public int health = 2;                           // Health for the drone (2 hits needed to destroy it)
    public float escapeHeight = 20f;                 // Height at which the drone escapes the screen

    public Image[] healthImages;                     // UI for displaying drone health
    private GameObject targetPresent = null;         // The specific present the drone is targeting
    private bool hasPresent = false;                 // Flag to check if the drone is carrying a present

    void Start()
    {
        // Initialize the health UI
        UpdateHealthUI();

        // Find the closest present to the drone
        FindClosestPresent();
    }

    void Update()
    {
        if (hasPresent)
        {
            // Fly upward with the present
            FlyAway();

            // Destroy the drone and present when it moves beyond the escape height
            if (transform.position.y > escapeHeight)
            {
                if (targetPresent != null)
                {
                    Debug.Log("Drone successfully escaped with the present!");
                    Destroy(targetPresent); // Destroy the present
                }
                Destroy(gameObject); // Destroy the drone
            }
        }
        else if (targetPresent != null)
        {
            // Move towards the target present if the drone doesn't have it
            MoveTowards(targetPresent.transform.position);
        }
    }

    void MoveTowards(Vector3 target)
    {
        // Move towards the target
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void FlyAway()
    {
        // Move upward
        Vector3 direction = Vector3.up;
        transform.position += direction * moveSpeed * Time.deltaTime;
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

        if (targetPresent != null)
        {
            Debug.Log("Closest present found: " + targetPresent.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Present") && !hasPresent)
        {
            // Only interact with the specific present that the drone is targeting
            if (collider.gameObject == targetPresent)
            {
                Debug.Log("Drone collided with Present: " + collider.gameObject.name);

                // Attach the present to the drone
                AttachPresentToDrone(collider.gameObject);

                hasPresent = true; // Start flying away
                Debug.Log("Drone successfully attached to Present!");
            }
        }
        else if (collider.gameObject.CompareTag("Snowball"))
        {
            Debug.Log($"Drone hit by Snowball at position {transform.position}");

            // Reduce health by 1 for each snowball hit
            health -= 1;
            Debug.Log("Drone hit! Health remaining: " + health);

            // Update the health UI
            UpdateHealthUI();

            // Check if health is zero or below
            if (health <= 0)
            {
                DestroyDrone();
            }

            // Destroy the snowball upon collision with the drone
            Destroy(collider.gameObject);
        }
    }

    private void AttachPresentToDrone(GameObject present)
    {
        // Parent the present to the drone
        present.transform.SetParent(transform);

        // Position the present slightly below the drone
        present.transform.localPosition = new Vector3(0, -2.0f, 0); // Adjust Y-axis (-0.5f) to place it below the drone

        // Disable the Rigidbody2D on the present to stop physics
        Rigidbody2D presentRb = present.GetComponent<Rigidbody2D>();
        if (presentRb != null)
        {
            presentRb.simulated = false;
        }

        // Disable the collider on the present to prevent further collisions
        Collider2D presentCollider = present.GetComponent<Collider2D>();
        if (presentCollider != null)
        {
            presentCollider.enabled = false;
        }
    }

    private void DestroyDrone()
    {
        if (hasPresent && targetPresent != null)
        {
            Debug.Log("Drone destroyed while carrying the present!");

            // Detach the present from the drone
            targetPresent.transform.SetParent(null); // Remove parent relationship

            // Re-enable the Rigidbody2D on the present for gravity
            Rigidbody2D presentRb = targetPresent.GetComponent<Rigidbody2D>();
            if (presentRb != null)
            {
                presentRb.simulated = true; // Re-enable physics
                presentRb.gravityScale = 1f; // Ensure gravity affects the present
            }

            // Re-enable the collider on the present
            Collider2D presentCollider = targetPresent.GetComponent<Collider2D>();
            if (presentCollider != null)
            {
                presentCollider.enabled = true;
            }

            targetPresent = null; // Clear the reference to the present
            hasPresent = false;   // Reset the flag
        }

        // Destroy the drone
        Destroy(gameObject);
        Debug.Log("Drone destroyed!");
    }

    private void UpdateHealthUI()
    {
        // Update the health UI based on the current health
        for (int i = 0; i < healthImages.Length; i++)
        {
            healthImages[i].enabled = i < health; // Show full health for indices below current health
        }
    }
}
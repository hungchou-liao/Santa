using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public float lifetime = 3.0f; // Public variable for the snowball's lifetime

    void Start()
    {
        // Destroy the snowball after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Destroy the snowball if it goes off-screen
        if (transform.position.magnitude > 20f) // Adjust 20f as per your scene size
        {
            Destroy(gameObject);
        }
    }

    // Destroy the snowball when it collides with an enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Ensure enemies have the "Enemy" tag
        {
            Destroy(gameObject); // Destroy the snowball
            Destroy(collision.gameObject); // Destroy the enemy (optional)
        }
    }
}
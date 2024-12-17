using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject droneRobotPrefab;             // Prefab for drones
    public GameObject groundRobotLeftPrefab;        // Prefab for ground robots (left)
    public GameObject groundRobotRightPrefab;       // Prefab for ground robots (right)

    [Header("Wave Settings")]
    public int totalWaves = 3;                      // Total number of waves
    public float[] waveStartDelays;                 // Array for delays before each wave starts
    public float spawnDelay = 0.5f;                 // Delay between each enemy spawn

    [Header("Enemy Count Settings")]
    public int droneCountPerWave = 5;               // Number of drones per wave
    public int groundCountPerWave = 5;              // Total number of ground robots per wave

    // Screen boundaries
    private float screenLeft = -16f;
    private float screenRight = 16f;
    private float screenTop = 12f;
    private float screenBottom = -12f;

    private int waveNumber = 0;                     // Tracks the current wave number

    void Start()
    {
        // Start the wave spawning process with an initial delay
        StartCoroutine(WaveController());
    }

    IEnumerator WaveController()
    {
        while (waveNumber < totalWaves)
        {
            float currentWaveDelay = waveStartDelays[waveNumber];
            Debug.Log("Wave " + (waveNumber + 1) + " starting in " + currentWaveDelay + " seconds!");

            yield return new WaitForSeconds(currentWaveDelay);

            Debug.Log("Wave " + (waveNumber + 1) + " started!");

            // Spawn enemies for this wave
            StartCoroutine(SpawnDrones(droneCountPerWave));
            StartCoroutine(SpawnGroundRobots(groundCountPerWave / 2, true));  // Left robots
            StartCoroutine(SpawnGroundRobots(groundCountPerWave / 2, false)); // Right robots

            waveNumber++;
        }

        Debug.Log("All waves completed!");
    }

    IEnumerator SpawnDrones(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Generate a random x position and spawn drones above the screen
            float randomX = Random.Range(screenLeft, screenRight);
            Vector3 spawnPosition = new Vector3(randomX, screenTop + 2f, 0f);

            Instantiate(droneRobotPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnGroundRobots(int count, bool spawnLeft)
    {
        for (int i = 0; i < count; i++)
        {
            float spawnX = spawnLeft ? screenLeft - 2f : screenRight + 2f;  // Spawn left or right
            float randomY = Random.Range(screenBottom, screenTop);

            Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);

            // Choose the prefab based on spawn direction
            GameObject prefab = spawnLeft ? groundRobotLeftPrefab : groundRobotRightPrefab;

            Instantiate(prefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
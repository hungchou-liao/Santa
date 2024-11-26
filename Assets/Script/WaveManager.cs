using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject droneRobotPrefab;             // Prefab for drones
    public GameObject groundRobotLeftPrefab;        // Prefab for left-entering ground robots
    public GameObject groundRobotRightPrefab;       // Prefab for right-entering ground robots

    public Transform[] leftSpawnPoints;             // Spawn points for left side
    public Transform[] rightSpawnPoints;            // Spawn points for right side

    public float waveInterval = 30f;                // Time interval between waves
    public int baseDroneCount = 2;                  // Base number of drones per wave
    public int baseGroundCount = 2;                 // Base number of ground robots per wave
    public float spawnDelay = 0.5f;                 // Delay between each robot spawn

    private int waveNumber = 0;                     // Tracks the current wave number

    // Screen boundary limits for spawning
    private float screenLeft = -16f;
    private float screenRight = 16f;
    private float screenTop = 12f;

    void Start()
    {
        // Start the wave spawning process
        InvokeRepeating(nameof(SpawnWave), 0f, waveInterval);
    }

    void SpawnWave()
    {
        waveNumber++;
        Debug.Log("Wave " + waveNumber + " starting!");

        // Increase the robot count with each wave
        int droneCount = baseDroneCount + waveNumber;
        int groundCount = baseGroundCount + waveNumber;

        // Spawn drones above the screen limit
        StartCoroutine(SpawnDrones(droneRobotPrefab, droneCount));

        // Spawn ground robots alternately from left and right
        StartCoroutine(SpawnGroundRobots(groundRobotLeftPrefab, leftSpawnPoints, groundCount / 2));
        StartCoroutine(SpawnGroundRobots(groundRobotRightPrefab, rightSpawnPoints, groundCount / 2));
    }

    System.Collections.IEnumerator SpawnDrones(GameObject dronePrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Generate a random x position within the screen limits
            float randomX = Random.Range(screenLeft, screenRight);

            // Spawn the drone above the top screen limit
            Vector3 spawnPosition = new Vector3(randomX, screenTop + 2f, 0f); // Adjust spawn height as needed

            // Instantiate the drone
            Instantiate(dronePrefab, spawnPosition, Quaternion.identity);

            // Wait for a short delay before spawning the next drone
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    System.Collections.IEnumerator SpawnGroundRobots(GameObject robotPrefab, Transform[] spawnPoints, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Pick a random spawn point from the given array
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the robot at the chosen spawn point
            Instantiate(robotPrefab, spawnPoint.position, spawnPoint.rotation);

            // Wait for a short delay before spawning the next robot
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
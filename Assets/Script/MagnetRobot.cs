using System.Collections;
using UnityEngine;

public class MagnetRobot : MonoBehaviour
{
    public float startYPosition = 15f;       // Initial Y position (above the screen)
    public float stopYPosition = -2f;       // Y position where the robot stops
    public float leaveYPosition = 15f;      // Y position where the robot leaves the scene
    public float holdDuration = 20f;        // Time the robot holds Santa
    public float enterDelay = 2f;           // Delay before the robot starts descending
    public Vector2 magnetAreaSize = new Vector2(5f, 5f); // Width and height of the magnet area
    public float snapSpeed = 5f;            // Speed to pull Santa toward the MagnetRobot
    public float enterDuration = 2f;        // Duration for the robot to descend
    public float leaveDelay = 2f;           // Delay after holding Santa before leaving
    public float leaveDuration = 2f;        // Duration for the robot to ascend and leave

    private Transform santa;                // Reference to Santa's transform
    private Rigidbody2D santaRb;            // Reference to Santa's Rigidbody2D
    private FixedJoint2D joint;             // Joint to attach Santa to the MagnetRobot
    private bool isHolding = false;         // Whether the robot is holding Santa
    private bool isSnapping = false;        // Whether Santa is being snapped to the MagnetRobot
    private float holdTimer = 0f;           // Timer for holding Santa
    private Bounds magnetBounds;            // Bounds for the square magnet area

    void Start()
    {
        // Set the initial position above the scene
        transform.position = new Vector3(transform.position.x, startYPosition, transform.position.z);

        // Find Santa in the scene
        GameObject santaObject = GameObject.FindWithTag("Player"); // Ensure Santa is tagged as "Player"
        if (santaObject != null)
        {
            santa = santaObject.transform;
            santaRb = santaObject.GetComponentInParent<Rigidbody2D>();
            if (santaRb == null)
            {
                Debug.LogError("Santa must have a Rigidbody2D component!");
            }
        }

        // Add a FixedJoint2D to the MagnetRobot (disabled by default)
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.enabled = false;

        // Start entering the scene with a delay
        StartCoroutine(DelayedEnterScene());
    }

    void Update()
    {
        // Handle snapping behavior
        if (isSnapping && santa != null)
        {
            SnapSantaToMagnet();
        }

        // Handle holding behavior
        if (isHolding)
        {
            holdTimer += Time.deltaTime;

            // Release Santa and leave after hold duration
            if (holdTimer >= holdDuration)
            {
                ReleaseSanta();
            }
        }
    }

    private IEnumerator DelayedEnterScene()
    {
        Debug.Log("MagnetRobot waiting for enter delay...");
        yield return new WaitForSeconds(enterDelay); // Wait for the specified delay

        // Start descending after the delay
        StartCoroutine(EnterScene());
    }

    private IEnumerator EnterScene()
    {
        Debug.Log("MagnetRobot is entering the scene...");
        float elapsedTime = 0f;
        Vector3 startPosition = new Vector3(transform.position.x, startYPosition, transform.position.z);
        Vector3 endPosition = new Vector3(transform.position.x, stopYPosition, transform.position.z);

        while (elapsedTime < enterDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / enterDuration);
            yield return null;
        }

        transform.position = endPosition; // Ensure exact stop position
        Debug.Log("MagnetRobot has entered the scene.");
        isSnapping = true; // Start the snapping phase
    }

    private void SnapSantaToMagnet()
    {
        if (santa == null) return;

        // Update the magnet bounds based on the MagnetRobot's current position
        UpdateMagnetBounds();

        // Check if Santa is within the square magnet area
        if (IsSantaWithinMagnetArea())
        {
            AttachSanta();
        }
        else
        {
            // Pull Santa toward the center of the magnet area
            Vector2 direction = (Vector2)(transform.position - santa.position).normalized;
            santaRb.velocity = direction * snapSpeed;
        }
    }

    private void AttachSanta()
    {
        Debug.Log("Attaching Santa...");
        joint.connectedBody = santaRb;
        joint.enabled = true;
        isSnapping = false;
        isHolding = true;
        holdTimer = 0f;
    }

    private void ReleaseSanta()
    {
        Debug.Log("Releasing Santa...");

        // Detach Santa
        if (joint.enabled)
        {
            joint.connectedBody = null;
            joint.enabled = false;
        }

        // Reset Santa's velocity
        if (santaRb != null)
        {
            santaRb.velocity = Vector2.zero;
        }

        StartCoroutine(LeaveAfterDelay());
    }

    private IEnumerator LeaveAfterDelay()
    {
        Debug.Log("Waiting for leave delay...");
        yield return new WaitForSeconds(leaveDelay);

        Debug.Log("MagnetRobot is leaving the scene...");
        StartCoroutine(LeaveScene());
    }

    private IEnumerator LeaveScene()
    {
        Debug.Log("MagnetRobot ascending to leave...");
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, leaveYPosition, transform.position.z);

        while (elapsedTime < leaveDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / leaveDuration);
            yield return null;
        }

        transform.position = endPosition; // Ensure exact leave position
        Debug.Log("MagnetRobot has left the scene. Destroying...");
        Destroy(gameObject);
    }

    private void UpdateMagnetBounds()
    {
        // Define the bounds based on the MagnetRobot's position and magnet area size
        magnetBounds = new Bounds(transform.position, new Vector3(magnetAreaSize.x, magnetAreaSize.y, 0f));
    }

    private bool IsSantaWithinMagnetArea()
    {
        // Check if Santa's position is within the bounds
        return magnetBounds.Contains(santa.position);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the magnet range as a square in the editor for visualization
        Gizmos.color = Color.green;
        Vector3 size = new Vector3(magnetAreaSize.x, magnetAreaSize.y, 0f);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
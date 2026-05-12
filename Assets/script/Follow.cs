using UnityEngine;
using UnityEngine.InputSystem; // Required for New Input System

public class Follow : MonoBehaviour
{
    [SerializeField] private float maxDistance = 0.5f; // How far the pupil moves
    [SerializeField] private float moveSpeed = 10f;    // Smoothness of follow

    private Vector3 initialPosition;

    void Start()
    {
        // Store the starting local position (usually center of eye)
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // 1. Get mouse position from the New Input System
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // 2. Convert screen pixels to world coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));
        mouseWorldPos.z = 0f;

        // 3. Calculate direction from the eyeball center to the mouse
        Vector3 direction = mouseWorldPos - transform.parent.position;

        // 4. Clamp the distance so the pupil doesn't leave the eye
        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
        }

        // 5. Apply the position smoothly
        Vector3 targetPosition = transform.parent.position + direction;
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
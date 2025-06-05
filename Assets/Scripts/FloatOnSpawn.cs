// This script lifts the GameObject it is attached to by a specified height over a specified duration when the the object is spawned.
using UnityEngine;

public class LiftOnSpawn : MonoBehaviour
{
    public float liftHeight = 10f;
    public float liftDuration = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float elapsedTime = 0f;

  void Start()
{
    startPosition = transform.position;
    Camera mainCamera = Camera.main;

    // Calculate direction from the object toward the camera
    Vector3 directionToCamera = (mainCamera.transform.position - startPosition).normalized;

    // Optionally add a slight upward movement
    Vector3 diagonalUpToCamera = (directionToCamera + Vector3.up * 0.3f).normalized;

    // Set the target position slightly closer to the camera
    targetPosition = startPosition + diagonalUpToCamera * liftHeight;
}

    void Update()
    {
        if (elapsedTime < liftDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / liftDuration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
    }
}

// This script lifts the GameObject it is attached to by a specified height over a specified duration when the the object is spawned.
using UnityEngine;

public class LiftOnSpawn : MonoBehaviour
{
    public float liftHeight = 7f;
    public float liftDuration = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * liftHeight;
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

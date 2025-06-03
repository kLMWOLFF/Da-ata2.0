using UnityEngine;

public class DistanceReaction : MonoBehaviour
{
    public DistanceToPlayer distanceTracker;
    private Rigidbody rb;

    public bool allowAttraction = true;
    public bool allowRepulsion = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float distance = distanceTracker.distanceToPlayer;

        if (distance > 30f)
        {
            // Stop object motion
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Allow attraction only
            allowRepulsion = false;
            allowAttraction = true;
        }
        else
        {
            // Within range: allow both
            allowRepulsion = true;
            allowAttraction = true;
        }
    }
}


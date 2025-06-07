using UnityEngine;

public class CloudStopPlayer : MonoBehaviour
{
    private DistanceToPlayer distanceTracker;
    private Rigidbody rb;

    public bool allowAttraction = true;
    public bool allowRepulsion = true;

    void Start()
    {
// find script DistanceToPlayer from the same gameobject
        distanceTracker = GetComponent<DistanceToPlayer>();
        

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float distance = distanceTracker.distanceToPlayer;

        if (distance < 5f)
        {
            // Stop object motion
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Allow repulsionn only
            allowRepulsion = true;
            allowAttraction = false;
        }
        else
        {
            // Within range: allow both
            allowRepulsion = true;
            allowAttraction = true;
        }
    }
}


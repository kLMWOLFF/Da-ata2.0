using UnityEngine;

public class DistanceToPlayer : MonoBehaviour
{
    public Transform player;  // Assign in Inspector or via script
    public float distanceToPlayer;  // Public for reference in other scripts or the Inspector

    void Update()
    {
        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
        }
    }
}



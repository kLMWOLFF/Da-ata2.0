using UnityEngine;

public class DistanceToPlayer : MonoBehaviour
{
    public Transform player;  // Assign in Inspector or via script
    public float distanceToPlayer;  // Public for reference in other scripts or the Inspector

    // On start define player variable using the MainCamera tag
    void Start()
    {
        
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            
                player = mainCamera.transform;
                        
            }
    void Update()
    {
        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);
        }
    }
}



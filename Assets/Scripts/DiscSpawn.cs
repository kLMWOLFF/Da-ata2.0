using UnityEngine;

public class DiscSpawn : MonoBehaviour
{
    public GameObject[] discPrefabs;
    public Transform spawnPoint;
    public DistanceToPlayer distanceScript;
    public float minDelay = 5f;
    public float maxDelay = 10f;
    public AudioSource[] soundEffects;

    private GameObject currentDisc;
    private float spawnTimer;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (currentDisc != null) return;

        float distance = distanceScript.distanceToPlayer;

        if (distance > 10f && distance < 35f)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnDisc();
                ResetTimer();
            }
        }
        else
        {
            ResetTimer(); // Pause timer outside range
        }
    }

    void SpawnDisc()
    {
        int index = Random.Range(0, discPrefabs.Length);
        currentDisc = Instantiate(discPrefabs[index], spawnPoint.position, spawnPoint.rotation);
        // give this instance the name of its prefab
        currentDisc.name = discPrefabs[index].name;
        Debug.Log("daata Spawned disc: " + currentDisc.name);

        // Call FloatOnSpawn script
        LiftOnSpawn floatOnSpawn = currentDisc.GetComponent<LiftOnSpawn>();
       
       // play the sound effect
            GetComponent<AudioSource>()?.Play();
            soundEffects[0]?.Play();
        
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minDelay, maxDelay);
    }

    public void NotifyDiscDestroyed(GameObject disc)
    {
        if (disc == currentDisc)
        {
            currentDisc = null;
        }
    }
}

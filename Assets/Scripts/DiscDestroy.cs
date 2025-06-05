using UnityEngine;

public class DiscDestroy : MonoBehaviour
{
    public float lifetime = 40f;
     public float timer = 0f;

    private DiscSpawn spawner;

    void Start()
    {
        timer = lifetime;
        spawner = FindObjectOfType<DiscSpawn>(); // Assumes there's only one spawner in the scene
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            NotifySpawnerAndDestroy();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            NotifySpawnerAndDestroy();
        }
    }

    void NotifySpawnerAndDestroy()
    {
        if (spawner != null)
        {
            Debug.Log("Notifying spawner: disc destroyed");
            spawner.NotifyDiscDestroyed(gameObject); // Let the spawner nullify the reference
        }
        Destroy(gameObject);
    }
}

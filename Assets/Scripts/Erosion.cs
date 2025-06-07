using UnityEngine;

public class UpDirectionCloudTrigger : MonoBehaviour
{
    public float requiredHoldTime = 15f;
    public GameObject cloudPrefab;
    public Transform cloudSpawnPoint; // Optional: assign a position to spawn at

    private float upHoldTimer = 0f;
    private bool cloudSpawned = false;

    void Update()
    {
        if (ShakingMargin.Instance != null && ShakingMargin.Instance.direction == ShakingMargin.Direction.Up)
        {
            upHoldTimer += Time.deltaTime;

            if (upHoldTimer >= requiredHoldTime && !cloudSpawned)
            {
                TriggerCloud();
                cloudSpawned = true;
            }
        }
        else
        {
            upHoldTimer = 0f;
            cloudSpawned = false;
        }
    }

    void TriggerCloud()
    {
        // Deactivate current environment
        if (ArcanaEnvironmentManager.Instance != null)
        {
            var currentEnv = typeof(ArcanaEnvironmentManager)
                .GetField("currentActiveEnvironment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(ArcanaEnvironmentManager.Instance) as GameObject;

            if (currentEnv != null)
            {
                currentEnv.SetActive(false);
            }
        }

        // Spawn cloud
        if (cloudPrefab != null)
        {
            Vector3 spawnPos = cloudSpawnPoint != null ? cloudSpawnPoint.position : Vector3.zero;
            Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Cloud prefab not assigned.");
        }
    }
}

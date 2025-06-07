using UnityEngine;

public class UpDirectionCloudTrigger : MonoBehaviour
{
    public float requiredHoldTime = 15f;
    public GameObject cloudInScene; // This is the DISABLED cloud object in your scene hierarchy

    private float upHoldTimer = 0f;
    private bool cloudActivated = false;

    void Update()
    {
        if (ShakingMargin.Instance != null && ShakingMargin.Instance.direction == ShakingMargin.Direction.Down)
        {
            upHoldTimer += Time.deltaTime;

            if (upHoldTimer >= requiredHoldTime && !cloudActivated)
            {
                TriggerCloud();
                cloudActivated = true;
            }
        }
        else
        {
            upHoldTimer = 0f;
            cloudActivated = false;
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

        // Activate the disabled cloud in the scene
        if (cloudInScene != null)
        {
            cloudInScene.SetActive(true);
        }
        else
        {
            Debug.LogWarning("cloudInScene not assigned in UpDirectionCloudTrigger.");
        }
    }
}

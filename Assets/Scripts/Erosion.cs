using UnityEngine;

public class UpDirectionCloudTrigger : MonoBehaviour
{
    public float requiredHoldTime = 15f;
    public GameObject cloudInScene;               // Drag the disabled cloud object here
    public GameObject targetEnvironment;          // Drag the specific environment this logic should apply to

    private float upHoldTimer = 0f;
    private bool cloudActivated = false;

    void Update()
    {
        // Make sure the right environment is active
        if (ArcanaEnvironmentManager.Instance == null || ArcanaEnvironmentManager.Instance.GetCurrentEnvironment() != targetEnvironment)
        {
            upHoldTimer = 0f;
            cloudActivated = false;
            return;
        }

        // Only count when direction is Up
        if (ShakingMargin.Instance != null && ShakingMargin.Instance.direction == ShakingMargin.Direction.Up)
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
        // Deactivate the current environment
        if (targetEnvironment != null)
        {
            targetEnvironment.SetActive(false);
        }

        // Activate the in-scene cloud
        if (cloudInScene != null)
        {
            cloudInScene.SetActive(true);
        }
        else
        {
            Debug.LogWarning("cloudInScene not assigned.");
        }
    }
}

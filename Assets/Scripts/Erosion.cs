using UnityEngine;

public class UpDirectionCloudTrigger : MonoBehaviour
{
    public GameObject cloudToSpawn;
    public GameObject specificEnvironmentToWatch;

    public float holdDuration = 7f;
    public float blinkStartTime = 4f;
    public float blinkInterval = 0.2f;

    private float upTime = 0f;
    private bool hasTriggered = false;
    private bool isBlinking = false;
    private float nextBlinkTime = 0f;

    private GameObject currentEnvironment;

    void Update()
    {
        if (ArcanaEnvironmentManager.Instance == null) return;

        currentEnvironment = ArcanaEnvironmentManager.Instance.GetCurrentEnvironment();
        if (currentEnvironment != specificEnvironmentToWatch) {
            upTime = 0f;
            ResetBlinking();
            return;
        }

        if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Down)
        {
            upTime += Time.deltaTime;

            // Start blinking at 4 seconds
            if (upTime >= blinkStartTime && upTime < holdDuration)
            {
                if (!isBlinking)
                {
                    isBlinking = true;
                    nextBlinkTime = Time.time + blinkInterval;
                }

                if (Time.time >= nextBlinkTime && currentEnvironment != null)
                {
                    // Toggle visibility
                    bool isActive = currentEnvironment.activeSelf;
                    currentEnvironment.SetActive(!isActive);
                    nextBlinkTime = Time.time + blinkInterval;
                }
            }

            // Final trigger at 7 seconds
            if (upTime >= holdDuration && !hasTriggered)
            {
                if (currentEnvironment != null)
                {
                    currentEnvironment.SetActive(false);
                }

                if (cloudToSpawn != null)
                {
                    cloudToSpawn.SetActive(true);
                }

                hasTriggered = true;
            }
        }
        else
        {
            upTime = 0f;
            ResetBlinking();
        }
    }

    private void ResetBlinking()
    {
        if (isBlinking && currentEnvironment != null && !currentEnvironment.activeSelf)
        {
            currentEnvironment.SetActive(true);
        }

        isBlinking = false;
        hasTriggered = false;
    }
}

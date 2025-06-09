using UnityEngine;

public class UpDirectionCloudTrigger : MonoBehaviour
{
    public GameObject cloudToSpawn;
    public GameObject specificEnvironmentToWatch;

    public float holdDuration = 7f;
    public float blinkStartTime = 4f;
    public float blinkInterval = 0.2f;
    public AudioSource[] soundEffects;

    private float downTime = 0f;
    private bool hasTriggered = false;
    private bool isBlinking = false;
    private float nextBlinkTime = 0f;
    private bool shouldBlink = false;
    private bool environmentPermanentlyDisabled = false;

    private GameObject currentEnvironment;

    void Update()
    {
        if (ArcanaEnvironmentManager.Instance == null) return;

        currentEnvironment = ArcanaEnvironmentManager.Instance.GetCurrentEnvironment();

        if (currentEnvironment != specificEnvironmentToWatch)
        {
            ResetState();
            return;
        }

        if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Down)
        {
            downTime += Time.deltaTime;

            // Start blinking logic
            if (downTime >= blinkStartTime && downTime < holdDuration && !hasTriggered && !environmentPermanentlyDisabled)
            {
                if (!isBlinking)
                {
                    isBlinking = true;
                    nextBlinkTime = Time.time + blinkInterval;
                    shouldBlink = currentEnvironment != null && currentEnvironment.activeSelf;

                    GetComponent<AudioSource>()?.Play();
                    soundEffects[0]?.Play();
                }

                if (shouldBlink && Time.time >= nextBlinkTime && currentEnvironment != null)
                {
                    currentEnvironment.SetActive(!currentEnvironment.activeSelf);
                    nextBlinkTime = Time.time + blinkInterval;
                }
            }

            // Final action after full hold duration
            if (downTime >= holdDuration && !hasTriggered)
            {
                if (currentEnvironment != null)
                {
                    currentEnvironment.SetActive(false); // Fully disable
                    environmentPermanentlyDisabled = true;

                    soundEffects[0]?.Stop();
                }

                if (cloudToSpawn != null && !cloudToSpawn.activeSelf)
                {
                    cloudToSpawn.SetActive(true); // Spawn cloud only if not already active
                }

                hasTriggered = true;
                isBlinking = false;
            }
        }
        else
        {
            ResetState();
        }
    }

    private void ResetState()
    {
        downTime = 0f;

        // Only reactivate environment if blinking was active and it hasn't been permanently disabled
        if (isBlinking && currentEnvironment != null && !environmentPermanentlyDisabled)
        {
            currentEnvironment.SetActive(true);
        }

        isBlinking = false;
        hasTriggered = false;
        shouldBlink = false;
        environmentPermanentlyDisabled = false;
    }
}

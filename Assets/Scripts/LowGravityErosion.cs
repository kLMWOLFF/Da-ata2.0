using UnityEngine;

public class DisappearTimer : MonoBehaviour
{
    public GameObject cloudObject;
    public float checkDistance = 35f;
    public float countdownTime = 15f;
    public AudioSource moveSound;

    private float timer;
    private bool countdownStarted = false;
    private bool hasDisappeared = false;

    private EnvironmentTracker envTracker;
    private BlinkController blink;

    void Start()
    {
        timer = countdownTime;
        envTracker = FindObjectOfType<EnvironmentTracker>();
        blink = GetComponent<BlinkController>();

        if (envTracker == null)
            Debug.LogError("EnvironmentTracker not found.");
    }

    void Update()
    {
        if (cloudObject == null || envTracker == null) return;

        GameObject[] activeEnvs = envTracker.GetAllActiveEnvironments();
        if (activeEnvs.Length == 0)
        {
            ResetTimer();
            return;
        }

        float distance = Vector3.Distance(cloudObject.transform.position, transform.position);
        if (distance > checkDistance && !hasDisappeared)
        {
            if (!countdownStarted)
            {
                countdownStarted = true;
                blink?.StartBlinking();
                if (!moveSound.isPlaying) moveSound.Play();
            }

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                gameObject.SetActive(false);
                hasDisappeared = true;
                blink?.StopBlinking();
                if (moveSound.isPlaying) moveSound.Stop();
            }
        }
        else
        {
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = countdownTime;
        countdownStarted = false;
        hasDisappeared = false;
        blink?.StopBlinking();
        if (moveSound.isPlaying) moveSound.Stop();
    }
}


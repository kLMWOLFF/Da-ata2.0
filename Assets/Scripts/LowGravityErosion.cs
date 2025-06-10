using UnityEngine;

public class LowGravityErosion : MonoBehaviour
{
    public GameObject cloudObject;
    public GameObject[] environmentsToDisable; // 👈 Set these manually
    public float checkDistance = 35f;
    public float countdownTime = 15f;
    public AudioSource moveSound;
    public BlinkEnvironment blinkController; // Drag it in via Inspector!

    private float timer;
    private bool countdownStarted = false;
    private bool hasDisappeared = false;

    private EnvironmentTracker envTracker;
    

    void Start()
    {
        timer = countdownTime;
        envTracker = FindObjectOfType<EnvironmentTracker>();
        
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
        blinkController?.StartBlinking();
        if (!moveSound.isPlaying) moveSound.Play();
    }

    timer -= Time.deltaTime;

    if (timer <= 0f)
    {
        foreach (GameObject env in environmentsToDisable)
        {
            if (env != null) env.SetActive(false);
        }
        hasDisappeared = true;
        blinkController?.StopBlinking();
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
    blinkController?.StopBlinking();
    if (moveSound != null && moveSound.isPlaying) moveSound.Stop();
}
}

using UnityEngine;
using System.Linq;  // For SequenceEqual

public class LowGravityErosion : MonoBehaviour
{
    public GameObject cloudObject;
    public float checkDistance = 35f;
    public float countdownTime = 15f;
    public float blinkInterval = 1f;
    public float liftHeight = 1f;
    public float moveSpeed = 0.1f;
    public AudioSource moveSound;

    private float timer;
    private bool countdownStarted = false;
    private bool hasDeactivated = false;
    private bool isBlinking = false;
    private float nextBlinkTime;

    private EnvironmentTracker envTracker;
    private Vector3[] originalPositions;
    private GameObject[] environments;

    void Start()
    {
        timer = countdownTime;
        envTracker = FindObjectOfType<EnvironmentTracker>();
        if (envTracker == null)
            Debug.LogError("EnvironmentTracker not found in scene.");
    }

    void Update()
    {
        if (cloudObject == null || envTracker == null) return;

        float distance = Vector3.Distance(cloudObject.transform.position, transform.position);
        GameObject[] activeEnvs = envTracker.GetAllActiveEnvironments();

        if (activeEnvs.Length == 0)
        {
            ResetState();
            return;
        }

        if (environments == null || originalPositions == null || 
            originalPositions.Length != activeEnvs.Length || 
            !environments.SequenceEqual(activeEnvs))
        {
            environments = activeEnvs;
            originalPositions = new Vector3[environments.Length];
            for (int i = 0; i < environments.Length; i++)
                originalPositions[i] = environments[i].transform.position;
        }

        if (distance > checkDistance && !hasDeactivated)
            countdownStarted = true;

        if (countdownStarted && distance > checkDistance && !hasDeactivated)
        {
            timer -= Time.deltaTime;
            if (!isBlinking)
            {
                isBlinking = true;
                if (!moveSound.isPlaying) moveSound.Play();
            }
            BlinkAndLift();

            if (timer <= 0f)
            {
                for (int i = 0; i < environments.Length; i++)
                {
                    if (environments[i].activeSelf) environments[i].SetActive(false);
                    environments[i].transform.position = originalPositions[i];
                }
                hasDeactivated = true;
                isBlinking = false;
                if (moveSound.isPlaying) moveSound.Stop();
            }
        }
        else if (distance <= checkDistance)
        {
            ResetState();
            StopBlinkAndLower();
        }
    }

    void ResetState()
    {
        timer = countdownTime;
        countdownStarted = false;
        hasDeactivated = false;
        if (isBlinking)
        {
            isBlinking = false;
            if (moveSound.isPlaying) moveSound.Stop();
        }
    }

    void BlinkAndLift()
    {
        if (environments == null || environments.Length == 0) return;

        foreach (var env in environments)
        {
            if (!env.activeSelf) continue;

            if (Time.time >= nextBlinkTime)
            {
                env.SetActive(false);
                nextBlinkTime = Time.time + blinkInterval / 2f;
            }
            else if (Time.time >= nextBlinkTime - (blinkInterval / 2f))
            {
                env.SetActive(true);
            }

            int idx = System.Array.IndexOf(environments, env);
            if (idx < 0) continue;

            Vector3 targetPos = originalPositions[idx] + Vector3.up * liftHeight;
            env.transform.position = Vector3.MoveTowards(env.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    void StopBlinkAndLower()
    {
        if (environments == null || environments.Length == 0) return;

        GameObject env = environments[0];
        if (!env.activeSelf) return;

        env.transform.position = Vector3.MoveTowards(env.transform.position, originalPositions[0], moveSpeed * Time.deltaTime);
        if (Vector3.Distance(env.transform.position, originalPositions[0]) < 0.01f)
            env.transform.position = originalPositions[0];
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverByDistance : MonoBehaviour
{
    [Header("Assign the Player GameObject")]
    public GameObject player;

    [Header("Assign the Cloud GameObject")]
    public GameObject cloud;

    [Header("Assign the Hidden Game Over Sphere")]
    public GameObject gameOverSphere;

    [Header("Distance & Timing Settings")]
    public float triggerDistance = 7f;
    public float timeToTrigger = 5f;
    public float gracePeriod = 10f;
    public float effectDuration = 1.5f;

    [Header("Scale Settings")]
    public float shrinkMultiplier = 0.5f;
    public float growMultiplier = 1.5f;

    private float currentTime = 0f;
    private bool gameOverTriggered = false;
    private float timeSinceStart = 0f;

    void Update()
    {
        if (gameOverTriggered || player == null || cloud == null)
            return;

        timeSinceStart += Time.deltaTime;

        if (timeSinceStart < gracePeriod)
        {
            return; // Grace period active — no checks yet
        }

        float distance = Vector3.Distance(player.transform.position, cloud.transform.position);

        if (distance <= triggerDistance)
        {
            currentTime += Time.deltaTime;
            Debug.Log($"Cloud close to player for {currentTime:F2}s");

            if (currentTime >= timeToTrigger)
            {
                gameOverTriggered = true;
                StartCoroutine(DeathSequence());
            }
        }
        else
        {
            if (currentTime > 0f)
                Debug.Log("Cloud moved away. Timer reset.");
            currentTime = 0f;
        }
    }

    private IEnumerator DeathSequence()
    {
        Debug.Log("Game Over: Triggered by proximity.");

        if (gameOverSphere != null)
        {
            gameOverSphere.SetActive(true);
            Transform sphereTransform = gameOverSphere.transform;
            Vector3 originalScale = sphereTransform.localScale;

            // Shrink
            float t = 0f;
            while (t < effectDuration / 2f)
            {
                t += Time.deltaTime;
                float scale = Mathf.Lerp(1f, shrinkMultiplier, t / (effectDuration / 2f));
                sphereTransform.localScale = originalScale * scale;
                yield return null;
            }

            // Grow
            t = 0f;
            while (t < effectDuration / 2f)
            {
                t += Time.deltaTime;
                float scale = Mathf.Lerp(shrinkMultiplier, growMultiplier, t / (effectDuration / 2f));
                sphereTransform.localScale = originalScale * scale;
                yield return null;
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

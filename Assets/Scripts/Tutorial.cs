using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject cloud;         // Assign your cloud prefab instance
    public GameObject tutorialMesh;  // Assign your mesh in the hierarchy (keep it disabled)

    public float distance = 2f;
    public float duration = 10f;
    public int oscillations = 2;

    private Vector3 startPosition;
    private float timeElapsed;
    private bool isTutorialRunning = true;

    private GravityToPlayer gravityScript;

    void Start()
    {
        if (cloud == null || tutorialMesh == null)
        {
            Debug.LogError("Assign both the cloud and the tutorial mesh in the inspector.");
            enabled = false;
            return;
        }

        startPosition = cloud.transform.position;

        // Disable gravity attraction
        gravityScript = cloud.GetComponent<GravityToPlayer>();
        if (gravityScript != null)
        {
            gravityScript.enabled = false;
        }

        // Enable the tutorial mesh
        tutorialMesh.SetActive(true);
    }

    void Update()
    {
        if (!isTutorialRunning) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed < duration)
        {
            // Smooth back-and-forth motion using sine wave
            float phase = Mathf.Sin((timeElapsed / duration) * oscillations * 2 * Mathf.PI);
            Vector3 offset = new Vector3(0, 0, phase * (distance / 2)); // Z-axis movement
            cloud.transform.position = startPosition + offset;
        }
        else
        {
            // Reset position
            cloud.transform.position = startPosition;

            // Re-enable gravity
            if (gravityScript != null)
            {
                gravityScript.enabled = true;
            }

            // Disable the tutorial mesh
            tutorialMesh.SetActive(false);

            // End the tutorial
            isTutorialRunning = false;
            enabled = false;
        }
    }
}

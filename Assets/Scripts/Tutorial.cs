using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject cloud;
    public GameObject tutorialMesh;

    public AudioSource pushSound;
    public AudioSource pullSound;

    public float distance = 2f;
    public float duration = 10f;
    public int oscillations = 2;

    private Vector3 startPosition;
    private float timeElapsed;
    private bool isTutorialRunning = true;

    private GravityToPlayer gravityScript;
    private float lastZ = 0f;

    void Start()
    {
        if (cloud == null || tutorialMesh == null)
        {
            Debug.LogError("Assign both cloud and tutorialMesh.");
            enabled = false;
            return;
        }

        startPosition = cloud.transform.position;

        gravityScript = cloud.GetComponent<GravityToPlayer>();
        if (gravityScript != null)
            gravityScript.enabled = false;

        tutorialMesh.SetActive(true);
    }

    void Update()
    {
        if (!isTutorialRunning) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed < duration)
        {
            float phase = Mathf.Sin((timeElapsed / duration) * oscillations * 2 * Mathf.PI);
            float zOffset = phase * (distance / 2);
            cloud.transform.position = startPosition + new Vector3(0, 0, zOffset);

            // Play sound on direction change
            if (zOffset > lastZ && pushSound && !pushSound.isPlaying)
                pushSound.Play();
            else if (zOffset < lastZ && pullSound && !pullSound.isPlaying)
                pullSound.Play();

            lastZ = zOffset;
        }
        else
        {
            cloud.transform.position = startPosition;
            tutorialMesh.SetActive(false);

            if (gravityScript != null)
                gravityScript.enabled = true;

            isTutorialRunning = false;
            enabled = false;
        }
    }
}

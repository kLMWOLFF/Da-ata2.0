using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public GameObject cloud;
    public GameObject hand;
    public GameObject[] handAnimations; // ZaHandoMT etc.

    [Header("Sounds")]
    public AudioSource pushSound;
    public AudioSource pullSound;

    [Header("Cloud Motion")]
    public float distance = 5f;           // Total back-and-forth range
    public float duration = 10f;          // Oscillation duration
    public int oscillations = 2;          // How many complete oscillations

    [Header("Hand Glow")]
    public Color glowColor = Color.cyan;
    public float glowIntensity = 3f;

    private Vector3 startPosition;
    private float timeElapsed;
    private float lastZ;

    private GravityToPlayer gravityScript;
    private Material handMaterial;
    private Color baseEmission;
    private bool tutorialStarted = false;
    private bool tutorialFinished = false;

    void Start()
    {
        if (!cloud || !hand)
        {
            Debug.LogError("Assign both cloud and hand.");
            enabled = false;
            return;
        }

        // Store initial cloud position
        startPosition = cloud.transform.position;

        // Disable cloud movement and hand at start
        cloud.transform.position = startPosition;
        hand.SetActive(false);

        gravityScript = cloud.GetComponent<GravityToPlayer>();
        if (gravityScript) gravityScript.enabled = false;

        // Start after 3 seconds
        Invoke("InitializeTutorial", 3f);
    }

    void InitializeTutorial()
    {
        tutorialStarted = true;
        timeElapsed = 0f;

        // Activate hand and its animations
        hand.SetActive(true);
        foreach (GameObject handAnim in handAnimations)
        {
            if (handAnim != null)
                handAnim.SetActive(true);
        }

        // Set up glow
        Renderer handRenderer = hand.GetComponent<Renderer>();
        if (handRenderer)
        {
            handMaterial = handRenderer.material;
            baseEmission = handMaterial.GetColor("_EmissionColor");
            handMaterial.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        if (!tutorialStarted || tutorialFinished)
            return;

        timeElapsed += Time.deltaTime;

        // Handle hand glow
        if (handMaterial)
        {
            float cycle = Mathf.PingPong(Time.time, 2f) / 2f;
            Color emission = Color.Lerp(baseEmission, glowColor * glowIntensity, cycle);
            handMaterial.SetColor("_EmissionColor", emission);
        }

        // Cloud movement during tutorial (after 3s)
        if (timeElapsed < duration)
        {
            float phase = Mathf.Sin((timeElapsed / duration) * oscillations * 2 * Mathf.PI);
            float zOffset = phase * (distance / 2); // total travel is distance
            cloud.transform.position = startPosition + new Vector3(0, 0, zOffset);

            // Push/pull audio
            if (zOffset > lastZ && pushSound && !pushSound.isPlaying)
                pushSound.Play();
            else if (zOffset < lastZ && pullSound && !pullSound.isPlaying)
                pullSound.Play();

            lastZ = zOffset;
        }
        else if (!tutorialFinished)
        {
            // Tutorial end: stop cloud, disable hand
            cloud.transform.position = startPosition;
            if (gravityScript) gravityScript.enabled = true;

            hand.SetActive(false);
            tutorialFinished = true;
            enabled = false;
        }
    }
}

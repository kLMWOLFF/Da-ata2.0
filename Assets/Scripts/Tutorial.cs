using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public GameObject cloud;
    public GameObject hand;

    [Header("Sounds")]
    public AudioSource pushSound;
    public AudioSource pullSound;

    [Header("Cloud Motion")]
    public float distance = 2f;
    public float duration = 10f;
    public int oscillations = 2;

    [Header("Hand Glow")]
    public Color glowColor = Color.cyan;
    public float glowIntensity = 3f; // Max brightness

    private Vector3 startPosition;
    private float timeElapsed;
    private float lastZ;

    private GravityToPlayer gravityScript;
    private Material handMaterial;
    private Color baseEmission;
    private bool tutorialFinished = false;

    void Start()
    {
        if (!cloud || !hand)
        {
            Debug.LogError("Assign both cloud and hand.");
            enabled = false;
            return;
        }

        startPosition = cloud.transform.position;

        gravityScript = cloud.GetComponent<GravityToPlayer>();
        if (gravityScript) gravityScript.enabled = false;

        hand.SetActive(true);

        Renderer handRenderer = hand.GetComponent<Renderer>();
        if (handRenderer)
        {
            handMaterial = handRenderer.material; // Unique instance
            baseEmission = handMaterial.GetColor("_EmissionColor");
            handMaterial.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (handMaterial)
        {
            // Smooth fade on/off every 2 seconds (1s fade in, 1s fade out)
            float cycle = Mathf.PingPong(Time.time, 2f) / 2f;
            Color emission = Color.Lerp(baseEmission, glowColor * glowIntensity, cycle);
            handMaterial.SetColor("_EmissionColor", emission);
        }

        if (timeElapsed < duration)
        {
            // Move cloud back and forth
            float phase = Mathf.Sin((timeElapsed / duration) * oscillations * 2 * Mathf.PI);
            float zOffset = phase * (distance / 2);
            cloud.transform.position = startPosition + new Vector3(0, 0, zOffset);

            // Push/pull sounds
            if (zOffset > lastZ && pushSound && !pushSound.isPlaying)
                pushSound.Play();
            else if (zOffset < lastZ && pullSound && !pullSound.isPlaying)
                pullSound.Play();

            lastZ = zOffset;
        }
        else if (!tutorialFinished)
        {
            // End tutorial
            cloud.transform.position = startPosition;
            if (gravityScript) gravityScript.enabled = true;

            hand.SetActive(false);
            tutorialFinished = true;
            enabled = false;
        }
    }
}

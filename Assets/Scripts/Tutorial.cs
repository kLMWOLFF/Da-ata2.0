using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public GameObject cloud;
    public GameObject hand;
    public GameObject[] handAnimations; // Assign ZaHandoMT objects here

    [Header("Sounds")]
    public AudioSource pushSound;
    public AudioSource pullSound;

    [Header("Cloud Motion")]
    public float distance = 5f;
    public float duration = 10f;
    public int oscillations = 2;

    [Header("Hand Glow")]
    public Color glowColor = Color.cyan;
    public float glowIntensity = 3f;

    private Vector3 startPosition;
    private float timeElapsed;
    private float lastZ;
    private bool movingForward = true;

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

        startPosition = cloud.transform.position;
        cloud.transform.position = startPosition;
        hand.SetActive(false);

        gravityScript = cloud.GetComponent<GravityToPlayer>();
        if (gravityScript) gravityScript.enabled = false;

        // Mute all audio globally at start
        AudioListener.volume = 0f;

        // After 3 seconds, unmute and start tutorial
        Invoke("InitializeTutorial", 3f);
    }

    void InitializeTutorial()
    {
        // Unmute audio globally
        AudioListener.volume = 1f;

        tutorialStarted = true;
        timeElapsed = 0f;

        hand.SetActive(true);
        foreach (GameObject handAnim in handAnimations)
        {
            if (handAnim != null)
                handAnim.SetActive(true);
        }

        Renderer handRenderer = hand.GetComponent<Renderer>();
        if (handRenderer)
        {
            handMaterial = handRenderer.material;
            baseEmission = handMaterial.GetColor("_EmissionColor");
            handMaterial.EnableKeyword("_EMISSION");
        }

        float initialPhase = Mathf.Sin((0f / duration) * oscillations * 2 * Mathf.PI);
        lastZ = initialPhase * (distance / 2);
    }

    void Update()
    {
        if (tutorialFinished || !tutorialStarted)
            return;

        timeElapsed += Time.deltaTime;

        if (handMaterial)
        {
            float cycle = Mathf.PingPong(Time.time, 2f) / 2f;
            Color emission = Color.Lerp(baseEmission, glowColor * glowIntensity, cycle);
            handMaterial.SetColor("_EmissionColor", emission);
        }

        if (timeElapsed < duration)
        {
            float phase = Mathf.Sin((timeElapsed / duration) * oscillations * 2 * Mathf.PI);
            float zOffset = phase * (distance / 2);
            cloud.transform.position = startPosition + new Vector3(0, 0, zOffset);

            if (zOffset > lastZ && !movingForward)
            {
                movingForward = true;
                if (pushSound) pushSound.Play();
            }
            else if (zOffset < lastZ && movingForward)
            {
                movingForward = false;
                if (pullSound) pullSound.Play();
            }

            lastZ = zOffset;
        }
        else
        {
            cloud.transform.position = startPosition;
            if (gravityScript) gravityScript.enabled = true;

            hand.SetActive(false);
            tutorialFinished = true;
            enabled = false;
        }
    }
}

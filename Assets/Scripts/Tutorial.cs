using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public GameObject cloud1;
    public GameObject cloud2;
    public GameObject cloud3;
    public GameObject hand;
    public GameObject[] handAnimations;

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

    private Vector3[] startPositions = new Vector3[3];
    private float timeElapsed;
    private float lastZ;
    private bool movingForward = true;

    private GravityToPlayer[] gravityScripts = new GravityToPlayer[3];
    private Material handMaterial;
    private Color baseEmission;
    private bool tutorialStarted = false;
    private bool tutorialFinished = false;

    void Start()
    {
        if (!cloud1 || !cloud2 || !cloud3 || !hand)
        {
            Debug.LogError("Assign all three clouds and the hand.");
            enabled = false;
            return;
        }

        startPositions[0] = cloud1.transform.position;
        startPositions[1] = cloud2.transform.position;
        startPositions[2] = cloud3.transform.position;

        cloud1.transform.position = startPositions[0];
        cloud2.transform.position = startPositions[1];
        cloud3.transform.position = startPositions[2];

        hand.SetActive(false);

        gravityScripts[0] = cloud1.GetComponent<GravityToPlayer>();
        gravityScripts[1] = cloud2.GetComponent<GravityToPlayer>();
        gravityScripts[2] = cloud3.GetComponent<GravityToPlayer>();

        foreach (var script in gravityScripts)
        {
            if (script) script.enabled = false;
        }

        AudioListener.volume = 0f;
        Invoke("InitializeTutorial", 3f);
    }

    void InitializeTutorial()
    {
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

            cloud1.transform.position = startPositions[0] + new Vector3(0, 0, zOffset);
            cloud2.transform.position = startPositions[1] + new Vector3(0, 0, zOffset);
            cloud3.transform.position = startPositions[2] + new Vector3(0, 0, zOffset);

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
            cloud1.transform.position = startPositions[0];
            cloud2.transform.position = startPositions[1];
            cloud3.transform.position = startPositions[2];

            foreach (var script in gravityScripts)
            {
                if (script) script.enabled = true;
            }

            hand.SetActive(false);
            tutorialFinished = true;
            enabled = false;
        }
    }
}

using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public GameObject[] cloudFields; // 9 clouds to be assigned in Inspector
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

    private Vector3[,] startPositions = new Vector3[3, 3];
    private GameObject[,] clouds = new GameObject[3, 3];

    private float timeElapsed;
    private float lastZ;
    private bool movingForward = true;

    private GravityToPlayer[,] gravityScripts = new GravityToPlayer[3, 3];
    private Material handMaterial;
    private Color baseEmission;

    private bool tutorialStarted = false;
    private bool tutorialFinished = false;

    void Start()
    {
        if (cloudFields.Length != 9 || !hand)
        {
            Debug.LogError("Assign exactly 9 cloud objects and the hand.");
            enabled = false;
            return;
        }

        // Setup 3x3 grid of clouds
        for (int i = 0; i < 9; i++)
        {
            int x = i % 3;
            int y = i / 3;
            clouds[x, y] = cloudFields[i];
            startPositions[x, y] = cloudFields[i].transform.position;
            gravityScripts[x, y] = cloudFields[i].GetComponent<GravityToPlayer>();
            if (gravityScripts[x, y]) gravityScripts[x, y].enabled = false;
        }

        hand.SetActive(false);

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

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (clouds[x, y])
                    {
                        clouds[x, y].transform.position = startPositions[x, y] + new Vector3(0, 0, zOffset);
                    }
                }
            }

            // Sound logic
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
            // End tutorial
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (clouds[x, y])
                    {
                        clouds[x, y].transform.position = startPositions[x, y];
                        if (gravityScripts[x, y]) gravityScripts[x, y].enabled = true;
                    }
                }
            }

            hand.SetActive(false);
            tutorialFinished = true;
            enabled = false;
        }
    }
}

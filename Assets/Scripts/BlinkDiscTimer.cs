using UnityEngine;

public class BlinkOnTimer : MonoBehaviour
{
    public DiscDestroy discDestroy;
    public float blinkSpeed = 0.5f;
    public AudioSource blinkAudio;      // Sound to play during blink
    public AudioSource normalAudio;     // Ongoing sound to stop during blink

    private Renderer rend;
    private float blinkTimer;
    private bool isBlinking = false;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Find disc with tag
        GameObject discObject = GameObject.FindGameObjectWithTag("Disc");
        if (discObject != null)
        {
            discDestroy = discObject.GetComponent<DiscDestroy>();
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Disc' found.");
        }
    }

    void Update()
    {
        float time = discDestroy.timer;

        if (time <= 10f && time > 0f)
        {
            if (!isBlinking)
            {
                isBlinking = true;
                if (normalAudio && normalAudio.isPlaying) normalAudio.Stop();
                if (blinkAudio && !blinkAudio.isPlaying) blinkAudio.Play();
            }

            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkSpeed)
            {
                rend.enabled = !rend.enabled;
                blinkTimer = 0f;
            }
        }
        else
        {
            if (isBlinking)
            {
                isBlinking = false;
                if (blinkAudio && blinkAudio.isPlaying) blinkAudio.Stop();
                if (normalAudio && !normalAudio.isPlaying) normalAudio.Play();
            }

            rend.enabled = true;
            blinkTimer = 0f;
        }
    }
}

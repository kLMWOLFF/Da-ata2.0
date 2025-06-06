using UnityEngine;

public class BlinkOnTimer : MonoBehaviour
{
    public DiscDestroy discDestroy; // Reference to the script with the timer
    public float blinkSpeed = 0.5f;

    private Renderer rend;
    private float blinkTimer;

    void Start()
    {
        rend = GetComponent<Renderer>();
        // Find gameObject with the tag "Disc"
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
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkSpeed)
            {
                rend.enabled = !rend.enabled;
                blinkTimer = 0f;
            }
        }
        else
        {
            rend.enabled = true;
            blinkTimer = 0f;
        }
    }
}

using UnityEngine;

public class BlinkController : MonoBehaviour
{
    public float blinkInterval = 1f;
    private float nextBlinkTime;

    private Renderer rend;
    private bool isBlinking = false;

    // 👇 Reference to the other script
    public LowGravityErosion controllerReference;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Only blink if the external script allows it
        if (controllerReference != null && controllerReference.isBlinkingAllowed)
        {
            HandleBlinking();
        }
        else
        {
            // Reset visibility and blinking state
            if (isBlinking)
            {
                rend.enabled = true;
                isBlinking = false;
            }
        }
    }

    void HandleBlinking()
    {
        isBlinking = true;

        if (Time.time >= nextBlinkTime)
        {
            rend.enabled = !rend.enabled;
            nextBlinkTime = Time.time + blinkInterval;
        }
    }
}

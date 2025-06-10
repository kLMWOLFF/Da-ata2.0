using UnityEngine;

public class BlinkEnvironment : MonoBehaviour
{
    public float blinkInterval = 1f;
    public GameObject[] objectsToBlink; // 👈 Drag objects here in Inspector

    private float nextBlinkTime;
    private bool isBlinking = false;

    public bool isBlinkingAllowed = false; // 👈 Set to true externally to trigger blinking

    void Update()
    {
        if (isBlinkingAllowed)
        {
            HandleBlinking();
        }
        else
        {
            if (isBlinking)
            {
                SetRenderersVisible(true);
                isBlinking = false;
            }
        }
    }

    void HandleBlinking()
    {
        isBlinking = true;

        if (Time.time >= nextBlinkTime)
        {
            ToggleRenderers();
            nextBlinkTime = Time.time + blinkInterval;
        }
    }

    void ToggleRenderers()
    {
        foreach (var obj in objectsToBlink)
        {
            if (obj != null)
            {
                Renderer rend = obj.GetComponent<Renderer>();
                if (rend != null) rend.enabled = !rend.enabled;
            }
        }
    }

    void SetRenderersVisible(bool visible)
    {
        foreach (var obj in objectsToBlink)
        {
            if (obj != null)
            {
                Renderer rend = obj.GetComponent<Renderer>();
                if (rend != null) rend.enabled = visible;
            }
        }
    }

    public void StartBlinking()
    {
        isBlinkingAllowed = true;
    }

    public void StopBlinking()
    {
        isBlinkingAllowed = false;
    }
}

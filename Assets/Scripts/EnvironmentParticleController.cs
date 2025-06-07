using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EnvironmentParticleController : MonoBehaviour
{
    [Tooltip("Speed of the color fade transition.")]
    public float fadeSpeed = 2f;

    private ParticleSystem ps;
    private ParticleSystem.MainModule main;
    private Color currentColor;
    private Color targetColor;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;

        // Initialize with current color
        currentColor = main.startColor.color;
        targetColor = currentColor;
        main.startColor = currentColor;
    }

    void OnEnable()
    {
        // Refresh main module reference on enable in case of prefab reactivation
        main = ps.main;
        main.startColor = currentColor;
    }

    void Update()
    {
        if (currentColor != targetColor)
        {
            currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * fadeSpeed);
            main.startColor = currentColor;
        }
    }

    // Call this when activating the environment to set the new color
    public void SetTargetColor(Color newColor)
    {
        targetColor = newColor;
    }
}

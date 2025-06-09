using UnityEngine;

public class DiscSpin : MonoBehaviour
{
    [Header("Adjustable Speeds")]
    public float downSpeed = 300f;
    public float upSpeed = -1000f;
    public float neutralSpeed = 100f;

    [Header("Control how fast the spin changes")]
    public float lerpSpeed = 0.1f;

    private float spinSpeed = 0f;
    private float targetSpeed = 0f;

    void Update()
    {
        // Determine the target speed based on gravity direction
        if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Down)
            targetSpeed = downSpeed;
        else if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Up)
            targetSpeed = upSpeed;
        else
            targetSpeed = neutralSpeed;

        // Smoothly approach the target speed
        spinSpeed = Mathf.Lerp(spinSpeed, targetSpeed, Time.deltaTime * lerpSpeed);

        // Apply the rotation
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}

using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public bool shouldGrow = true;
    public float scaleSpeed = 1f;
    public Vector3 targetScale = Vector3.one;

    [HideInInspector]
    public bool isActive = false; // Can be accessed and set from other scripts

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (!isActive) return;

        Vector3 currentScale = transform.localScale;
        Vector3 goal = shouldGrow ? targetScale : initialScale;

        transform.localScale = Vector3.MoveTowards(currentScale, goal, scaleSpeed * Time.deltaTime);

        if (transform.localScale == goal)
            isActive = false; // Stop updating once goal is reached
    }

    public void Activate()
    {
        isActive = true;
    }
}

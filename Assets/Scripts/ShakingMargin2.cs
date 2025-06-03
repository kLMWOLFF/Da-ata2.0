using UnityEngine;

public class ShakingMargin2 : MonoBehaviour
{

    public enum Direction
    {
        Neutral,
        Up,
        Down
    }

    public Direction direction = Direction.Neutral;

    public float margin = 10f;

    public float upAngle = 30f;

    public float downAngle = 150f;

    public static ShakingMargin2 Instance { get; private set; }

    private void Awake2()
    {
        // Ensure only one instance of ShakingMargin2 exists
        if (Instance == null)
        {
            Instance = this;
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start2()
    {

    }

    // Update is called once per frame
    void Update2()
    {
        // Calculate the angle between this object up axis and the world up axis
        float angle = Vector3.Angle(transform.up, Vector3.up);
        Debug.Log("Angle: " + angle);

        if (angle < upAngle - margin)
        {
            direction = Direction.Up;
        }
        else if (angle > downAngle + margin)
        {
            direction = Direction.Down;
        }
        else if (angle > upAngle + margin && angle < downAngle - margin)
        {
            direction = Direction.Neutral;
        }
        // Log the current direction

    }
}

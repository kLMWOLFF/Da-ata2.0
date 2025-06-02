using UnityEngine;
using UnityEngine.XR;

public class ControllerGravityManager : MonoBehaviour
{
    public enum Direction
    {
        Neutral,
        Up,
        Down
    }

    public enum InputType
    {
        XRController,
        HandTracking
    }

    [Header("Input Settings")]
    public InputType currentInputType = InputType.HandTracking;
    public XRNode controllerNode = XRNode.RightHand;

    [Header("Angle Settings")]
    public Direction direction = Direction.Neutral;
    public float margin = 10f;
    public float upAngle = 30f;
    public float downAngle = 150f;

    [Header("References")]
    public GravityToPlayer gravityScript;

    private InputDevice targetDevice;

    void Start()
    {
        if (currentInputType == InputType.XRController)
        {
            targetDevice = InputDevices.GetDeviceAtXRNode(controllerNode);
        }

        if (gravityScript == null)
        {
            gravityScript = FindObjectOfType<GravityToPlayer>();
            if (gravityScript == null)
            {
                Debug.LogWarning("No GravityToPlayer script found in the scene!");
            }
        }
    }

    void Update()
    {
        Vector3 rightDirection = Vector3.right;
        bool inputValid = false;

        // Get input device if not valid
        if (!targetDevice.isValid)
        {
            targetDevice = InputDevices.GetDeviceAtXRNode(controllerNode);
        }

        // Get rotation from the device
        if (targetDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            rightDirection = rotation * Vector3.right;
            inputValid = true;
        }

        if (inputValid)
        {
            // Calculate angle with world right
            float angle = Vector3.Angle(rightDirection, Vector3.right);

            // Update direction based on angle
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

            // Update GravityToPlayer script state
            if (gravityScript != null)
            {
                gravityScript.enabled = (direction == Direction.Up);
            }
        }
    }
}
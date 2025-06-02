using UnityEngine;

public class GravityLinker : MonoBehaviour
{
    ShakingMargin shakingMargin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shakingMargin = GetComponent<ShakingMargin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakingMargin.direction == ShakingMargin.Direction.Up)
        {
            Debug.Log("Gravity is linked to Up");

        }
        else if (shakingMargin.direction == ShakingMargin.Direction.Down)
        {
            Debug.Log("Gravity is linked to Down");
        }
    }
}

using UnityEngine;

public class DiscSpin : MonoBehaviour
{
    // Speed of the disc spin
    public float spinSpeed = 0.1f; // degrees per second
    public float targetSpeed = 0.1f; // target speed for the disc spin
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        // check the gravity direction
       if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Down)
        {
            // if the gravity direction is down, set the target speed to 0.1f
            targetSpeed = 300f;
        }
        else if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Up)
        {
            // if the gravity direction is up, set the target speed to -0.1f
            targetSpeed = -1000f;
        }
        else
        {
            // if the gravity direction is neutral, set the target speed to 0
            targetSpeed = 100f;
        }
       

       
       
       
        // lerp the spin speed towards the target speed
        spinSpeed = Mathf.Lerp(spinSpeed, targetSpeed, Time.deltaTime * 0.1f);
        //spin the disc around its Y-axis
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
   
}

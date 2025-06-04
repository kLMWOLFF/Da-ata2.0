using UnityEngine;
using System.Collections.Generic;

public class GravityToPlayer : MonoBehaviour
{
    private Transform playerCenter;             // The camera or XR Origin
    public float gravityStrength = 5f;         // How strong the attraction is
    public float floatForceStrength = 0.2f;    // How much it floats around
    public float massInfluence = 2f;           // How much mass affects the gravity force

    void Start()
    {
        // find gameobject with tag MainCamera
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
            playerCenter = mainCamera.transform;
        
        
    }

    void FixedUpdate()
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        var grabInteractable = rb.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected) return;

        ApplyFloating(rb);

        if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Down)
        {
            AttractToPlayer(rb);
        }
        else if (ShakingMargin.Instance.direction == ShakingMargin.Direction.Up)
        {
            RepelFromPlayer(rb);
        }
    }

    void ApplyFloating(Rigidbody rb)
    {
        Vector3 randomFloat = new Vector3(
            Mathf.PerlinNoise(Time.time * 0.5f + rb.position.x, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * 0.5f + rb.position.y) - 0.5f,
            Mathf.PerlinNoise(Time.time * 0.3f, Time.time * 0.3f + rb.position.z) - 0.5f
        ) * floatForceStrength;

        rb.AddForce(randomFloat, ForceMode.Acceleration);
    }

    void AttractToPlayer(Rigidbody rb)
    {
        Vector3 direction = (playerCenter.position - rb.position).normalized;
        float distance = Vector3.Distance(playerCenter.position, rb.position);

        float baseForceMagnitude = gravityStrength / Mathf.Max(distance, 0.5f);
        float massMultiplier = Mathf.Pow(rb.mass, massInfluence);
        float forceMagnitude = baseForceMagnitude * massMultiplier;

        rb.AddForce(direction * forceMagnitude, ForceMode.Acceleration);
    }

    void RepelFromPlayer(Rigidbody rb)
    {
        Vector3 direction = (rb.position - playerCenter.position).normalized;
        float distance = Vector3.Distance(playerCenter.position, rb.position);

        float baseForceMagnitude = gravityStrength / Mathf.Max(distance, 0.5f);
        float massMultiplier = Mathf.Pow(rb.mass, massInfluence);
        float forceMagnitude = baseForceMagnitude * massMultiplier;

        rb.AddForce(direction * forceMagnitude, ForceMode.Acceleration);
    }

    void OnLevelWasLoaded()
    {
        Start(); // Refresh the list of affected objects
    }
}
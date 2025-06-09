using UnityEngine;

public class CloudColorChange : MonoBehaviour
{
    private DistanceToPlayer distanceScript;
    public float nearLimit = 15f, farLimit = 35f;
    public Color insideColor = Color.red, outsideColor = Color.white;
    private Color currentColor;

    ParticleSystem ps;
    ParticleSystem.MainModule main;

    void Start()
    {
        distanceScript = GetComponent<DistanceToPlayer>();

        currentColor = outsideColor;
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
    }

    void Update()
    {
        bool inRange = distanceScript.distanceToPlayer > nearLimit && distanceScript.distanceToPlayer < farLimit;
        Color targetColor = inRange ? insideColor : outsideColor;

        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 0.5f);
        main.startColor = new ParticleSystem.MinMaxGradient(currentColor);

        Debug.Log("Distance to player: " + distanceScript.distanceToPlayer);
        Debug.Log("Current Color: " + currentColor);
    }
}

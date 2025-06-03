using UnityEngine;

public class ConditionalSelfActivator : MonoBehaviour
{
    [Header("Set these in Inspector")]
    public GameObject triggeringCD;     // The CD object (Beth1, etc.)
    public GameObject playerObject;     // The player object

    private bool hasTriggered = false;

    void Update()
    {
        if (triggeringCD == null || playerObject == null) return;

        if (IsTouching(triggeringCD, playerObject))
        {
            ArcanaEnvironmentManager.Instance?.ActivateEnvironment(this.gameObject);
        }
    }

    bool IsTouching(GameObject a, GameObject b)
    {
        Collider colA = a.GetComponent<Collider>();
        Collider colB = b.GetComponent<Collider>();

        if (colA == null || colB == null) return false;

        return colA.bounds.Intersects(colB.bounds);
    }
}

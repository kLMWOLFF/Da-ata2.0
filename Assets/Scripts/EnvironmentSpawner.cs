using UnityEngine;

public class ConditionalSelfActivator : MonoBehaviour
{
    [Header("Set these in Inspector")]
    public string cdName;               // e.g. "Beth1", "Gimel2"
    public GameObject triggeringCD;
    public GameObject playerObject;

    void Update()
    {
        if (triggeringCD == null || playerObject == null) return;

        if (IsTouching(triggeringCD, playerObject))
        {
            ArcanaEnvironmentManager.Instance?.TryActivateEnvironment(cdName, this.gameObject);
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

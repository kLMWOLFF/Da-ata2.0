using UnityEngine;

public class ArcanaEnvironmentManager : MonoBehaviour
{
    public static ArcanaEnvironmentManager Instance { get; private set; }

    private GameObject currentActiveEnvironment;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ActivateEnvironment(GameObject newEnvironment)
    {
        if (currentActiveEnvironment != null && currentActiveEnvironment != newEnvironment)
        {
            SetChildrenActive(currentActiveEnvironment, false);
        }

        SetChildrenActive(newEnvironment, true);
        currentActiveEnvironment = newEnvironment;
    }

    private void SetChildrenActive(GameObject parent, bool active)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}

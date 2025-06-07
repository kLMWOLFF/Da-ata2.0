using System.Collections.Generic;
using UnityEngine;

public class ArcanaEnvironmentManager : MonoBehaviour
{
    public static ArcanaEnvironmentManager Instance { get; private set; }

    private GameObject currentActiveEnvironment;
    private string currentActiveCD;

    [System.Serializable]
    public class FusionRule
    {
        public string cdNameA;
        public string cdNameB;
        public GameObject fusionEnvironment;
    }

    [System.Serializable]
    public class EnvironmentColor
    {
        public GameObject environmentPrefab;
        public Color color;
    }

    [Header("Fusion Rules")]
    public List<FusionRule> fusionRules = new List<FusionRule>();

    [Header("Environment Colors")]
    public List<EnvironmentColor> environmentColors = new List<EnvironmentColor>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public bool TryActivateEnvironment(string newCDName, GameObject newEnvironment)
    {
        GameObject fusion = TryGetFusion(currentActiveCD, newCDName);

        if (fusion != null)
        {
            ActivateEnvironment(fusion);
            Debug.Log("Fusion triggered: " + currentActiveCD + " + " + newCDName);
            return true;
        }
        else if (newEnvironment.GetComponent<ConditionalSelfActivator>().cdName == newCDName)
        {
            ActivateEnvironment(newEnvironment);
            currentActiveCD = newCDName;
            return true;
        }
        else
        {
            Debug.LogWarning("No fusion or environment relation " + newCDName + " and " + newEnvironment.name);
            return false;
        }
    }

    void ActivateEnvironment(GameObject env)
    {
        if (currentActiveEnvironment != null && currentActiveEnvironment != env)
        {
            SetChildrenActive(currentActiveEnvironment, false);
        }

        SetChildrenActive(env, true);
        currentActiveEnvironment = env;

        UpdateParticleColor(env);
    }

    void SetChildrenActive(GameObject parent, bool active)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    GameObject TryGetFusion(string a, string b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return null;

        foreach (var rule in fusionRules)
        {
            if ((rule.cdNameA == a && rule.cdNameB == b) || (rule.cdNameA == b && rule.cdNameB == a))
                return rule.fusionEnvironment;
        }
        return null;
    }

    void UpdateParticleColor(GameObject env)
    {
        foreach (var entry in environmentColors)
        {
            if (entry.environmentPrefab == env)
            {
                // Try to find an EnvironmentParticleController on the activated environment or its children
                EnvironmentParticleController controller = env.GetComponentInChildren<EnvironmentParticleController>();
                if (controller != null)
                {
                    controller.SetTargetColor(entry.color);
                }
                else
                {
                    Debug.LogWarning("No EnvironmentParticleController found on " + env.name);
                }
                break;
            }
        }
    }

    // ✅ NEW: Getter for the current active environment
    public GameObject GetCurrentEnvironment()
    {
        return currentActiveEnvironment;
    }
}

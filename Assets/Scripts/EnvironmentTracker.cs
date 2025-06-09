using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTracker : MonoBehaviour
{
    public string environmentTag = "environments";

    public GameObject[] GetAllActiveEnvironments()
    {
        List<GameObject> foundEnvs = new List<GameObject>();

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(environmentTag);
        foreach (GameObject obj in taggedObjects)
        {
            if (obj.activeInHierarchy)
            {
                foundEnvs.Add(obj);
            }
        }

        Debug.Log("Tracked active environments: " + foundEnvs.Count);
        return foundEnvs.ToArray();
    }
}

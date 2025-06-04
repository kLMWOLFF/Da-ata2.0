using UnityEngine;

public class ActivateOnPlayerTouch : MonoBehaviour
{
    public string cdName;
    public GameObject environmentToActivate;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera")) // Make sure your player has the tag "MainCamera"
        {
            ArcanaEnvironmentManager.Instance?.TryActivateEnvironment(cdName, environmentToActivate);
            Destroy(this.gameObject); // Optional: destroy disc after use
        }
    }
}


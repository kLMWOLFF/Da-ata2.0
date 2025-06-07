using UnityEngine;

public class TouchedPlayer : MonoBehaviour
{
    // create a list of all possible environment gameobjects
    public GameObject[] environmentObjects;
    public AudioSource[] soundEffects;

    // Start is called before the first frame update
    void Start()
    {
       // debug all environment objects
        foreach (GameObject environmentObject in environmentObjects)
        {
            Debug.Log("daata Environment Object: " + environmentObject.name);
        } 
    }

    // When triggered
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Disc"))
        {
            Debug.Log("daata Touched a disc: " + other.gameObject.name);
            // get the name of the disc
            string discName = other.gameObject.name;
            // loop through the environmentObjects array
            foreach (GameObject environmentObject in environmentObjects)
            {
                Debug.Log("daata Checking environment object: " + environmentObject.name);
                // check if the environmentObject name matches the disc name

                // tell the environment manager to try to activate this environment
                bool success = ArcanaEnvironmentManager.Instance?.TryActivateEnvironment(discName, environmentObject) ?? false;
                if (success)
                {
                    break; // exit loop if environment is activated
                }
            
            }

            // play the sound effect
            GetComponent<AudioSource>()?.Play();
            soundEffects[0]?.Play();
        }
    }
   
}

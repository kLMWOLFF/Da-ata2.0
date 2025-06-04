using UnityEngine;

public class TouchedPlayer : MonoBehaviour
{
    // create a list of all possible environment gameobjects
    public GameObject[] environmentObjects;
    // When triggered
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Disc"))
        {
            // get the name of the disc
            string discName = other.gameObject.name;
            // loop through the environmentObjects array
            // foreach (GameObject environmentObject in environmentObjects)
            // {
            //     // check if the environmentObject name matches the disc name
            //     if (environmentObject.name == discName)
            //     {
            //         // tell the environment manager to activate this environment
            //         ArcanaEnvironmentManager.Instance?.TryActivateEnvironment(discName, environmentObject);
            //         break; // exit loop after finding a match
            //     }
            // }

            // play the sound effect
            GetComponent<AudioSource>()?.Play();
        }
    }
   
}

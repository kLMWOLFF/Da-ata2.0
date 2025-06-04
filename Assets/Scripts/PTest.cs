using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = Color.green;
    }
}

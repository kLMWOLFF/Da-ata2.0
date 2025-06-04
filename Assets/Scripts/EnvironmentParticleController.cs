using UnityEngine;
using System.Collections;

public class EnvironmentParticleController : MonoBehaviour
{
    ParticleSystem ps;
    Coroutine fade;
    Color currentColor;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        currentColor = ps.main.startColor.color;
    }

    public void SetParticleColor(Color target, float duration = 1f)
    {
        if (fade != null) StopCoroutine(fade);
        fade = StartCoroutine(Fade(target, duration));
    }

    IEnumerator Fade(Color target, float d)
    {
        Color start = currentColor;
        for (float t = 0; t < d; t += Time.deltaTime)
        {
            currentColor = Color.Lerp(start, target, t / d);
            var m = ps.main; m.startColor = currentColor;
            yield return null;
        }
        currentColor = target;
        var mFinal = ps.main; mFinal.startColor = currentColor;
    }
}

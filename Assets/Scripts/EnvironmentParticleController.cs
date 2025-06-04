using UnityEngine;
using System.Collections;

public class EnvironmentParticleController : MonoBehaviour
{
    private ParticleSystem ps;
    private Coroutine colorChangeRoutine;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void SetParticleColor(Color newColor, float duration = 1f)
    {
        if (colorChangeRoutine != null)
            StopCoroutine(colorChangeRoutine);

        colorChangeRoutine = StartCoroutine(FadeToColor(newColor, duration));
    }

    private IEnumerator FadeToColor(Color targetColor, float duration)
    {
        var main = ps.main;
        Color currentColor = main.startColor.color;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            Color lerpedColor = Color.Lerp(currentColor, targetColor, time / duration);

            var tempMain = ps.main;
            tempMain.startColor = lerpedColor;

            yield return null;
        }

        var finalMain = ps.main;
        finalMain.startColor = targetColor;
    }
}

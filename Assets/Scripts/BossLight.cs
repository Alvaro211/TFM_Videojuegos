using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLight : MonoBehaviour
{
    public List<GameObject> redBalls;

    public float fadeOutDuration = 1f;   // Tiempo en bajar a 0
    public float fadeInDuration = 2f;    // Tiempo en volver al original
    
    private List<Light> lights;
    private List<TimeCounterTrigger> timeCounter;
    private List<float> originalIntensities;
    private Coroutine fadeCoroutine;

    void Start()
    {
        lights = new List<Light>();
        timeCounter = new List<TimeCounterTrigger>();

        foreach (GameObject go in redBalls)
        {
            // Añadir todas las luces del GameObject y sus hijos
            lights.AddRange(go.GetComponentsInChildren<Light>());

            // Añadir todos los TimeCounterTrigger del GameObject y sus hijos
            timeCounter.AddRange(go.GetComponentsInChildren<TimeCounterTrigger>());
        }

        // Guardamos las intensidades originales
        originalIntensities = new List<float>(lights.Count);
        foreach (var lt in lights)
            originalIntensities.Add(lt.intensity);
    }

    
    public void TriggerLightFade()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float timer = 0f;
        
        foreach(TimeCounterTrigger time in timeCounter)
        {
            time.startCounting = false;
        }

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeOutDuration;
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].intensity = Mathf.Lerp(originalIntensities[i], 0f, t);
            }
            yield return null;
        }

        timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeInDuration;
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].intensity = Mathf.Lerp(0f, originalIntensities[i], t);
            }
            yield return null;
        }

        for (int i = 0; i < lights.Count; i++)
            lights[i].intensity = originalIntensities[i];

        foreach (TimeCounterTrigger time in timeCounter)
        {
            time.startCounting = true;
        }
    }
}

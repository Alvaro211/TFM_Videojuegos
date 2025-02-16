using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HotSpot : MonoBehaviour
{
    public List<Light[]> lights = new List<Light[]>(); // Array de luces a controlar
    public List<int> numLightPlatform;
    public List<Light> allLight;
    public float fadeDuration = 100f;
    public float TimeLightOn = 10f;

    void Start()
    {
        int currentIndex = 0;

        foreach (int numLights in numLightPlatform)
        {
            // Evita exceder el número de luces disponibles
            int count = Mathf.Min(numLights, allLight.Count - currentIndex);

            // Si no quedan luces suficientes, salimos del bucle
            if (count <= 0) break;

            // Crea el array con las luces correspondientes
            Light[] lightGroup = new Light[count];
            for (int j = 0; j < count; j++)
            {
                lightGroup[j] = allLight[currentIndex + j];
            }

            // Agrega el grupo a la lista
            lights.Add(lightGroup);

            // Avanza el índice
            currentIndex += count;
        }
        // Asegurarse de que todas las luces estén apagadas al inicio
        SetAllLightsIntensity(0);
    }

    public void ActivateLights()
    {
        StopAllCoroutines(); // Detener cualquier desvanecimiento previo
        SetAllLightsIntensity(20); // Encender todas las luces al máximo
        for (int i = 0; i < lights.Count; i++)
        {
            StartCoroutine(FadeOutLights(TimeLightOn - i, lights[i])); // Iniciar la reducción de intensidad
        }
    }

    private IEnumerator FadeOutLights(float wait, Light[] light)
    {
        yield return new WaitForSeconds(wait);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float intensity = Mathf.Lerp(20f, 0f, elapsedTime / fadeDuration);
            SetLightsIntensity(intensity, light);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetLightsIntensity(0, light); // Asegurar que terminen completamente apagadas
    }

    private void SetAllLightsIntensity(float intensity)
    {
        foreach (Light[] lightVector in lights)
        {
            foreach (Light light in lightVector) { 
                if (light != null)
                {
                    light.intensity = intensity;
                }
            }
        }
    }

    private void SetLightsIntensity(float intensity, Light[] light)
    {
        foreach (Light lightHotSpot in light)
        {
                if (lightHotSpot != null)
                {
                    lightHotSpot.intensity = intensity;
                }
        }
    }
}

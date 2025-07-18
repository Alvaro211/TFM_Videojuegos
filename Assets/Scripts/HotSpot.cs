using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class HotSpot : MonoBehaviour
{
    public TextMeshPro control;
    public SpriteRenderer iconX;
    public AudioSource audioSource;
    public List<Light[]> lights = new List<Light[]>(); // Array de luces a controlar
    public List<int> numLightPlatform;
    public List<Light> allLight;
    public float fadeDuration = 100f;
    public float TimeLightOn = 10f;
    public int numHotSpot = 0;
    public bool continueLeft;


    void Start()
    {
    
        audioSource = GetComponent<AudioSource>();

        int currentIndex = 0;

        control.gameObject.SetActive(false);

        foreach (int numLights in numLightPlatform)
        {
            // Evita exceder el n�mero de luces disponibles
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

            // Avanza el �ndice
            currentIndex += count;
        }
        // Asegurarse de que todas las luces est�n apagadas al inicio
        SetAllLightsIntensity(0);
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            if (control.gameObject.activeSelf)
            {
                control.gameObject.SetActive(false);
                iconX.gameObject.SetActive(true);
            }
        }
        else
        {
            if (iconX.gameObject.activeSelf)
            {
                control.gameObject.SetActive(true);
                iconX.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateLights()
    {
        StopAllCoroutines(); // Detener cualquier desvanecimiento previo
        SetAllLightsIntensity(50); // Encender todas las luces al m�ximo
        for (int i = 0; i < lights.Count; i++)
        {
            StartCoroutine(FadeOutLights(TimeLightOn - i, lights[i])); // Iniciar la reducci�n de intensidad
        }
    }

    public void AudioPlay()
    {
        audioSource.Play();
    }

    private IEnumerator FadeOutLights(float wait, Light[] light)
    {
        yield return new WaitForSeconds(wait);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float intensity = Mathf.Lerp(10f, 0f, elapsedTime / fadeDuration);
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
                    //light.range = 7;
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

    public void ShowControl()
    {
        if (GameManager.instance.helpControls)
        {
            if (Gamepad.current != null)
                iconX.gameObject.SetActive(true);
            else
                control.gameObject.SetActive(true);
        }

    }

    public void HideControl()
    {
        control.gameObject.SetActive(false);
        iconX.gameObject.SetActive(false);
    }

    
}

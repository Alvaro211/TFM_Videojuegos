using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public List<AudioClip> audioClips = new List<AudioClip>(); // Array de clips de audio (3 sonidos)
    public bool activated = false;
    public GameObject advise;

    private bool souning = false;


    public IEnumerator PlaySoundsInSequence(AudioSource audioSource)
    {
        if (GameManager.instance.helpControls)
        {
            StartCoroutine(ShowAdvice());
        }

        if (!souning)
        {
            souning = true;
            foreach (AudioClip clip in audioClips)
            {
                audioSource.clip = clip;  // Asigna el clip actual al AudioSource
                audioSource.Play();        // Lo reproduce

                yield return new WaitWhile(() => audioSource.isPlaying); // Espera a que termine
            }
            souning = false;
        }

        
    }

    public IEnumerator ShowAdvice()
    {
        advise.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        advise.gameObject.SetActive(false);
    }
}

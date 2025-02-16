using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public AudioClip[] audioClips; // Array de clips de audio (3 sonidos)
    public bool activated = false;

    private bool souning = false;


    public IEnumerator PlaySoundsInSequence(AudioSource audioSource)
    {
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
}

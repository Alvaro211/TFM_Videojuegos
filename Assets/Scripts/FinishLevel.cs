using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public GameObject door;
    public AudioClip open;
    public AudioClip wrong;
    public AudioSource audioSource;
    public TextMeshPro control;
    public bool doorOpen = false;

    public List<AudioClip> audioClips = new List<AudioClip>(); // Array de clips de audio (3 sonidos)
    public bool activated = false;
    public GameObject advise;

    private bool souning = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RotateOverTime()
    {
        Quaternion startRotation = door.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(door.transform.eulerAngles + new Vector3(0, -90, 0));
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            door.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / 2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.rotation = endRotation; // Asegura la rotación final exacta
    }

    public void SoundDoor(bool correct)
    {
        if (correct)
        {
            audioSource.clip = open;
            audioSource.Play();
        }
        else
        {
            StartCoroutine(PlaySoundsInSequence(audioSource));
        }

        
    }

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

    public IEnumerator ShowAdvice()
    {
        advise.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        advise.gameObject.SetActive(false);
    }

    public void ShowControl()
    {
        if (GameManager.instance.helpControls)
            control.gameObject.SetActive(true);
    }

    public void HideControl()
    {
        control.gameObject.SetActive(false);
    }
}

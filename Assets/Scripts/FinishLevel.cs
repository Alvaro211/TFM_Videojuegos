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
    public bool isDoorOnLeft;
    public bool doorOpen = false;

    public List<AudioClip> audioClips = new List<AudioClip>();
    public bool activated = false;
    public GameObject advise;

    private bool souning = false;
    private List<AudioClip> playerSequence = new List<AudioClip>();
    public Animator dooranim;
    public GameObject doorcollision;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        /*if (isDoorOnLeft)
        {
            Transform[] children = new Transform[door.transform.childCount];

            // Guardamos los hijos antes de mover el objeto
            for (int i = 0; i < door.transform.childCount; i++)
            {
                children[i] = door.transform.GetChild(i);
                children[i].SetParent(null); // Los desvinculamos del padre
            }

            // Mover el objeto padre
            door.transform.position += new Vector3(0, 0, 10);

            // Volver a asignar los hijos
            foreach (Transform child in children)
            {
                child.SetParent(door.transform);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RotateOverTime()
    {
        Quaternion startRotation = door.transform.rotation;
        Quaternion endRotation;
        float elapsedTime = 0f;

        if (isDoorOnLeft)
            endRotation = Quaternion.Euler(door.transform.eulerAngles + new Vector3(0, 90, 0));
        else
            endRotation = Quaternion.Euler(door.transform.eulerAngles + new Vector3(0, -90, 0));

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
            dooranim.SetBool("IsOpened", true);
            doorcollision.SetActive(false);
            // StartCoroutine(RotateOverTime());
        }
        else
        {
            audioSource.clip = wrong;
            audioSource.Play();
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


    public void RegisterSound(AudioClip clip)
    {
        playerSequence.Add(clip);
        CheckSequence();
    }

    void CheckSequence()
    {
        if (playerSequence.Count == audioClips.Count)
        {
            if (IsSequenceCorrect())
                SoundDoor(true);
            else
            {
                playerSequence.Clear();
                SoundDoor(false);
            }
        }
    }

    bool IsSequenceCorrect()
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            if (playerSequence[i] != audioClips[i])
                return false;
        }
        return true;
    }

    public void ClearSequence()
    {
        playerSequence.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public GameObject door;
    public AudioClip open;
    public AudioClip wrong;
    public AudioSource audioSource;
    public TextMeshPro[] control;
    public bool isDoorOnLeft;
    public bool doorOpen = false;

    public List<AudioClip> audioClips = new List<AudioClip>();
    public bool activated = false;
    public GameObject advise;

    public CinemachineAnimation cineMachine;

    private bool souning = false;
    private List<AudioClip> playerSequence = new List<AudioClip>();
    public Animator[] dooranimgreen;
    public GameObject doorcollision;

    private bool isPlayerOn = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameManager.instance.Load();

        bool mustOpen = false;
        if (GameManager.instance.isOpenDoorGreen && this.name == "FinishLevelDoor1" && !GameManager.instance.newGame)
        {
            mustOpen = true;
        }
        else if (GameManager.instance.isOpenDoorGreenYellow && this.name == "FinishLevelDoor2" && !GameManager.instance.newGame)
        {
            mustOpen = true;
        }
        else if (GameManager.instance.isOpenDoorBoss && this.name == "FinishLevelDoorBoss" && !GameManager.instance.newGame)
        {
            mustOpen = true;
        }

        if (mustOpen)
        {
            for(int i = 0; i < dooranimgreen.Length; i++)
                dooranimgreen[i].SetBool("IsOpened", true);

            doorcollision.SetActive(false);
            doorOpen = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOn)
        {
            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                control[0].gameObject.SetActive(true);
                control[1].gameObject.SetActive(false);
                control[2].gameObject.SetActive(false);
            }
            else if (GameManager.instance.idiom == GameManager.Language.Valencian)
            {
                control[0].gameObject.SetActive(false);
                control[1].gameObject.SetActive(true);
                control[2].gameObject.SetActive(false);
            }
            else
            {
                control[0].gameObject.SetActive(false);
                control[1].gameObject.SetActive(false);
                control[2].gameObject.SetActive(true);
            }
        }
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
            dooranimgreen[dooranimgreen.Length-1].SetBool("IsOpened", true);
            doorcollision.SetActive(false);
            // StartCoroutine(RotateOverTime());

            if (this.name == "FinishLevelDoor1")
            {
                GameManager.instance.canMove = false;
                cineMachine.PlayTimelineLevel1();
            }
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
        isPlayerOn = true;

        if (GameManager.instance.helpControls)
        {
            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                control[0].gameObject.SetActive(true);
                control[1].gameObject.SetActive(false);
                control[2].gameObject.SetActive(false);
            }
            else if (GameManager.instance.idiom == GameManager.Language.Valencian)
            {
                control[0].gameObject.SetActive(false);
                control[1].gameObject.SetActive(true);
                control[2].gameObject.SetActive(false);
            }
            else
            {
                control[0].gameObject.SetActive(false);
                control[1].gameObject.SetActive(false);
                control[2].gameObject.SetActive(true);
            }
        }
    }

    public void HideControl()
    {
        isPlayerOn = false;

        control[0].gameObject.SetActive(false);
        control[1].gameObject.SetActive(false);
        control[2].gameObject.SetActive(false);
    }


    public void RegisterSound(AudioClip clip)
    {
        playerSequence.Add(clip);
        CheckSequence();
    }

    void CheckSequence()
    {
        if (!doorOpen)
        {
            if (IsSequenceCorrect())
            {
                if (playerSequence.Count == audioClips.Count)
                {
                    GameManager.instance.vibration.VibrarMando(0.5f, 0.25f);
                    SoundDoor(true);
                    doorOpen = true;

                    if (this.name == "FinishLevelDoor1")
                    {
                        GameManager.instance.isOpenDoorGreen = true;
                    }
                    else if (this.name == "FinishLevelDoor2")
                    {
                        GameManager.instance.isOpenDoorGreenYellow = true;
                    }
                    else if (this.name == "FinishLevelDoor2")
                    {
                        GameManager.instance.isOpenDoorBoss = true;
                    }
                }

                dooranimgreen[playerSequence.Count-1].SetBool("IsOpened", true);
            }
            else
            {

                for (int i = 0; i < playerSequence.Count; i++)
                    dooranimgreen[i].SetBool("IsOpened", false);

                playerSequence.Clear();
                SoundDoor(false);

            }
        }
    }

    bool IsSequenceCorrect()
    {

        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != audioClips[i])
                return false;
        }
        return true;
    }

}

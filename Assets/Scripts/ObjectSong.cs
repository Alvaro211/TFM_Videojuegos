using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectSong : MonoBehaviour
{
    public GameObject objectSong;
    public AudioSource audioSource;
    public TextMeshPro control;

    public GameObject player;

    public Color color;
    public int indexSound;

    private bool isSouning;
    private PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource del mismo GameObject
        isSouning = false;

        movement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        
        if (!isSouning && movement.hearingSound[indexSound])
        {
            StartCoroutine(SoundItemCoorutine());
        }
    }

    private void OnEnable()
    {
        isSouning = false;
    }
    
    public Color TakeItem()
    {
        
        if (objectSong.activeInHierarchy)
        {
            SoundItem();
            Color color = objectSong.gameObject.GetComponent<Renderer>().material.color;
            objectSong.gameObject.SetActive(false);

            return color;
        }

        return Color.white;
    }

    public void SoundItem()
    {
        if (objectSong.gameObject.activeInHierarchy)
        {
            audioSource.Play();
        }
    }

    public IEnumerator SoundItemCoorutine()
    {
        isSouning = true;
        if (objectSong.gameObject.activeInHierarchy)
        {
            audioSource.Play();
        }
        yield return new WaitForSeconds(2f);
        isSouning = false;
    }

   /* public void ShowControl()
    {
        if (GameManager.instance.helpControls && objectSong.activeInHierarchy)
            control.gameObject.SetActive(true);
    }

    public void HideControl()
    {
        control.gameObject.SetActive(false);
    }*/

   /* public void ChangeControlTake()
    {
        if (control.text == "E")
            control.text = "R";
    }

    public void ChangeControlSound()
    {
        if (control.text == "R")
            control.text = "E";
    }*/
}

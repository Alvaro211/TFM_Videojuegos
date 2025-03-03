using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectSong : MonoBehaviour
{
    public GameObject objectSong;
    public AudioSource audioSource;
    public TextMeshPro control;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource del mismo GameObject
    }

    public Color TakeItem()
    {
        SoundItem();
        Color color = objectSong.gameObject.GetComponent<Renderer>().material.color;
        objectSong.gameObject.SetActive(false);

        return color;
    }

    public void SoundItem()
    {
        if (objectSong.gameObject.activeInHierarchy)
        {
            audioSource.Play();
        }
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

    public void ChangeControlTake()
    {
        if (control.text == "E")
            control.text = "R";
    }

    public void ChangeControlSound()
    {
        if (control.text == "R")
            control.text = "E";
    }
}

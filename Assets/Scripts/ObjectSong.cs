using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSong : MonoBehaviour
{
    public GameObject objectSong;
    private AudioSource audioSource;

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
}

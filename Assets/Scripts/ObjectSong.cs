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

    private bool isSouning;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource del mismo GameObject
        isSouning = false;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 20.0f && !isSouning) 
        {
            StartCoroutine(SoundItemCoorutine());
        }
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

    public void ShowControl()
    {
        if (GameManager.instance.helpControls && objectSong.activeInHierarchy)
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

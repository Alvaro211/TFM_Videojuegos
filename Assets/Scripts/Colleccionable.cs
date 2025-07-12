using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class Colleccionable : MonoBehaviour
{
    [Header("Float Settings")]
    public float amplitude = 0.5f;   // Altura del movimiento
    public float frequency = 1f;     // Velocidad del movimiento
    public int indexCollecionable;
    public Book book;

    public SpriteRenderer iconX;
    public GameObject tecla;

    private Vector3 startPos;
    private AudioSource audio;

    private bool visible = false;

    void Start()
    {
        startPos = transform.position;
        audio = GetComponent<AudioSource>();

        GameManager.instance.Load();

        if(this.name == "Coleccionables1" && GameManager.instance.isTakeColeccionable1 && !GameManager.instance.newGame)
        {
            this.gameObject.SetActive(false);

            int index = indexCollecionable * 2 + 1;

            book.bookPages[index] = book.bookPageWrittenEng[index];
            book.bookPages[index + 1] = book.bookPageWrittenEng[index + 1];
        }
        else if(this.name == "Coleccionables2" && GameManager.instance.isTakeColeccionable2 && !GameManager.instance.newGame)
        {
            this.gameObject.SetActive(false);

            int index = indexCollecionable * 2 + 1;

            book.bookPages[index] = book.bookPageWrittenEng[index];
            book.bookPages[index + 1] = book.bookPageWrittenEng[index + 1];
        }
        else if(this.name == "Coleccionables3" && GameManager.instance.isTakeColeccionable3 && !GameManager.instance.newGame)
        {
            this.gameObject.SetActive(false);

            int index = indexCollecionable * 2 + 1;

            book.bookPages[index] = book.bookPageWrittenEng[index];
            book.bookPages[index + 1] = book.bookPageWrittenEng[index + 1];
        }
    }

    void Update()
    {
        // Movimiento vertical tipo onda senoidal
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);

        if (visible)
        {
            if (Gamepad.current != null)
            {
                iconX.gameObject.SetActive(true);
                tecla.SetActive(false);
            }
            else
            {
                tecla.SetActive(true);
                iconX.gameObject.SetActive(false);
            }
        }
    }

    public void PlayAudio()
    {
        audio.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        visible = true;
    }

    private void OnTriggerExit(Collider other)
    {
        visible = false;
        tecla.SetActive(false);
        iconX.gameObject.SetActive(false);
    }
}

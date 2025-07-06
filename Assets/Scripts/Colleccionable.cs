using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Colleccionable : MonoBehaviour
{
    [Header("Float Settings")]
    public float amplitude = 0.5f;   // Altura del movimiento
    public float frequency = 1f;     // Velocidad del movimiento
    public int indexCollecionable;
    public Book book;

    private Vector3 startPos;
    private AudioSource audio;

    void Start()
    {
        startPos = transform.position;
        audio = GetComponent<AudioSource>();

        GameManager.instance.Load();

        if(this.name == "Coleccionables1" && GameManager.instance.isTakeColeccionable1)
        {
            this.gameObject.SetActive(false);

            int index = indexCollecionable * 2 + 1;

            book.bookPages[index] = book.bookPageWritten[index];
            book.bookPages[index + 1] = book.bookPageWritten[index + 1];
        }
        else if(this.name == "Coleccionables2" && GameManager.instance.isTakeColeccionable2)
        {
            this.gameObject.SetActive(false);

            int index = indexCollecionable * 2 + 1;

            book.bookPages[index] = book.bookPageWritten[index];
            book.bookPages[index + 1] = book.bookPageWritten[index + 1];
        }
        else if(this.name == "Coleccionables3" && GameManager.instance.isTakeColeccionable3)
        {
            this.gameObject.SetActive(false);

            int index = indexCollecionable * 2 + 1;

            book.bookPages[index] = book.bookPageWritten[index];
            book.bookPages[index + 1] = book.bookPageWritten[index + 1];
        }
    }

    void Update()
    {
        // Movimiento vertical tipo onda senoidal
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }

    public void PlayAudio()
    {
        audio.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Colleccionable : MonoBehaviour
{
    [Header("Float Settings")]
    public float amplitude = 0.5f;   // Altura del movimiento
    public float frequency = 1f;     // Velocidad del movimiento
    public int indexCollecionable;

    private Vector3 startPos;
    private AudioSource audio;

    void Start()
    {
        startPos = transform.position;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Movimiento vertical tipo onda senoidal
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            audio.Play();
    }
}

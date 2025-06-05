using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vibration : MonoBehaviour
{

    // Aqu� se definen los valores de la vibraci�n
    public float intensidadVibracion = 0.5f; // Intensidad de la vibraci�n (0 a 1)
    public float duracionVibracion = 0.5f;  // Duraci�n de la vibraci�n en segundos
    void Update()
    {
        // Verificar si hay un Gamepad conectado
        if (Gamepad.current != null)
        {
            Debug.Log("Mando detectado: " + Gamepad.current.name);
        }
        else
        {
            Debug.Log("No se detect� un mando.");
        }
        if (Gamepad.current != null)
        {
            // Verifica si se presiona el bot�n A (Xbox) o el bot�n X (PlayStation)
            if (Gamepad.current.buttonSouth.isPressed)
            {
                Debug.Log("Bot�n A presionado");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Solo se activa si el objeto que entra en el trigger tiene un controlador de mando (Gamepad)
        if (other.CompareTag("Player")) // Aseg�rate de que el objeto tiene la etiqueta "Player"
        {
            // Vibration en el mando
            VibrarMando(intensidadVibracion, duracionVibracion);
        }
    }

    void VibrarMando(float intensidad, float duracion)
    {
        // Verifica si hay un Gamepad conectado
        if (Gamepad.current != null)
        {
            // Usando el sistema de vibraci�n del Input System para Gamepad
            Gamepad.current.SetMotorSpeeds(intensidad, intensidad);  // Vibraci�n a la misma intensidad en ambos motores

            // Detener la vibraci�n despu�s de un tiempo
            StartCoroutine(DetenerVibracion(duracion));
        }
        else
        {
            Debug.LogWarning("No se encontr� un Gamepad conectado.");
        }
    }

    private System.Collections.IEnumerator DetenerVibracion(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);  // Detener la vibraci�n
        }
    }
}


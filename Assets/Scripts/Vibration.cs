using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vibration : MonoBehaviour
{

    // Aquí se definen los valores de la vibración
    public float intensidadVibracion = 0.5f; // Intensidad de la vibración (0 a 1)
    public float duracionVibracion = 0.5f;  // Duración de la vibración en segundos
    void Update()
    {
        // Verificar si hay un Gamepad conectado
        if (Gamepad.current != null)
        {
            Debug.Log("Mando detectado: " + Gamepad.current.name);
        }
        else
        {
            Debug.Log("No se detectó un mando.");
        }
        if (Gamepad.current != null)
        {
            // Verifica si se presiona el botón A (Xbox) o el botón X (PlayStation)
            if (Gamepad.current.buttonSouth.isPressed)
            {
                Debug.Log("Botón A presionado");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Solo se activa si el objeto que entra en el trigger tiene un controlador de mando (Gamepad)
        if (other.CompareTag("Player")) // Asegúrate de que el objeto tiene la etiqueta "Player"
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
            // Usando el sistema de vibración del Input System para Gamepad
            Gamepad.current.SetMotorSpeeds(intensidad, intensidad);  // Vibración a la misma intensidad en ambos motores

            // Detener la vibración después de un tiempo
            StartCoroutine(DetenerVibracion(duracion));
        }
        else
        {
            Debug.LogWarning("No se encontró un Gamepad conectado.");
        }
    }

    private System.Collections.IEnumerator DetenerVibracion(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);  // Detener la vibración
        }
    }
}


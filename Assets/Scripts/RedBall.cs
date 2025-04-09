using UnityEngine;

public class RedBall : MonoBehaviour
{
    public float timeUnderLight = 2f;  // Tiempo en segundos que el jugador debe estar bajo la luz roja

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si la bola es golpeada por otro objeto con el tag "Ball"
        if (other.CompareTag("Ball"))
        {
            this.gameObject.SetActive(false);
        }
    }

    
}

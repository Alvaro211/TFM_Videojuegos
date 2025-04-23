using UnityEngine;

public class RedBall : MonoBehaviour
{

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

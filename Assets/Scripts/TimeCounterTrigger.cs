using UnityEngine;
using UnityEngine.Diagnostics;

public class TimeCounterTrigger : MonoBehaviour
{
    public bool startCounting = true;   
    private float timeInTrigger = 0f;

    private PlayerMovement player;

    private void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        player = playerGO.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        Debug.Log("Entro");
=======
        //Debug.Log("Entro");
>>>>>>> Inventario-y-Enemigos

        if (other.CompareTag("Player"))  
        {
            timeInTrigger = 0f; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (startCounting && other.CompareTag("Player"))
        {
<<<<<<< HEAD
            Debug.Log(timeInTrigger);
=======
           // Debug.Log(timeInTrigger);
>>>>>>> Inventario-y-Enemigos
            timeInTrigger += Time.deltaTime;  
            if (timeInTrigger >= 2f) 
            {
                player.Dead();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
<<<<<<< HEAD
        Debug.Log("Salgo");
=======
        //Debug.Log("Salgo");
>>>>>>> Inventario-y-Enemigos

        if (other.CompareTag("Player"))
        {
            timeInTrigger = 0f;
        }
    }
}

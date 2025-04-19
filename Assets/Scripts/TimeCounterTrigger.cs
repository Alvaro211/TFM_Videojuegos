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
        //Debug.Log("Entro");

        if (other.CompareTag("Player"))  
        {
            timeInTrigger = 0f; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (startCounting && other.CompareTag("Player"))
        {
           // Debug.Log(timeInTrigger);
            timeInTrigger += Time.deltaTime;  
            if (timeInTrigger >= 2f) 
            {
                player.Dead();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Salgo");

        if (other.CompareTag("Player"))
        {
            timeInTrigger = 0f;
        }
    }
}

using UnityEngine;
using UnityEngine.Diagnostics;

public class TimeCounterTrigger : MonoBehaviour
{
    public bool startCounting = true;
    public float timeAfterDead = 2f;

    private float timeInTrigger = 0f;

    private PlayerMovement player;

    private void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        player = playerGO.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))  
        {
            timeInTrigger = 0f; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (startCounting && other.CompareTag("Player"))
        {

            timeInTrigger += Time.deltaTime;  
            if (timeInTrigger >= timeAfterDead) 
            {
                player.Dead();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            timeInTrigger = 0f;
        }
    }
}

using UnityEngine;

public class TimeCounterTrigger : MonoBehaviour
{
    public bool startCounting = true;   
    private float timeInTrigger = 0f;    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entro");

        if (other.CompareTag("Player"))  
        {
            timeInTrigger = 0f; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (startCounting && other.CompareTag("Player"))
        {
            Debug.Log(timeInTrigger);
            timeInTrigger += Time.deltaTime;  
            if (timeInTrigger >= 2f) 
            {
                CallFunction();  
                startCounting = false;  
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Salgo");

        if (other.CompareTag("Player"))
        {
            timeInTrigger = 0f;
        }
    }

    // Funci�n que se llama despu�s de 2 segundos
    private void CallFunction()
    {
        Debug.Log("Han pasado 2 segundos. Llamando la funci�n.");
        // Aqu� puedes poner la l�gica que deseas ejecutar despu�s de 2 segundos
    }
}

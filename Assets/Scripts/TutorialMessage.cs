using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    public TextMeshPro message;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="Player")
        {
            message.gameObject.SetActive(true); 
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            message.gameObject.SetActive(false);
        }
    }
}

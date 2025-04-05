using UnityEngine;

// This script is used to handle the ball collection logic.
public class CollectibleBall : MonoBehaviour
{
    // Record the total number of balls collected
    public static int collectedCount = 0;

    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
          
            collectedCount++;
      
            Destroy(gameObject);
        }
    }
}
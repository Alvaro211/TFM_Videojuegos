using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunicionBoss : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if(player != null)
                player.Dead();
        }   
          Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;


public class TimeCounterTrigger : MonoBehaviour
{

    public bool startCounting = true;
    public float timeAfterDead = 2f;

    private float timeInTrigger = 0f;

    private PlayerMovement player;


    public float jiGuang_XianShi_shijian = 0.75f;

    public GameObject JiGuang;

    private void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        player = playerGO.GetComponent<PlayerMovement>();
    }
    Transform dframb;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            timeInTrigger = 0f;
            dframb = other.transform;
        }
    }



    private void OnTriggerStay(Collider other)
    {

        if (startCounting && other.CompareTag("Player"))
        {

            timeInTrigger += Time.deltaTime;
            if (timeInTrigger >= (timeAfterDead - jiGuang_XianShi_shijian))
            {
                JiGuang.SetActive(true);
                JiGuang.transform.LookAt(other.transform.position);

            }
            if (timeInTrigger >= timeAfterDead)
            {
                JiGuang.SetActive(false);

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
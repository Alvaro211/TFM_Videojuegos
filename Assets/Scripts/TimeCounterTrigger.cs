using Assets.Scripts.BeamWeapon;
using System.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;


public class TimeCounterTrigger : MonoBehaviour
{

    public bool startCounting = true;
    public float timeAfterDead = 2f;

    private float timeInTrigger = 0f;

    private PlayerMovement player;

    private float vibration;

    public float jiGuang_XianShi_shijian = 0.75f;

    public GameObject JiGuang;
    public BeamWeapon JiGuangChang;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public float defaultOrthoSize = 7f;
    private float targetOrthoSize = 5f;

    private Coroutine zoomCoroutine;
    private bool isZooming = false;

    public Transform LaserIgnitionPoint;
    Transform HeadPosition;
    private void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        player = playerGO.GetComponent<PlayerMovement>();
        HeadPosition = playerGO.transform.Find("HeadPosition");
    }
    Transform dframb;

    bool ison;
    private void Update()
    {
    
    if (JiGuang.activeSelf)
        {
            JiGuangChang.MaxDistance = (HeadPosition.position - LaserIgnitionPoint.position).magnitude;
            JiGuang.transform.LookAt(HeadPosition.position);
        }
           
   
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            timeInTrigger = 0f;
            dframb = other.transform;

            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.flickerSpeed = 15;
        }
    }



    private void OnTriggerStay(Collider other)
    {
       

        

        if (startCounting && other.CompareTag("Player"))
        {
            
            timeInTrigger += Time.deltaTime;

            if (timeInTrigger < 0.5f)
                vibration = timeInTrigger;
            else
                vibration = 0.5f;

            GameManager.instance.vibration.VibrarMando((0.5f + vibration), 0.5f);

            
            JiGuang.SetActive(true);
            

           if (!isZooming && timeInTrigger >= 0.1f) // Puedes ajustar el umbral
           {
               if (zoomCoroutine != null)
                   StopCoroutine(zoomCoroutine);
           
               zoomCoroutine = StartCoroutine(ChangeOrthoSizeSmooth(targetOrthoSize, timeAfterDead));
           }
           
           if (timeInTrigger >= timeAfterDead)
           {
               JiGuang.SetActive(false);
           
               GameManager.instance.firstDeadEye = true;
               GameManager.instance.UpdateIdiom();
               
               player.Dead();
           }
        }
        else
        {
            JiGuang.SetActive(false);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            timeInTrigger = 0f;

            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.flickerSpeed = 5;

            JiGuang.SetActive(false);

            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            StartCoroutine(ChangeOrthoSizeSmooth(defaultOrthoSize, 1f));
        }
    }

    private IEnumerator ChangeOrthoSizeSmooth(float targetSize, float duration)
    {
        isZooming = true;

        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (startCounting)
            {
                elapsed += Time.deltaTime;
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            }
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize;
        isZooming = false;
    }
}
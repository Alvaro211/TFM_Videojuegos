using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectSong : MonoBehaviour
{
    public GameObject objectSong;
    public AudioSource audioSource;
    public TextMeshPro control;

    public GameObject player;

    public Color color;
    public int indexSound;

    public Light light;
    public float decreaseSpeed = 2f; // Velocidad de disminución
    private float minRange = 5f; // Valor mínimo del rango

    private bool isSouning;
    private PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource del mismo GameObject
        isSouning = false;

        movement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        
        if (!isSouning && movement.hearingSound[indexSound])
        {
            StartCoroutine(SoundItemCoorutine());
        }

        if (light.range > minRange)
        {
            light.range -= decreaseSpeed * Time.deltaTime;
            light.range = Mathf.Max(light.range, minRange); // Clamp
        }

        if(objectSong.activeInHierarchy)
            light.gameObject.SetActive(true);
        else
            light.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        isSouning = false;

        light.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        light.gameObject.SetActive(false);
    }

    public Color TakeItem()
    {
        
        if (objectSong.activeInHierarchy)
        {
            SoundItem();
            Color color = objectSong.gameObject.GetComponent<Renderer>().material.color;
            objectSong.gameObject.SetActive(false);

            return color;
        }

        return Color.white;
    }

    public void SoundItem()
    {
        if (objectSong.gameObject.activeInHierarchy)
        {
            audioSource.Play();
            light.range += 5;
        }
    }

    public IEnumerator SoundItemCoorutine()
    {
        isSouning = true;
        if (objectSong.gameObject.activeInHierarchy)
        {
            audioSource.Play();
            light.range += 5;
        }
        yield return new WaitForSeconds(5f);
        isSouning = false;
    }

    
   /* public void ShowControl()
    {
        if (GameManager.instance.helpControls && objectSong.activeInHierarchy)
            control.gameObject.SetActive(true);
    }

    public void HideControl()
    {
        control.gameObject.SetActive(false);
    }*/

   /* public void ChangeControlTake()
    {
        if (control.text == "E")
            control.text = "R";
    }

    public void ChangeControlSound()
    {
        if (control.text == "R")
            control.text = "E";
    }*/
}

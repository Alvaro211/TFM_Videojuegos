using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class AutoSpikeTrap : MonoBehaviour
{
    public float riseSpeed = 2f;         // Velocidad al subir (escalar)
    public float fallSpeed = 2f;         // Velocidad al bajar (encoger)
    public float stayTime = 1f;          // Tiempo que se mantiene arriba
    public float triggerInterval = 2f;   // Tiempo entre activaciones

    public AudioSource audio;

    private bool isRising = false;
    private bool isFalling = false;
    private float timer = 0f;
    private float triggerTimer = 0f;

    private float targetScaleY = 1f;         // Escala m�xima
    private float initialScaleY = 0f;        // Escala inicial (m�nima)
    
    private bool allReached = false;
    private bool allHidden = false;
    private bool firstUp = true;

    public Light spotlight;          // Asigna en el Inspector
    private float maxIntensity;      // Guardamos la intensidad original
    private float minIntensity = 0f;
    private float lightTransitionSpeed = 800f;

    void Start()
    {
        // Al inicio, escala todos los cilindros a 0 (escondidos)
        foreach (Transform child in transform)
{
            if (child.name.Contains("Cylinder")) // o "Cilindro" si usas espa�ol
            {
                Vector3 scale = child.localScale;
                scale.y = initialScaleY;
                child.localScale = scale;
            }

           
        }

        if (spotlight != null)
        {
            maxIntensity = spotlight.intensity;
        }
    }

    void Update()
    {
        triggerTimer += Time.deltaTime;

        if (triggerTimer >= triggerInterval && !allReached)
        {
            isRising = true;
        }

        if (isRising)
        {
            if (firstUp) { 
                firstUp = false;
                Invoke("PlaySound", 0.2f);
            }
            allReached = false;

            foreach (Transform child in transform)
            {
                if (child.name.Contains("Cylinder"))
                {
                    Vector3 scale = child.localScale;
                    scale.y = Mathf.MoveTowards(scale.y, targetScaleY, riseSpeed * Time.deltaTime/3);
                    child.localScale = scale;

                    if (scale.y >= targetScaleY)
                        allReached = true;
                }

                    Animator anim = child.GetComponent<Animator>();
                    if (anim != null && child.gameObject.activeInHierarchy && !anim.GetBool("isUp"))
                    {
                        anim.SetBool("isUp", true);
                    }
            }


            if (allReached)
            {
                isRising = false;
                triggerTimer = 0f;
                timer = 0f;
            }
        }
        else if (!isFalling && allReached)
        {
            firstUp = true;
            timer += Time.deltaTime;
            if (timer >= stayTime)
            {
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            allHidden = false;

            foreach (Transform child in transform)
            {
                if (child.name.Contains("Cylinder"))
                {
                    Vector3 scale = child.localScale;
                    scale.y = Mathf.MoveTowards(scale.y, initialScaleY, fallSpeed * Time.deltaTime/3);
                    child.localScale = scale;

                    if (scale.y <= initialScaleY)
                        allHidden = true;
                }

                    Animator anim = child.GetComponent<Animator>();
                    if (anim != null && child.gameObject.activeInHierarchy && anim.GetBool("isUp"))
                    {
                        anim.SetBool("isUp", false);
                    }

            }

            if (allHidden)
            {
                isFalling = false;
                allReached = false;
            }
        }

        Invoke("LigthTrasparecen", 0.3f);

        if (isRising)
        {
            SetSpriteColor(UnityEngine.Color.white); // Blanco
        }
        else if (isFalling)
        {
            SetSpriteColor(UnityEngine.Color.grey);
        }
    }

    private void LigthTrasparecen()
    {
        if (spotlight != null)
        {
            float targetIntensity = spotlight.intensity;

            if (isRising)
            {
                targetIntensity = maxIntensity;
            }
            else if (isFalling)
            {
                targetIntensity = minIntensity;
            }

            spotlight.intensity = Mathf.MoveTowards(
                spotlight.intensity,
                targetIntensity,
                lightTransitionSpeed * Time.deltaTime
            );
        }
    }

    private void SetSpriteColor(UnityEngine.Color targetColor)
    {
        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = targetColor;
            }
        }
    }

    private void PlaySound()
    {
        audio.Play();
    }
}

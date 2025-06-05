using UnityEngine;

public class AutoSpikeTrap : MonoBehaviour
{
    public float riseSpeed = 2f;         // Velocidad al subir (escalar)
    public float fallSpeed = 2f;         // Velocidad al bajar (encoger)
    public float stayTime = 1f;          // Tiempo que se mantiene arriba
    public float triggerInterval = 2f;   // Tiempo entre activaciones

    private bool isRising = false;
    private bool isFalling = false;
    private float timer = 0f;
    private float triggerTimer = 0f;

    private float targetScaleY = 1f;         // Escala máxima
    private float initialScaleY = 0f;        // Escala inicial (mínima)

    void Start()
    {
        // Al inicio, escala todos los cilindros a 0 (escondidos)
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Cylinder")) // o "Cilindro" si usas español
            {
                Vector3 scale = child.localScale;
                scale.y = initialScaleY;
                child.localScale = scale;
            }
        }
    }

    void Update()
    {
        triggerTimer += Time.deltaTime;

        if (triggerTimer >= triggerInterval)
        {
            isRising = true;
            triggerTimer = 0f;
        }

        if (isRising)
        {
            bool allReached = true;

            foreach (Transform child in transform)
            {
                if (child.name.Contains("Cylinder"))
                {
                    Vector3 scale = child.localScale;
                    scale.y = Mathf.MoveTowards(scale.y, targetScaleY, riseSpeed * Time.deltaTime);
                    child.localScale = scale;

                    if (scale.y < targetScaleY)
                        allReached = false;
                }

                    Animator anim = child.GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetBool("isUp", true);
                    }

            }


            if (allReached)
            {
                isRising = false;
                timer = 0f;
            }
        }
        else if (!isFalling)
        {
            timer += Time.deltaTime;
            if (timer >= stayTime)
            {
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            bool allHidden = true;

            foreach (Transform child in transform)
            {
                if (child.name.Contains("Cylinder"))
                {
                    Vector3 scale = child.localScale;
                    scale.y = Mathf.MoveTowards(scale.y, initialScaleY, fallSpeed * Time.deltaTime);
                    child.localScale = scale;

                    if (scale.y > initialScaleY)
                        allHidden = false;
                }

                    Animator anim = child.GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetBool("isUp", false);
                    }

            }

            if (allHidden)
            {
                isFalling = false;
            }
        }
    }
}

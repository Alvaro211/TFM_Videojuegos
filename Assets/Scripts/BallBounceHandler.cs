using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallBounceHandler : MonoBehaviour
{
    const int MAXREBOUNCE = 0;

    public int bounceCount = 0;
    private Rigidbody rb;
    private SphereCollider collider;
    public float ascendSpeed = 1f; // Velocidad de ascenso
    public bool isAscending = false; // Para evitar múltiples llamadas
    public float velocityX = 0;
    public float velocityY = 0;
    private bool HasCollision = false;

    private Vector3 velocity;

    private float lastCollisionTime = 0f; // Guarda el tiempo del último choque
    private float collisionCooldown = 0.1f; // Tiempo mínimo entre colisiones

    private List<Light> lightBounce = new List<Light>();

    private AudioSource audio;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!HasCollision)
            velocity = rb.velocity;
    }

    private IEnumerator AscendToHeight()
    {
        isAscending = true; // Evitar múltiples llamadas
        rb.isKinematic = true; // Desactivar la física para controlar el movimiento manualmente
        collider.isTrigger = true;
        float targetY = transform.position.y + 2f;

        while (transform.position.y < targetY)
        {
            transform.position += Vector3.up * ascendSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY, transform.position.z); // Asegurar la altura exacta
        isAscending = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        audio.Play();

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("HotSpot"))
        {

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.MoveToBall(gameObject); // Envía la posición de la bola a cada enemigo
            }

            // Obtener la normal del primer punto de contacto
            Vector3 contactNormal = collision.GetContact(0).normal;

            // Verificar si la colisión vino desde abajo (normal apunta hacia arriba)
            if (Vector3.Dot(contactNormal, Vector3.up) < 0.5f)
            {
                velocity.x = -velocity.x;
                rb.velocity = velocity * 0.7f;

                CreatePointLight(collision.GetContact(0).point);
                return;
            }
            else 
                ControlBounce(collision);
        }else if(collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
        
    }

    public void ControlBounce(Collision collision)
    {
        if (Time.time - lastCollisionTime < collisionCooldown) return;
        lastCollisionTime = Time.time;

        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            bounceCount = 0;
            return;
        }
        bounceCount++;

        if (bounceCount >= MAXREBOUNCE && !isAscending)
        {
            StartCoroutine(AscendToHeight());
            rb.velocity = new Vector3(0, 0, 0); // Detener la bola
            rb.angularVelocity = Vector3.zero; // Detener la rotación
            rb.isKinematic = true; // Hacer que la bola no sea afectada por la física
        }
        else if (bounceCount < 3)
        {
            rb.velocity = new Vector3(velocityX, velocityY, 0) * 15;

        }
        CreatePointLight(collision.GetContact(0).point);
    }


    // Método para generar la Point Light en la colisión
    void CreatePointLight(Vector3 position)
    {
        position.y += 2;
        GameObject lightObject = new GameObject("BounceLight"); // Crear objeto vacío
        Light pointLight = lightObject.AddComponent<Light>(); // Agregar componente Light
        pointLight.type = LightType.Point; // Configurar como luz puntual
        pointLight.range = 10f; // Ajustar alcance de la luz
        pointLight.intensity = 40f; // Ajustar intensidad
        pointLight.color = Color.white; // Color de la luz

        lightObject.transform.position = position; // Colocar en la posición de la colisión

        lightBounce.Add(pointLight);

        // Iniciar la reducción de intensidad después de 5 segundos
        StartCoroutine(FadeOutLight(pointLight, 2f));
    }


    // Corrutina para reducir la intensidad gradualmente
    private IEnumerator FadeOutLight(Light light, float fadeDuration)
    {
        yield return new WaitForSeconds(5f); // Esperar 5 segundos antes de empezar el desvanecimiento
        float elapsedTime = 0f;
        float startIntensity = light.intensity;

        while (elapsedTime < fadeDuration)
        {
            light.intensity = Mathf.Lerp(startIntensity, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light.intensity = 0f; // Asegurar que quede completamente apagada
        lightBounce.RemoveAt(0);
        Destroy(light.gameObject); // Eliminar la luz después de apagarse
    }

    public void TurnOffLight()
    {
        foreach(Light light in lightBounce)
        {
            light.gameObject.SetActive(false);
        }

        lightBounce.Clear();
    }
}

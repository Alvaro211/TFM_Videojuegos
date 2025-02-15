using UnityEngine;
using System.Collections;

public class BallBounceHandler : MonoBehaviour
{
    const int MAXREBOTE = 0;

    private int bounceCount = 0;
    private Rigidbody rb;
    public float ascendSpeed = 1f; // Velocidad de ascenso
    private bool isAscending = false; // Para evitar m�ltiples llamadas
    public float velocityX = 0;
    public float velocityY = 0;

    private float lastCollisionTime = 0f; // Guarda el tiempo del �ltimo choque
    private float collisionCooldown = 0.1f; // Tiempo m�nimo entre colisiones

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator AscendToHeight(float targetY)
    {
        isAscending = true; // Evitar m�ltiples llamadas
        rb.isKinematic = true; // Desactivar la f�sica para controlar el movimiento manualmente

        while (transform.position.y < targetY)
        {
            transform.position += Vector3.up * ascendSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY, transform.position.z); // Asegurar la altura exacta
    }

    void OnCollisionEnter(Collision collision)
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
        Debug.Log(bounceCount);

        if (bounceCount >= MAXREBOTE && !isAscending)
        {
            StartCoroutine(AscendToHeight(2f));
            rb.velocity = Vector3.zero; // Detener la bola
            rb.angularVelocity = Vector3.zero; // Detener la rotaci�n
            rb.isKinematic = true; // Hacer que la bola no sea afectada por la f�sica
        }
        else if (bounceCount < 3) {
            rb.velocity = new Vector3(velocityX, velocityY, 0)  * 15;

        }
        CreatePointLight(collision.GetContact(0).point);
    }

    // M�todo para generar la Point Light en la colisi�n
    void CreatePointLight(Vector3 position)
    {
        position.y += 2;
        GameObject lightObject = new GameObject("BounceLight"); // Crear objeto vac�o
        Light pointLight = lightObject.AddComponent<Light>(); // Agregar componente Light
        pointLight.type = LightType.Point; // Configurar como luz puntual
        pointLight.range = 10f; // Ajustar alcance de la luz
        pointLight.intensity = 40f; // Ajustar intensidad
        pointLight.color = Color.white; // Color de la luz

        lightObject.transform.position = position; // Colocar en la posici�n de la colisi�n

        // Iniciar la reducci�n de intensidad despu�s de 5 segundos
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
        Destroy(light.gameObject); // Eliminar la luz despu�s de apagarse
    }

}

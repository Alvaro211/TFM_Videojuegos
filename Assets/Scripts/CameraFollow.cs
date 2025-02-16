using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Referencia al jugador
    public float smoothSpeed = 5f; // Velocidad de suavizado
    public float yOffset = 10f; // Ajuste en el eje Y
    public float zOffset = -10f; // Ajuste en el eje Z (puedes modificarlo según necesites)

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(
                player.position.x,
                player.position.y + yOffset,
                player.position.z + zOffset // Agrega el offset en Z
            );

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}

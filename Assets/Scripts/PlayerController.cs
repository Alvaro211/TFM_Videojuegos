using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Velocidad del movimiento
    public float gravityScale = 1f; // Gravedad normal del jugador
    public float jumpForce = 5f; // Fuerza de salto
    public PoolBolaLuminosa poolBola;

    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isOnFloor; // Indica si el jugador está tocando el suelo
    private bool isOnHotSpot;
    private HotSpot hotspot;

    public GameObject ballPrefab; // Prefab de la bola
    public float launchForce = 5f; // Fuerza con la que se lanza la bola
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Leer entrada de W, A, S, D (movimiento en X y Z)
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D (movimiento en X)
        moveInput.z = Input.GetAxisRaw("Vertical"); // W/S (movimiento en Z)
        moveInput.Normalize(); // Evita moverse más rápido en diagonal

        // Si está tocando el suelo (Floor), desactivamos la gravedad
        if (isOnFloor)
        {
            rb.useGravity = false; // Desactivar la gravedad mientras esté sobre el suelo
        }
        else
        {
            rb.useGravity = true; // Reactivar la gravedad fuera del suelo
        }

        // Aplicar el movimiento en X y Z
        Vector3 movement = moveInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement); // Mover el jugador
        //rb.AddForce(moveInput * moveSpeed, ForceMode.Acceleration);
       /* float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * moveSpeed;
        controller.Move((move ) * Time.deltaTime);*/

        // Detectar salto con la tecla espacio, solo si está tocando el suelo
        if (Input.GetKeyDown(KeyCode.Space) && (isOnFloor || isOnHotSpot))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E) && isOnHotSpot && hotspot != null)
        {
            hotspot.ActivateLights();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            LaunchBall();
        }
    }

    // Método para aplicar el salto
    void Jump()
    {
        // Aplica una fuerza vertical en el eje Y
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnFloor = false; // Evita saltar nuevamente mientras está en el aire
    }

    // Detectar cuando entra en contacto con el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnFloor = true; // El jugador está tocando el suelo
        }

        if (collision.gameObject.CompareTag("HotSpot"))
        {
            isOnHotSpot = true;
            hotspot = collision.collider.GetComponent<HotSpot>();
        }
    }

    // Detectar cuando sale del suelo
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnFloor = false; // El jugador ya no está tocando el suelo
        }

        if (collision.gameObject.CompareTag("HotSpot"))
        {
            isOnHotSpot = false;
            hotspot = null;
        }
    }

    void LaunchBall()
    {
        if (ballPrefab != null)
        {
            // Obtener la posición del ratón en el mundo
            Vector3 mousePosition = GetMouseWorldPosition();

            // Crear la bola desde el pool
            GameObject newBall = poolBola.GetInactivePrefab();
            if (newBall == null) return; // Si no hay bolas inactivas, salir

            newBall.gameObject.SetActive(true);
            

            // Asegurar que la bola tenga un Rigidbody
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            BallBounceHandler ballBuounce = newBall.GetComponent<BallBounceHandler>();
            if (rb != null && ballBuounce != null)
            {
                rb.isKinematic = false;
                // Calcular dirección hacia el ratón
                Vector3 direction = (mousePosition - transform.position).normalized;
                direction.z = 0;
                ballBuounce.velocityY = direction.y;
                ballBuounce.velocityX = direction.x;

                // Ajustar velocidad en base a la dirección
                rb.velocity = direction * 15;

                if(direction.x > 0) 
                    newBall.transform.position = transform.position + new Vector3(2, 1, 0);
                else

                    newBall.transform.position = transform.position + new Vector3(-2, 1, 0);
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero); // Plano en el eje Z = 0
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance); // Retorna la posición del ratón en el mundo
        }

        return Vector3.zero;
    }
}

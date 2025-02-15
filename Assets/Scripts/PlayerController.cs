using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Velocidad del movimiento
    public float gravityScale = -9.8f; // Gravedad normal del jugador
    public float jumpForce = 5f; // Fuerza de salto
    public PoolBolaLuminosa poolBola;

    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isOnHotSpot;
    private HotSpot hotspot;

    public GameObject ballPrefab; // Prefab de la bola
    public float launchForce = 5f; // Fuerza con la que se lanza la bola

    private Vector3 startPosition;
    private float verticalVelocity;
    private bool isMoving;
    private Vector3 currentVelocity;
    private bool jumpCooldown;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        startPosition = transform.position; // Guarda la posición inicial
    }

    void Update()
    {
        // Leer entrada de W, A, S, D (movimiento en X y Z)
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D (movimiento en X)
        moveInput.z = Input.GetAxisRaw("Vertical"); // W/S (movimiento en Z)
        moveInput.Normalize(); // Evita moverse más rápido en diagonal

        if (moveInput.magnitude > 0.1f )
            isMoving = true;

        else
            isMoving = false;

        // Si está tocando el suelo (Floor), desactivamos la gravedad
        if (!controller.isGrounded)
        {
            //rb.useGravity = false; // Desactivar la gravedad mientras esté sobre el suelo
            
        /*}
        else
        {*/
            //rb.useGravity = true; // Reactivar la gravedad fuera del
            verticalVelocity += gravityScale * Time.deltaTime;
        }

        //Move the player
        if (isMoving)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, moveInput * moveSpeed, Time.deltaTime * 10f);
        }
        else
        {
            //Player inertia
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * 5f);

             
        }

        currentVelocity.y = verticalVelocity;
        controller.Move(currentVelocity * Time.deltaTime);

        // Detectar salto con la tecla espacio, solo si está tocando el suelo
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded && !jumpCooldown)
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

        if(transform.position.y < -1.5f)
        {
            transform.position = startPosition;
        }
    }

    // Método para aplicar el salto
    void Jump()
    {
        jumpCooldown = true;
        verticalVelocity = Mathf.Sqrt(jumpForce * -2f *gravityScale);
        Invoke(nameof(EnableJumpCooldown), 0.1f);
    }

    void EnableJumpCooldown()
    {
        jumpCooldown = false;
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

    // Detectar cuando entra en contacto con el suelo
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.position = startPosition;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            transform.position = startPosition;
        }
    }

    // Detectar cuando sale del suelo
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "HotSpot")
        {
            isOnHotSpot = false;
            hotspot = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HotSpot") {
            isOnHotSpot = true;
            hotspot = other.GetComponent<HotSpot>();
        }


    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del movimiento
    public float gravityScale = 1f; // Gravedad normal del jugador
    public float jumpForce = 5f; // Fuerza de salto
    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isOnFloor; // Indica si el jugador está tocando el suelo
    private bool isOnHotSpot;
    private HotSpot hotspot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        // Detectar salto con la tecla espacio, solo si está tocando el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isOnFloor)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(1) && isOnHotSpot)
        {

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
}

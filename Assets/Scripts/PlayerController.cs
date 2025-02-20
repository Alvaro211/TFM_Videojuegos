using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Velocidad del movimiento
    public float gravityScale = -9.8f; // Gravedad normal del jugador
    public float jumpForce = 5f; // Fuerza de salto
    public AudioSource audioSource;
    public PoolBolaLuminosa poolBall;
    public Image[] circles;

    private CharacterController controller;
    private Vector3 moveInput;
    private bool isOnHotSpot;
    private HotSpot hotspot;

    public float launchForce = 5f; // Fuerza con la que se lanza la bola

    private Vector3 startPosition;
    private float verticalVelocity;
    private bool isMoving;
    private Vector3 currentVelocity;
    private bool jumpCooldown;
    private int currentIndex = 0;

    private bool isNearObjectSong;
    private ObjectSong objectSong;

    private StartLevel startLevel;
    private FinishLevel finishLevel;
    private AudioClip[] sequence;
    private bool isOnFinishLevel = false;
    private bool souning = false;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position; // Guarda la posición inicial
        sequence = new AudioClip[3];
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

        if (Input.GetKeyDown(KeyCode.E) && isOnFinishLevel && finishLevel != null)
        {
            PlaySoundsInSequence(audioSource);
            if (CheckSequence())
            {
                finishLevel.OpenDoor();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && isOnHotSpot && hotspot != null)
        {
            hotspot.ActivateLights();
        }else if(Input.GetKeyDown(KeyCode.E) && isNearObjectSong && objectSong != null)
        {
            objectSong.SoundItem();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            LaunchBall();
        }

        if (Input.GetKeyDown(KeyCode.R) && isNearObjectSong && objectSong != null)
        {
            Color color = objectSong.TakeItem();

            // Si hay un círculo disponible, cambia su color
            if (currentIndex < circles.Length)
            {
                circles[currentIndex].color = color; // Pinta el círculo
                sequence[currentIndex] = objectSong.audioSource.clip;
                currentIndex++; // Pasa al siguiente círculo
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) && audioSource != null && startLevel != null)
        {
            StartCoroutine(startLevel.PlaySoundsInSequence(audioSource));
        }

        if(transform.position.y < -1.5f)
        {
            Dead();
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

    public IEnumerator PlaySoundsInSequence(AudioSource audioSource)
    {
        if (!souning)
        {
            souning = true;
            foreach (AudioClip clip in sequence)
            {
                audioSource.clip = clip;  // Asigna el clip actual al AudioSource
                audioSource.Play();        // Lo reproduce

                yield return new WaitWhile(() => audioSource.isPlaying); // Espera a que termine
            }
            souning = false;
        }
    }

    public bool CheckSequence()
    {
        for (int i = 0; i < sequence.Length; i++)
        {
            if(sequence[i] != startLevel.audioClips[i])
                return false;
        }
        return true;
    }
    void LaunchBall()
    {
        if (poolBall != null)
        {
            // Obtener la posición del ratón en el mundo
            Vector3 mousePosition = GetMouseWorldPosition();

            // Crear la bola desde el pool
            GameObject newBall = poolBall.GetInactivePrefab();
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

    private void Dead()
    {
        transform.position = startPosition;
        foreach (Image image in circles)
        {
            image.color = Color.white;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto con el que colisionó tiene la etiqueta "Bola"
        
    }

    void OnControllerCollideraHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Dead();
        }else if (hit.gameObject.CompareTag("Ball"))
        {
            BallBounceHandler ballScript = hit.gameObject.GetComponent<BallBounceHandler>();

            if (ballScript != null)
            {
                // Llamar a una función dentro del script si es necesario
                ballScript.TurnOffLight();
            }
            hit.gameObject.SetActive(false);
        }
    }

    // Detectar cuando sale del suelo
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HotSpot")
        {
            isOnHotSpot = false;
            hotspot = null;
        }else if (other.tag == "FloorObjectSong")
        {
            isNearObjectSong = false;
            objectSong = null;
        }
        else if (other.gameObject.CompareTag("FinishLevel"))
        {
            isOnFinishLevel = false;
            finishLevel = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "HotSpot") {
            isOnHotSpot = true;
            hotspot = other.GetComponent<HotSpot>();
        }else if(other.tag == "FloorObjectSong")
        {
            isNearObjectSong = true;
            objectSong = other.GetComponent<ObjectSong>();
        }else if(other.tag == "StartLevel")
        {
            startLevel = other.GetComponent<StartLevel>();
            if (!startLevel.activated)
            {
                StartCoroutine(startLevel.PlaySoundsInSequence(audioSource));
                startLevel.activated = true;
            }
        }else if(other.gameObject.CompareTag("Ball"))
        {
            BallBounceHandler ballScript = other.gameObject.GetComponent<BallBounceHandler>();

            if (ballScript != null)
            {
                // Llamar a una función dentro del script si es necesario
                ballScript.TurnOffLight();
            }
            other.gameObject.SetActive(false);

            SphereCollider colliderBall = other.gameObject.GetComponent<SphereCollider>();
            if (colliderBall != null) colliderBall.isTrigger = false;
        }else if (other.gameObject.CompareTag("FinishLevel"))
        {
            isOnFinishLevel = true;
            finishLevel = other.GetComponent<FinishLevel>();
        }
    }
}

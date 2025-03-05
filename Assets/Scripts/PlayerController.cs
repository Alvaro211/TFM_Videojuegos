using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Velocidad del movimiento
    public float gravityScale = -9.8f; // Gravedad normal del jugador
    public float jumpForce = 5f; // Fuerza de salto
    public AudioSource audioSourceSequence;
    public AudioSource audioSourceEffectPlayer;
    public PoolBolaLuminosa poolBall;
    public GameObject menuPause;
    public RawImage[] notes;
    public RawImage[] imagesBall;
    public AudioClip aduioJump;

    public List<Enemy> listEnemy = new List<Enemy>();
    public AudioSource audioSourceMusic;
    public List<GameObject> listObjectSong = new List<GameObject>();

    private CharacterController controller;
    private Vector3 moveInput;
    private bool isOnHotSpot;
    private HotSpot hotspot;

    private float launchForce = 5f; // Fuerza con la que se lanza la bola

    private Vector3 startPosition;
    private float verticalVelocity;
    private bool isMoving;
    private Vector3 currentVelocity;
    private bool jumpCooldown;
    private int currentIndex = 0;
    private int indexBallImage = 0;

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
        audioSourceEffectPlayer = GetComponent<AudioSource>();
        startPosition = transform.position; // Guarda la posición inicial
        sequence = new AudioClip[3];

        foreach(RawImage image in notes)
        {
            image.color = Color.white;      
        }
    }

    void Update()
    {
        // Leer entrada de W, A, S, D (movimiento en X y Z)
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D (movimiento en X)
        //moveInput.z = Input.GetAxisRaw("Vertical"); // W/S (movimiento en Z)
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

        if (Input.GetKeyDown(KeyCode.E) && isOnFinishLevel && finishLevel != null && !finishLevel.doorOpen)
        { 

            PlaySoundsInSequence(audioSourceSequence);
            bool correct = CheckSequence();
            if (correct)
            {
                StartCoroutine(finishLevel.RotateOverTime());
                finishLevel.HideControl();
                finishLevel.doorOpen = true;
            }
            finishLevel.SoundDoor(correct);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isOnHotSpot && hotspot != null)
        {
            hotspot.ActivateLights();
        }else if(Input.GetKeyDown(KeyCode.E) && isNearObjectSong && objectSong != null)
        {
            objectSong.SoundItem();
            objectSong.ChangeControlTake();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPause.activeSelf)
                menuPause.gameObject.SetActive(false);
            else
                menuPause.gameObject.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            LaunchBall();
            imagesBall[indexBallImage].gameObject.SetActive(false);
            indexBallImage--;
        }

        if (Input.GetKeyDown(KeyCode.R) && isNearObjectSong && objectSong != null)
        {
            Color color = objectSong.TakeItem();

            // Si hay un círculo disponible, cambia su color
            if (currentIndex < notes.Length)
            {
                notes[currentIndex].color = color; // Pinta el círculo
                sequence[currentIndex] = objectSong.audioSource.clip;
                currentIndex++; // Pasa al siguiente círculo
            }

            objectSong.ChangeControlSound();
            objectSong.HideControl();
        }

        if(Input.GetKeyDown(KeyCode.Q) && audioSourceSequence != null && startLevel != null)
        {
            StartCoroutine(startLevel.PlaySoundsInSequence(audioSourceSequence));
        }

        if(transform.position.y < -1.5f)
        {
            Dead();
        }

        if (CheckEnemyAround()) {

            //audioSourceMusic.pitch = Mathf.Lerp(1.0f, 0.7f, Time.deltaTime * 2);
            audioSourceMusic.pitch = 0.5f;
        }
        else if(audioSourceMusic.pitch != 1)
        {
            audioSourceMusic.pitch = 1;
        }
    }

    // Método para aplicar el salto
    void Jump()
    {
        audioSourceEffectPlayer.clip = aduioJump;
        audioSourceEffectPlayer.Play();
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
            if (sequence[i] == null || !sequence[i].name.Equals(startLevel.audioClips[i].name))
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
        
        controller.enabled = false;
        transform.position = startPosition;
        controller.enabled = true;
        foreach (RawImage image in notes)
        {
            image.color = Color.white;
        }

        foreach(GameObject obj in listObjectSong){
            obj.gameObject.SetActive(true);
        }

    }

    private bool CheckEnemyAround()
    {
        foreach (Enemy enemy in listEnemy)
        {
            float distancia = Vector3.Distance(transform.position, enemy.transform.position);
            if (distancia < 10)
            {
                return true;
            }
        }

        return false;
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
        if (other.tag == "HotSpot" && hotspot != null)
        {
            isOnHotSpot = false;
            hotspot.HideControl();
            hotspot = null;
        }else if (other.tag == "FloorObjectSong" && objectSong != null)
        {
            isNearObjectSong = false;
            objectSong.HideControl();
            objectSong = null;
        }
        else if (other.gameObject.CompareTag("FinishLevel") && finishLevel != null)
        {
            isOnFinishLevel = false;
            finishLevel.HideControl();
            finishLevel = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "HotSpot") {
            isOnHotSpot = true;
            hotspot = other.GetComponent<HotSpot>();
            hotspot.ShowControl();
        }
        else if(other.tag == "FloorObjectSong")
        {
            isNearObjectSong = true;
            objectSong = other.GetComponent<ObjectSong>();
            objectSong.ShowControl();
        }else if(other.tag == "StartLevel")
        {
            startLevel = other.GetComponent<StartLevel>();
            if (!startLevel.activated)
            {
                StartCoroutine(startLevel.PlaySoundsInSequence(audioSourceSequence));
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

            indexBallImage++;
            imagesBall[indexBallImage].gameObject.SetActive(true);
            if (colliderBall != null) colliderBall.isTrigger = false;
        }else if (other.gameObject.CompareTag("FinishLevel"))
        {
            isOnFinishLevel = true;
            finishLevel = other.GetComponent<FinishLevel>();
            finishLevel.ShowControl();
        }else if (other.gameObject.CompareTag("Enemy"))
        {
            Dead();
        }else if (other.gameObject.CompareTag("Reset"))
        {
            SceneManager.LoadScene(0);
        }
    }
}

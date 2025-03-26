using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

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
    private List<AudioClip> sequence;
    private bool isOnFinishLevel = false;
    private bool souning = false;


    private PlayerMap inputMap;

    private Vector2 inputValues;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSourceEffectPlayer = GetComponent<AudioSource>();
        startPosition = transform.position; // Guarda la posición inicial
        sequence = new List<AudioClip>();

        foreach (RawImage image in notes)
        {
            image.color = Color.white;      
        }

        inputMap = new PlayerMap();
        inputMap.Enable();

        inputMap.Player.Movement.performed += DirKeysPerformed;
        inputMap.Player.Movement.canceled += DirKeysPerformed;
        inputMap.Player.Interact.performed += InterectPerformed;
        inputMap.Player.Sequence.performed += SequencePerformed;
        inputMap.Player.Sphere.performed += SpherePerformed;
        inputMap.Player.Jump.performed += JumpPerformed;
        inputMap.Player.TakeSound.performed += TakeSoundPerformed;
        inputMap.Player.Options.performed += OptionsPerformed;
    }

    void Update()
    {
        moveInput.x = inputValues.x;
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

    public void DirKeysPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        inputValues = obj.ReadValue<Vector2>();
    }

    public void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Jump();
    }

    public void OptionsPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (menuPause.activeSelf)
            menuPause.gameObject.SetActive(false);
        else
            menuPause.gameObject.SetActive(true);
    }

    public void InterectPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isOnFinishLevel && finishLevel != null && !finishLevel.doorOpen)
        {

            //PlaySoundsInSequence(audioSourceSequence);
            bool correct = CheckSequence();
            if (correct)
            {
                StartCoroutine(finishLevel.RotateOverTime());
                finishLevel.HideControl();
                finishLevel.doorOpen = true;
            }
            finishLevel.SoundDoor(correct);
        }
        else if ( isOnHotSpot && hotspot != null)
        {
            hotspot.ActivateLights();
        }
    }

    public void SequencePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (audioSourceSequence != null && finishLevel != null)
        {
            StartCoroutine(finishLevel.PlaySoundsInSequence(audioSourceSequence));
        }
    }

    public void SpherePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (indexBallImage < imagesBall.Length)
        {
            LaunchBall();
            imagesBall[indexBallImage].gameObject.SetActive(false);
            indexBallImage++;
        }
    }

    public void TakeSoundPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isNearObjectSong && objectSong != null)
        {
            Color color = objectSong.TakeItem();

            // Si hay un círculo disponible, cambia su color
            if (currentIndex < notes.Length)
            {
                notes[currentIndex].color = color; // Pinta el círculo
                sequence.Add(objectSong.audioSource.clip);
                currentIndex++; // Pasa al siguiente círculo
            }

            objectSong.ChangeControlSound();
            objectSong.HideControl();
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
        if (sequence.Count == 0) return false;

        /*for (int i = 0; i < sequence.Count; i++)
        {
            for (int j = 0; j < finishLevel.audioClips.Count; j++)
            {
                if (sequence[i] == null)
                    return false;
                if (sequence[i].name.Equals(finishLevel.audioClips[j].name))
                    break;
            }
        }*/
        return finishLevel.audioClips.All(item => sequence.Any(audio => audio.name == item.name)); ;
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
                ballBuounce.bounceCount = 0;
                ballBuounce.isAscending = false;

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
      /*  foreach (RawImage image in notes)
        {
            image.color = Color.white;
        }

        foreach(GameObject obj in listObjectSong){
            obj.gameObject.SetActive(true);
        }*/

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
        else if (other.gameObject.CompareTag("FinishLevel") /*&& finishLevel != null*/)
        {
            isOnFinishLevel = false;
            finishLevel.HideControl();
            //finishLevel = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "HotSpot") {
            isOnHotSpot = true;
            hotspot = other.GetComponent<HotSpot>();
            hotspot.ShowControl();
            startPosition = transform.position;
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

            indexBallImage--;
            imagesBall[indexBallImage].gameObject.SetActive(true);
            if (colliderBall != null) colliderBall.isTrigger = false;
        }else if (other.gameObject.CompareTag("FinishLevel"))
        {
            isOnFinishLevel = true;
            finishLevel = other.GetComponent<FinishLevel>();
            finishLevel.ShowControl();
            if (GameManager.instance.helpControls)
            {
                StartCoroutine(finishLevel.ShowAdvice());
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Dead();
        }else if (other.gameObject.CompareTag("Reset"))
        {
            SceneManager.LoadScene(0);
        }
    }
}

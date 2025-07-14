using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public AcousticWave footSoundWaves;
    public SpriteRenderer footSoundWavesSprite;
    public float moveSpeed; // Velocidad del movimiento
    public float gravityScale = -9.8f; // Gravedad normal del jugador
    public float jumpForce = 5f; // Fuerza de salto
    public AudioSource audioSourceSequence;
    public AudioSource audioSourceEffectPlayer;
    public PoolBolaLuminosa poolBall;
    public GameObject menuPause;
    public float cooldownBall;
    public RawImage[] notes;
    public UnityEngine.UI.Slider sliderBall;
    public RawImage imageDamage;
    public AudioClip audioJump;
    public AudioClip audioThrowBall;
    public AudioClip audioDead;
    public AudioClip audioTakeNote;
    public AudioClip audioOpenDiary;
    public GameObject diary;

    public List<Enemy> listEnemy = new List<Enemy>();
    public List<GameObject> listSong = new List<GameObject>();
    public AudioSource audioSourceMusic;

    public GameObject imagePrefab;  // Arrastra aquí el prefab en el Inspector
    public Transform canvasTransform; 
    public float yOffset = 20f;  // Distancia desde la parte baja del Canvas
    public float spacing = 100f; // Espacio entre imágenes

    public SpriteRenderer sprite;
    public GameObject layout;

    private CharacterController controller;
    private Vector3 moveInput;
    private bool isOnHotSpot;
    private HotSpot hotspot;

    private bool isPressJumping;

    private bool updateSliderBall = false;
    private float timerSliderBall = 0f;

    public int currentIndexHotSpot = 0;
    private Vector3 startPosition;
    private bool startToLeft = false;
    private float verticalVelocity;
    private bool isMoving;
    private bool isHit = false;
    private Vector3 currentVelocity;
    private bool jumpCooldown;
    private bool ballLauch;
    private bool onBoss = false;
    private BossLight bossLight;

    private bool isNearObjectSong;
    private ObjectSong objectSong;

    private StartLevel startLevel;
    private FinishLevel finishLevel;
    private List<AudioClip> sequence;
    private bool isOnFinishLevel = false;
    private bool souning = false;

    private PlayerMap inputMap;
    private Vector2 inputValues;

    private List<GameObject> spawnedImages = new List<GameObject>();
    private int sound = -1;

    private PlatformMove movingPlatform;
    private Vector3 positionEscape;

    //Animaciones
    public Animator anim;
    private bool mirandoDerecha = true;

    public CinemachineAnimation cineMachine;

    private bool noteGreen = false;
    private bool noteYellow = false;
    private bool noteBlue = false;
    private bool noteRed = false;

    private bool isReseting = false;
    private float timeReset = 0f;

    public Book book;

    public bool[] hearingSound;

    public GameObject helpBall;
    private Vector3 originPositionHelpBall;
    private bool cooldownHideOptions = false;
    private SpriteRenderer spriteHelpBall;

    private Quaternion rotationCanMove;

    public CinemachineImpulseSource impulseSource;
    public UnityEngine.UI.Image vignetteImage;

    public CinemachineAnimation cinemachine;

    public Collider playerCollider;
    private Collider ballCollider;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private Coroutine corutineHideball;

    public Light spotlight;
    public Light spotlight2;
    public float flickerSpeed = 5f;
    public float angleVariation = 1f;
    public float innerAngleVariation = 0.5f;

    private float initialOuterAngle;
    private float initialInnerAngle;
    private float initialIntensity;

    private Coroutine damageCoroutine;

    private Colleccionable collecionable;

    private bool cooldawnLaunchBall;

    public GameObject sliderMusic;
    void Start()
    {
        
        GameManager.instance.canMove = true;
        Time.timeScale = 1f;

        hearingSound = new bool[4];

        sequence = new List<AudioClip>();

        controller = GetComponent<CharacterController>();

        GameManager.instance.playerMovement = this;


        GameManager.instance.Load();

        if (GameManager.instance.newGame || GameManager.instance.sharedData.player.positionZ == 0)
        {
            GameManager.instance.newGame = false;
            startPosition = transform.position;
        }
        else
        {
            ContinueGame();
        }

            
        audioSourceEffectPlayer = GetComponent<AudioSource>();
        
        ballLauch = false;

        playerCollider = this.GetComponent<Collider>();

        initialOuterAngle = spotlight.spotAngle;
        initialInnerAngle = spotlight.innerSpotAngle;

        foreach (RawImage image in notes)
        {
            image.color = Color.white;      
        }

        originPositionHelpBall = helpBall.transform.position;

        spriteHelpBall = helpBall.GetComponent<SpriteRenderer>();

        inputMap = new PlayerMap();
        inputMap.Enable();

        inputMap.Player.Movement.performed += DirKeysPerformed;
        inputMap.Player.Movement.canceled += DirKeysPerformed;
        inputMap.Player.Interact.performed += InterectPerformed;
        inputMap.Player.Interact.canceled += InterectCanceled;
        inputMap.Player.Sequence.performed += SequencePerformed;
        inputMap.Player.Sphere.performed += SpherePerformed;
        inputMap.Player.Sphere.canceled += SphereCanceled;
        inputMap.Player.Jump.performed += JumpPerformed;
        inputMap.Player.Jump.canceled += JumpCanceled;
        inputMap.Player.Options.performed += OptionsPerformed;
        inputMap.Player.Sound1.performed += Sound1Performed;
        inputMap.Player.Sound2.performed += Sound2Performed;
        inputMap.Player.Sound3.performed += Sound3Performed;
        inputMap.Player.Sound4.performed += Sound4Performed;
        inputMap.Player.Sound5.performed += Sound5Performed;
        inputMap.Player.Sound6.performed += Sound6Performed;
        inputMap.Player.Diary.performed += DiaryPerformed;
        inputMap.Player.Reset.performed += ResetPerformed;
        inputMap.Player.Reset.canceled += ResetCanceled;

        GameManager.instance.DisableCargar();
    }

    private void OnDestroy()
    {
        if (inputMap != null)
        {
            inputMap.Player.Movement.performed -= DirKeysPerformed;
            inputMap.Player.Movement.canceled -= DirKeysPerformed;
            inputMap.Player.Interact.performed -= InterectPerformed;
            inputMap.Player.Interact.canceled -= InterectCanceled;
            inputMap.Player.Sequence.performed += SequencePerformed;
            inputMap.Player.Sphere.performed -= SpherePerformed;
            inputMap.Player.Sphere.canceled -= SphereCanceled;
            inputMap.Player.Jump.performed -= JumpPerformed;
            inputMap.Player.Jump.canceled -= JumpCanceled;
            inputMap.Player.Options.performed -= OptionsPerformed;
            inputMap.Player.Sound1.performed -= Sound1Performed;
            inputMap.Player.Sound2.performed -= Sound2Performed;
            inputMap.Player.Sound3.performed -= Sound3Performed;
            inputMap.Player.Sound4.performed -= Sound4Performed;
            inputMap.Player.Sound5.performed -= Sound5Performed;
            inputMap.Player.Sound6.performed -= Sound6Performed;
            inputMap.Player.Diary.performed -= DiaryPerformed;
            inputMap.Player.Reset.performed -= ResetPerformed;
            inputMap.Player.Reset.canceled -= ResetCanceled;
        }
    }

    private void OnEnable()
    {
        souning = false;
    }

    private void OnDisable()
    {
        if (inputMap != null)
        {
            inputMap.Player.Movement.performed -= DirKeysPerformed;
            inputMap.Player.Movement.canceled -= DirKeysPerformed;
            inputMap.Player.Interact.performed -= InterectPerformed;
            inputMap.Player.Interact.canceled -= InterectCanceled;
            inputMap.Player.Sequence.performed -= SequencePerformed;
            inputMap.Player.Sphere.performed -= SpherePerformed;
            inputMap.Player.Sphere.canceled -= SphereCanceled;
            inputMap.Player.Jump.performed -= JumpPerformed;
            inputMap.Player.Jump.canceled -= JumpCanceled;
            inputMap.Player.Options.performed -= OptionsPerformed;
            inputMap.Player.Sound1.performed -= Sound1Performed;
            inputMap.Player.Sound2.performed -= Sound2Performed;
            inputMap.Player.Sound3.performed -= Sound3Performed;
            inputMap.Player.Sound4.performed -= Sound4Performed;
            inputMap.Player.Sound5.performed -= Sound5Performed;
            inputMap.Player.Sound6.performed -= Sound6Performed;
            inputMap.Player.Diary.performed -= DiaryPerformed;
            inputMap.Player.Reset.performed -= ResetPerformed;
            inputMap.Player.Reset.canceled -= ResetCanceled;
        }
    }

    void Update()
    {

        float wave = Mathf.Sin(Time.time * flickerSpeed);

        spotlight.spotAngle = initialOuterAngle + wave * angleVariation;
        spotlight.innerSpotAngle = initialInnerAngle + wave * innerAngleVariation;

        spotlight2.spotAngle = initialOuterAngle + wave * angleVariation;
        spotlight2.innerSpotAngle = initialInnerAngle + wave * innerAngleVariation;

        if (GameManager.instance.canMove) {
            rotationCanMove = transform.rotation;
        }
        else
        {
            transform.rotation = rotationCanMove;
        }
        
        moveInput.x = inputValues.x;
        moveInput.Normalize();

        if (moveInput.magnitude > 0.1f)
        {
            isMoving = true;
            if(GameManager.instance.canMove)
                anim.SetBool("IsWalking", true);
            
        }
        else
        {
            isMoving = false;
            if (GameManager.instance.canMove)
                anim.SetBool("IsWalking", false);
        }
            

        // Si está tocando el suelo (Floor), desactivamos la gravedad
        if (!controller.isGrounded)
        {
            coyoteTimeCounter -= Time.deltaTime;

            if (anim.GetBool("IsFalling"))
            {
                // Lanzamos un raycast para ver si está cerca del suelo
                RaycastHit hit;
                Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.8f, LayerMask.GetMask("Default"));

                if (hit.collider != null && !jumpCooldown && verticalVelocity < 0)
                {
                    anim.SetBool("IsJumping", false);
                    anim.SetBool("IsFalling", false);
                }
                else
                {
                    anim.SetBool("IsJumping", true);
                    anim.SetBool("IsFalling", true);
                }
            }
            else if(movingPlatform == null && Time.timeScale == 1 && !cooldownHideOptions) //se añade cooldwonHide para evitar un glitch de animacion con el menu de opciones
            {
                anim.SetBool("IsJumping", true);
                anim.SetBool("IsFalling", true);
            }
                


            if (isPressJumping)
                verticalVelocity += (gravityScale - gravityScale/3) * Time.deltaTime;
            else
                verticalVelocity += gravityScale * Time.deltaTime;

        }
        else
        {

            if (jumpCooldown)
            {
                anim.SetBool("IsJumping", true);
                anim.SetBool("IsFalling", true);
            }
            else
            {
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsFalling", false);
            }

            coyoteTimeCounter = coyoteTime;
        }

        

        if (!GameManager.instance.canMove)
            moveInput.x = 0;

        //Move the player
        if (isMoving)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, moveInput * moveSpeed, Time.deltaTime * 10f);
            if (currentVelocity.x > 5)
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if(currentVelocity.x < -5)
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            //Player inertia
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * 15f);
        }

        if(isHit && verticalVelocity > 0)
        {
            verticalVelocity = 0;
            
        }

        if (verticalVelocity < -30)
            verticalVelocity = -30;


        currentVelocity.y = verticalVelocity;

       // if(GameManager.instance.canMove)
            controller.Move(currentVelocity * Time.deltaTime);



	    float enemyDistance = CheckEnemyAround();
        if (enemyDistance != -1)
        {

            //audioSourceMusic.pitch = Mathf.Lerp(1.0f, 0.7f, Time.deltaTime * 2);
            audioSourceMusic.pitch = 0.5f;
            flickerSpeed = ConvertValue(enemyDistance);
        }
        else if (audioSourceMusic.pitch != 1)
        {
            audioSourceMusic.pitch = 1;
            flickerSpeed = 5;
        }


        if (updateSliderBall && !menuPause.activeSelf)
        {
            sliderBall.gameObject.SetActive(true);
            timerSliderBall += Time.deltaTime;
            sliderBall.value = Mathf.Clamp01(timerSliderBall / cooldownBall);

            if (timerSliderBall >= cooldownBall)
            {
                updateSliderBall = false;
                timerSliderBall = 0f;
                sliderBall.gameObject.SetActive(false);
            }
        }

        if (isReseting && controller.isGrounded && !isMoving) {
            timeReset += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 0.2f, timeReset / 2);

            Color color = vignetteImage.color;
            color.a = alpha;
            vignetteImage.color = color;

            if (timeReset > 2)
            {
                GameManager.instance.vibration.VibrarMando((2f), 0.5f);
                this.transform.position = startPosition + new Vector3(0, 1, 0);

                if (startToLeft)
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                else
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            timeReset = 0f;
        }

        if (helpBall.activeInHierarchy)
        {
            
            Vector3 direction = Vector3.zero;
            float rawAngle = 0f;
            Vector3 hitPoint = Vector3.zero;

            if (Gamepad.current != null)
            {
                Vector2 stickInput = Gamepad.current.rightStick.ReadValue();

                if (stickInput.magnitude > 0.1f) // Umbral para evitar ruido del stick
                {
                    spriteHelpBall.enabled = true;
                    direction = new Vector3(stickInput.x, stickInput.y, 0f);
                    rawAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    hitPoint = transform.position + direction.normalized * 5f; // Punto de referencia, ajusta el multiplicador a gusto
                }
                else
                {
                    spriteHelpBall.enabled = false;
                    return; // No hay input del joystick
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.forward, -7f);

                if (plane.Raycast(ray, out float enter))
                {
                    hitPoint = ray.GetPoint(enter);
                    Debug.DrawLine(ray.origin, hitPoint, Color.red);
                    Vector3 flatHitPoint = new Vector3(hitPoint.x, hitPoint.y, helpBall.transform.position.z);
                    direction = hitPoint - helpBall.transform.position;
                    rawAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                }
                else
                {
                    return; // No impacto del rayo
                }
            }

            float playerYRotation = transform.rotation.eulerAngles.y;
            float finalAngle = rawAngle;

            // Mirando a la izquierda
            if (Mathf.Abs(playerYRotation - 180f) < 1f)
            {
                helpBall.GetComponent<SpriteRenderer>().flipY = true;
                if (rawAngle > -125f && rawAngle < 125f)
                {
                    finalAngle = rawAngle >= 0 ? 125f : -125f;
                }
            }
            else // Mirando a la derecha
            {
                helpBall.GetComponent<SpriteRenderer>().flipY = false;
                finalAngle = Mathf.Clamp(rawAngle, -55f, 55f);
            }

            helpBall.transform.rotation = Quaternion.Euler(0, 0, finalAngle);

            float t = 0f;
            if (Gamepad.current != null)
            {
                t = Mathf.Clamp01(direction.magnitude); // Ya normalizado, entre 0 y 1
            }
            else
            {
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                float distanceFromCenter = Vector2.Distance(Input.mousePosition, screenCenter);
                float maxScreenDistance = screenCenter.magnitude;
                t = distanceFromCenter / maxScreenDistance;
            }

            float minScale = 0.15f;
            float maxScale = 0.45f;
            float finalScale = Mathf.Lerp(minScale, maxScale, t);

            if ((playerYRotation < 0.1f && playerYRotation > -0.1f && hitPoint.x < transform.position.x + 1.5f) ||
                (playerYRotation < 180.1f && playerYRotation > 179.9f && hitPoint.x > transform.position.x - 1.5f))
            {
                finalScale = minScale / 2;
                helpBall.transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteHelpBall.enabled = false;
            }
            else
            {
                spriteHelpBall.enabled = true;
            }

            helpBall.transform.localScale = new Vector3(finalScale, 1, 1);
        }

    }



    private void ContinueGame()
    {
        if (GameManager.instance.continueGame)
        {
            if (noteGreen)
            {
                ObjectSong script = listSong[0].GetComponent<ObjectSong>();

                SpawnImage(script.color);

                AudioSource audio = listSong[0].GetComponent<AudioSource>();

                sequence.Add(audio.clip);

                Transform child = listSong[0].transform.GetChild(0);
                child.gameObject.SetActive(false);
            }

            if (noteRed)
            {
                ObjectSong script = listSong[1].GetComponent<ObjectSong>();

                SpawnImage(script.color);

                AudioSource audio = listSong[1].GetComponent<AudioSource>();

                sequence.Add(audio.clip);

                Transform child = listSong[1].transform.GetChild(0);
                child.gameObject.SetActive(false);
            }

            if (noteYellow)
            {
                ObjectSong script = listSong[2].GetComponent<ObjectSong>();

                SpawnImage(script.color);

                AudioSource audio = listSong[2].GetComponent<AudioSource>();

                sequence.Add(audio.clip);

                Transform child = listSong[2].transform.GetChild(0);
                child.gameObject.SetActive(false);
            }

            if (noteBlue)
            {
                ObjectSong script = listSong[3].GetComponent<ObjectSong>();

                SpawnImage(script.color);

                AudioSource audio = listSong[3].GetComponent<AudioSource>();

                sequence.Add(audio.clip);

                Transform child = listSong[3].transform.GetChild(0);
                child.gameObject.SetActive(false);
            }

            if (GameManager.instance.isTakeColeccionable1)
            {
                int index = 0 * 2 + 1;

                book.bookPages[index] = book.bookPageWrittenEng[index];
                book.bookPages[index + 1] = book.bookPageWrittenEng[index + 1];
            }
            else
            {
                book.bookPages[1] = book.bloqued;
                book.bookPages[2] = book.bloqued;
            }

            if (GameManager.instance.isTakeColeccionable2)
            {
                int index = 1 * 2 + 1;

                book.bookPages[index] = book.bookPageWrittenEng[index];
                book.bookPages[index + 1] = book.bookPageWrittenEng[index + 1];
            }
            else
            {
                book.bookPages[3] = book.bloqued;
                book.bookPages[4] = book.bloqued;
            }

            if (GameManager.instance.isTakeColeccionable2)
            {
                int index = 2 * 2 + 1;

                book.bookPages[index] = book.bookPageWrittenEng[index];
                book.bookPages[index + 1] = book.bookPageWrittenEng[index + 1];
            }
            else
            {
                book.bookPages[5] = book.bloqued;
                book.bookPages[6] = book.bloqued;
            }

            controller.enabled = false;
            this.transform.position = startPosition + new Vector3(0, 1, 0);
            controller.enabled = true;
        }
    }
    public void DirKeysPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        inputValues = obj.ReadValue<Vector2>();

        if (inputValues.x > 0 && !mirandoDerecha)
        {
            mirandoDerecha = !mirandoDerecha;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (inputValues.x < 0 && mirandoDerecha)
        {
            mirandoDerecha = !mirandoDerecha;
            transform.Rotate(0f, 180f, 0f);
        }
    }



    public void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isPressJumping = true;

         if (controller.isGrounded && !jumpCooldown && GameManager.instance.canMove)
                 StartCoroutine(Jump());

        if (GameManager.instance.playerMovePlatform && !jumpCooldown && GameManager.instance.canMove)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
                StartCoroutine(Jump());

        }


    }


    public void JumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isPressJumping = false;
    }

    public void OptionsPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!GameManager.instance.onAnimation)
        {
            if (menuPause.activeSelf)
            {

                EventSystem.current.SetSelectedGameObject(null);
                GameManager.instance.musicEnemy = true;

                if (Gamepad.current != null)
                    UnityEngine.Cursor.visible = false;

                menuPause.gameObject.SetActive(false);

                vignetteImage.gameObject.SetActive(true);
                GameManager.instance.canMove = true;
                sprite.gameObject.SetActive(true);

                if (sliderBall.value != 1)
                    sliderBall.gameObject.SetActive(true);

                if (hotspot != null)
                    hotspot.ShowControl();
                if (finishLevel != null)
                    finishLevel.ShowControl();

                GameManager.instance.SaveMusic();
                cooldownHideOptions = true;
                Time.timeScale = 1;
                StartCoroutine(ResetCooldownOptions());

                foreach (Transform hijo in canvasTransform)
                {
                    if (hijo.name == "CirculoNota(Clone)")
                    {
                        hijo.gameObject.SetActive(true);
                    }
                }
            }
            else if (!book.gameObject.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(sliderMusic);

                GameManager.instance.musicEnemy = false;

                if (Gamepad.current != null)
                    UnityEngine.Cursor.visible = true;

                GameManager.instance.canMove = false;
                Time.timeScale = 0;
                sliderBall.gameObject.SetActive(false);
                vignetteImage.gameObject.SetActive(false);

                menuPause.gameObject.SetActive(true);
                if (hotspot != null)
                    hotspot.HideControl();
                if (finishLevel != null)
                    finishLevel.HideControl();

                foreach (Transform hijo in canvasTransform)
                {
                    if (hijo.name == "CirculoNota(Clone)")
                    {
                        hijo.gameObject.SetActive(false);
                    }
                }
            }else if (book.gameObject.activeSelf)
            {
                GameManager.instance.musicEnemy = true;

                if (Gamepad.current != null)
                    UnityEngine.Cursor.visible = false;

                GameManager.instance.canMove = true;
                diary.SetActive(false);
                // Time.timeScale = 1;

                foreach (Transform hijo in canvasTransform)
                {
                    if (hijo.name == "CirculoNota(Clone)")
                    {
                        hijo.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public IEnumerator ResetCooldownOptions()
    {
        yield return new WaitForSeconds(0.1f);
        cooldownHideOptions = false;
    }

    public void InterectCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Invoke("SetIsHittingFalse", 0.6f);
    }

    public void SetIsHittingFalse()
    {
        anim.SetBool("IsHitting", false);
    }

    public void DiaryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        if (diary.activeSelf)
        {
            GameManager.instance.musicEnemy = true;

            if (Gamepad.current != null)
                UnityEngine.Cursor.visible = false;

            audioSourceEffectPlayer.clip = audioOpenDiary;
            audioSourceEffectPlayer.Play();

            GameManager.instance.canMove = true;
            diary.SetActive(false);
           // Time.timeScale = 1;

            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    hijo.gameObject.SetActive(true);
                }
            }
        }
        else if(!menuPause.activeSelf)
        {
            GameManager.instance.musicEnemy = false;

            if (Gamepad.current != null)
                UnityEngine.Cursor.visible = true;

            audioSourceEffectPlayer.clip = audioOpenDiary;
            audioSourceEffectPlayer.Play();

            GameManager.instance.canMove = false;
            //Time.timeScale = 0;
            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    hijo.gameObject.SetActive(false);
                }
            }

            diary.SetActive(true);
        }
    }

    public void ResetPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isReseting = true;
    }

    public void ResetCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isReseting = false;
        Color color = vignetteImage.color;
        color.a = 0f; // Comienza opaco
        vignetteImage.color = color;
    }

    public void InterectPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       
        if (isOnFinishLevel && finishLevel != null && !finishLevel.doorOpen)
        {
          /*  GameManager.instance.canMove = false;
            anim.SetBool("IsHitting", true);

            RuntimeAnimatorController controller = anim.runtimeAnimatorController;
            float duration = 0;
            foreach (AnimationClip clip in controller.animationClips)
            {
                if (clip.name == "Hit_Animation")
                {
                    duration = clip.length;
                }
            }

            Invoke("DesactiveCanMove", duration);*/

        }
        else if ( isOnHotSpot && hotspot != null && GameManager.instance.canMove)
        {
            GameManager.instance.canMove = false;
            anim.SetBool("IsHitting", true);

            RuntimeAnimatorController controller = anim.runtimeAnimatorController;
            float duration = 0;
            foreach (AnimationClip clip in controller.animationClips)
            {
                if (clip.name == "Hit_Animation")
                {
                    duration = clip.length;
                }
            }

            GameManager.instance.vibration.VibrarMando(0.5f, 0.5f);

            hotspot.Invoke("AudioPlay", 0.6f);
            hotspot.Invoke("ActivateLights", (duration/3)*2);
            Invoke("DesactiveCanMove", duration);
            impulseSource.Invoke("GenerateImpulse", duration/2);

        }
        else if (collecionable != null)
        {
            int index = collecionable.indexCollecionable * 2 + 1;

            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                book.bookPages[index] = book.bookPageWrittenEsp[index-1];
                book.bookPages[index + 1] = book.bookPageWrittenEsp[index];
            }
            else if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                book.bookPages[index] = book.bookPageWrittenVal[index-1];
                book.bookPages[index + 1] = book.bookPageWrittenVal[index];
            }
            else if (GameManager.instance.idiom == GameManager.Language.English)
            {
                book.bookPages[index] = book.bookPageWrittenEng[index-1];
                book.bookPages[index + 1] = book.bookPageWrittenEng[index];
            }
            else
            {
                book.bookPages[index] = book.bookPageWrittenChi[index - 1];
                book.bookPages[index + 1] = book.bookPageWrittenChi[index];
            }

            collecionable.gameObject.SetActive(false);

            if (collecionable.indexCollecionable == 0)
            {
                GameManager.instance.isTakeColeccionable1 = true;
            }
            else if (collecionable.indexCollecionable == 1)
            {
                GameManager.instance.isTakeColeccionable2 = true;
            }
            else if (collecionable.indexCollecionable == 2)
            {
                GameManager.instance.isTakeColeccionable3 = true;
            }

            anim.SetBool("IsHitting", true);
            audioSourceEffectPlayer.clip = audioTakeNote;
            audioSourceEffectPlayer.Play();
        }

    }

    private void DesactiveCanMove()
    {
        GameManager.instance.canMove = true;
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
        if(GameManager.instance.canMove)
            helpBall.SetActive(true);
    }

    public void SphereCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!ballLauch && spriteHelpBall.enabled && !cooldawnLaunchBall && GameManager.instance.canMove)
        {
            LaunchBall();
        }
        helpBall.SetActive(false);
    }

    public void TakeSoundPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isNearObjectSong && objectSong != null)
        {
            Color color = objectSong.TakeItem();

            SpawnImage(color);
            
            sequence.Add(objectSong.audioSource.clip);

        }
    }
   
    public void Sound1Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 0;

        if(sequence.Count > sound && !souning)
        {
            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    var i = hijo.transform.Find("Text (TMP)");
                    if (i != null && (i.GetComponent<TextMeshProUGUI>().text == (sound + 1).ToString("0") || i.GetComponent<TextMeshProUGUI>().text == "<b>↑</b>"))
                    {
                        hijo.GetComponent<AcousticWave>().xianShi(sequence[sound].length);
                        Color newcolor = hijo.GetComponent<RawImage>().color;
                        footSoundWavesSprite.color = newcolor;
                        footSoundWaves.xianShi(sequence[sound].length);
                    }

                }
            }

            GameManager.instance.vibration.VibrarMando((0.5f), 0.5f);

            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());

            if (movingPlatform != null)
            {
                movingPlatform.MovePlatform(1);
               
            }
        }
    }
    public void Sound2Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 1;

        if (sequence.Count > sound && !souning)
        {
            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    var i = hijo.transform.Find("Text (TMP)");
                    if (i != null && (i.GetComponent<TextMeshProUGUI>().text == (sound + 1).ToString("0") || i.GetComponent<TextMeshProUGUI>().text == "<b>→</b>"))
                    {
                        hijo.GetComponent<AcousticWave>().xianShi(sequence[sound].length);
                        Color newcolor = hijo.GetComponent<RawImage>().color;
                        footSoundWavesSprite.color = newcolor;
                        footSoundWaves.xianShi(sequence[sound].length);
                    }

                }
            }

            GameManager.instance.vibration.VibrarMando((0.5f), 0.5f);

            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());

            if (movingPlatform != null)
            {
                movingPlatform.MovePlatform(2);
               
            }
        }
    }
    public void Sound3Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 2;

        if (sequence.Count > sound && !souning)
        {
            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    var i = hijo.transform.Find("Text (TMP)");
                    if (i != null && (i.GetComponent<TextMeshProUGUI>().text == (sound + 1).ToString("0") || i.GetComponent<TextMeshProUGUI>().text == "<b>↓</b>"))
                    {
                        hijo.GetComponent<AcousticWave>().xianShi(sequence[sound].length);
                        Color newcolor = hijo.GetComponent<RawImage>().color;
                        footSoundWavesSprite.color = newcolor;
                        footSoundWaves.xianShi(sequence[sound].length);
                    }

                }
            }

            GameManager.instance.vibration.VibrarMando((0.5f), 0.5f);

            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());
            
        }
    }
    public void Sound4Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 3;

        if (sequence.Count > sound && !souning)
        {
            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    var i = hijo.transform.Find("Text (TMP)");
                    if (i != null && (i.GetComponent<TextMeshProUGUI>().text == (sound + 1).ToString("0") || i.GetComponent<TextMeshProUGUI>().text == "<b>←</b>"))
                    {
                        hijo.GetComponent<AcousticWave>().xianShi(sequence[sound].length);
                        Color newcolor = hijo.GetComponent<RawImage>().color;
                        footSoundWavesSprite.color = newcolor;
                        footSoundWaves.xianShi(sequence[sound].length);
                    }

                }
            }

            GameManager.instance.vibration.VibrarMando((0.5f), 0.5f);

            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());
           


        }
    }
    public void Sound5Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 4;

        if (sequence.Count > sound && !souning)
        {
            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    var i = hijo.transform.Find("Text (TMP)");
                    if (i != null && i.GetComponent<TextMeshProUGUI>().text == (sound + 1).ToString("0"))
                    {
                        hijo.GetComponent<AcousticWave>().xianShi(sequence[sound].length);
                        Color newcolor = hijo.GetComponent<RawImage>().color;
                        footSoundWavesSprite.color = newcolor;
                        footSoundWaves.xianShi(sequence[sound].length);
                    }

                }
            }

            GameManager.instance.vibration.VibrarMando((0.5f), 0.5f);

            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());
           
        }
    }
    public void Sound6Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 5;

        if (sequence.Count > sound && !souning)
        {
            foreach (Transform hijo in canvasTransform)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    var i = hijo.transform.Find("Text (TMP)");
                    if (i != null && i.GetComponent<TextMeshProUGUI>().text == (sound + 1).ToString("0"))
                    {
                        hijo.GetComponent<AcousticWave>().xianShi(sequence[sound].length);
                        Color newcolor = hijo.GetComponent<RawImage>().color;
                        footSoundWavesSprite.color = newcolor;
                        footSoundWaves.xianShi(sequence[sound].length);
                    }

                }
            }
            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());
            
        }
    }

    public void SoundToDoor(AudioClip clip)
    {
        if (isOnFinishLevel && finishLevel != null && !finishLevel.doorOpen)
        {
            finishLevel.RegisterSound(clip);
        }
    }
    private IEnumerator WaitForSoundToEnd()
    {
        yield return new WaitUntil(() => !audioSourceSequence.isPlaying);
        souning = false;
    }

    public void SpawnImage(Color color)
    {
        GameObject newImage = Instantiate(imagePrefab, canvasTransform);
        RawImage image = newImage.GetComponent<RawImage>();
        TextMeshProUGUI text = newImage.GetComponentInChildren<TextMeshProUGUI>();
        color.a = 1;
        image.color = color;

        if (Gamepad.current != null)
        {
            if ((spawnedImages.Count + 1) == 1)
            {
                text.text = "<b>↑</b>";
                text.transform.localPosition = new Vector3((text.transform.localPosition.x - 0.5f), (text.transform.localPosition.y + 20f), text.transform.localPosition.z);
            }
            else if ((spawnedImages.Count + 1) == 2)
            {
                text.text = "<b>→</b>";
                text.transform.localPosition = new Vector3((text.transform.localPosition.x - 2.5f), (text.transform.localPosition.y + 40f), text.transform.localPosition.z);
            }
            else if ((spawnedImages.Count + 1) == 3)
            {
                text.text = "<b>↓</b>";
                text.transform.localPosition = new Vector3((text.transform.localPosition.x - 0.5f), (text.transform.localPosition.y + 20f), text.transform.localPosition.z);
            }
            else if ((spawnedImages.Count + 1) == 4)
            {
                text.text = "<b>←</b>";
                text.transform.localPosition = new Vector3((text.transform.localPosition.x - 2.5f), (text.transform.localPosition.y + 40f), text.transform.localPosition.z);
            }

            text.fontSize = 40f;
        }
        else
        {
            text.text = (spawnedImages.Count + 1).ToString();
            text.fontSize = 30f;
        }
        spawnedImages.Add(newImage);

        // Ajustar la posición de todas las imágenes
        UpdateImagePositions();

    }

    public void AddSequence(AudioClip clip)
    {
        sequence.Add(clip);
    }

    public void ResetImage()
    {
        sequence.Clear();

        foreach(GameObject image in spawnedImages){
            Destroy(image);
        }

        spawnedImages.Clear();
    }

    private void UpdateImagePositions()
    {
        int count = spawnedImages.Count;
        float startX = -(count - 1) * spacing / 2;  // Centra las imágenes

        for (int i = 0; i < count; i++)
        {
            RectTransform rectTransform = spawnedImages[i].GetComponent<RectTransform>();

            // Posición en la parte baja del Canvas
            rectTransform.anchoredPosition = new Vector2(startX + (i * spacing), -yOffset);

            Vector3 localPos = rectTransform.localPosition;
            localPos.z = -25;
            localPos.y = -180;
            rectTransform.localPosition = localPos;
        }
    }

    // Método para aplicar el salto
    IEnumerator Jump()
    {
        jumpCooldown = true;
        coyoteTime = 0;
        audioSourceEffectPlayer.clip = audioJump;
        audioSourceEffectPlayer.Play();
        yield return null;
        anim.SetBool("IsJumping", true);
        anim.SetBool("IsFalling", true);
        yield return new WaitForSeconds(0.1f);
        isHit = false;
       // if(controller.isGrounded || movingPlatform != null)
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravityScale);
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
        return finishLevel.audioClips.All(item => sequence.Any(audio => audio.name == item.name)); ;
    }

    void LaunchBall()
    {
        if (poolBall != null)
        {
            cooldawnLaunchBall = true;

            Invoke("SetCooldawnLaunchBall", 0.5f);

            Vector3 rawDirection = Vector3.zero;
            float rawAngle = 0f;
            Vector3 mousePosition = Vector3.zero;

            bool useGamepad = Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0.1f;

            if (useGamepad)
            {
                Vector2 stickInput = Gamepad.current.rightStick.ReadValue();
                rawDirection = new Vector3(stickInput.x, stickInput.y, 0f).normalized;
                rawAngle = Mathf.Atan2(rawDirection.y, rawDirection.x) * Mathf.Rad2Deg;
            }
            else
            {
                mousePosition = GetMouseWorldPosition();
                rawDirection = (mousePosition - transform.position).normalized;
                rawDirection.z = 0;
                rawAngle = Mathf.Atan2(rawDirection.y, rawDirection.x) * Mathf.Rad2Deg;
            }

            float playerYRotation = transform.rotation.eulerAngles.y;

            // Cancelar si la dirección no coincide con la rotación del personaje
            if (!useGamepad)
            {
                float directionToMouseX = mousePosition.x - transform.position.x;

                if (playerYRotation == 0 && directionToMouseX < 0)
                    return;

                if (playerYRotation == 180 && directionToMouseX > 0)
                    return;
            }
            else
            {
                if (playerYRotation == 0 && rawDirection.x < 0)
                    return;

                if (playerYRotation == 180 && rawDirection.x > 0)
                    return;
            }

            ballLauch = true;

            GameObject newBall = poolBall.GetInactivePrefab();
            if (newBall == null) return;

            newBall.gameObject.SetActive(true);

            ballCollider = newBall.GetComponent<Collider>();

            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            BallBounceHandler ballBuounce = newBall.GetComponent<BallBounceHandler>();
            if (rb != null && ballBuounce != null && ballCollider != null)
            {
                //Physics.IgnoreCollision(ballCollider, playerCollider, true);

                // Espera 2 segundos y vuelve a activar la colisión
                //StartCoroutine(EnableCollisionAfterDelay(2f));


                rb.isKinematic = false;

                // Limitar ángulos según orientación
                if (Mathf.Abs(playerYRotation - 180f) < 1f)
                {
                    if (rawAngle > -140f && rawAngle < 140f)
                        rawAngle = (rawAngle >= 0) ? 140f : -140f;
                }
                else
                {
                    rawAngle = Mathf.Clamp(rawAngle, -40f, 40f);
                }

                // Reconstruye la dirección desde el ángulo corregido
                float angleRad = rawAngle * Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0).normalized;

                // Usa la dirección cruda si apenas se corrigió
                if (Mathf.Abs(angleRad - Mathf.Atan2(rawDirection.y, rawDirection.x)) < 0.1f)
                    direction = rawDirection;

                ballBuounce.velocityY = direction.y;
                ballBuounce.velocityX = direction.x;
                ballBuounce.bounceCount = 0;
                ballBuounce.isAscending = false;

                rb.velocity = direction * 15;

                if (direction.x > 0)
                    newBall.transform.position = transform.position + new Vector3(2, 1, 0);
                else
                    newBall.transform.position = transform.position + new Vector3(-2, 1, 0);

                audioSourceEffectPlayer.clip = audioJump;
                audioSourceEffectPlayer.Play();
            }

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            // ... Aquí iría lo que tienes comentado

            corutineHideball = StartCoroutine(HideBall(newBall));
            updateSliderBall = true;
        }
    }

    void SetCooldawnLaunchBall()
    {
        cooldawnLaunchBall = false;
    }

    IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Ahora sí permitir colisión
        Physics.IgnoreCollision(ballCollider, playerCollider, false);
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

    IEnumerator HideBall(GameObject newBall)
    {
        float elapsed = 0f;

        while (elapsed < cooldownBall)
        {
            if (!menuPause.activeSelf) // Solo contar si el menú no está activo
            {
                elapsed += Time.deltaTime;
            }

            yield return null; // Esperar al siguiente frame
        }

        newBall.SetActive(false);

        BallBounceHandler ballScript = newBall.GetComponent<BallBounceHandler>();
        ballScript.TurnOffLight();

        SphereCollider colliderBall = newBall.GetComponent<SphereCollider>();
        colliderBall.isTrigger = false;

        ballLauch = false;
    }

    public void Dead()
    {
        GameManager.instance.canMove = false;
        Invoke("RestartMoveAfterDead", 0.5f);

        GameManager.instance.vibration.VibrarMando((2f), 0.5f);

        audioSourceEffectPlayer.clip = audioDead;
        audioSourceEffectPlayer.Play();

        controller.enabled = false;
        transform.position = startPosition + new Vector3(0, 1, 0);
        controller.enabled = true;

        if (startToLeft)
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

        rotationCanMove = transform.rotation;

        currentVelocity = Vector3.zero;

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        imageDamage.gameObject.SetActive(true);
        damageCoroutine = StartCoroutine(TrasparentDamage(3f));
    }

    private void RestartMoveAfterDead()
    {
        GameManager.instance.canMove = true;
    }

    private IEnumerator TrasparentDamage(float duration)
    {
        Color startColor = imageDamage.color;
        float startAlpha = 1;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            imageDamage.color = newColor;
            yield return null;
        }

        imageDamage.gameObject.SetActive(false);

        Color finalColor = imageDamage.color;
        finalColor.a = 1f;
        imageDamage.color = finalColor;
        damageCoroutine = null;
    }

    private float CheckEnemyAround()
    {
        float closestDistance = float.MaxValue;

        foreach (Enemy enemy in listEnemy)
        {
            float distancia = Vector3.Distance(transform.position, enemy.transform.position);
            if (distancia < 15f && distancia < closestDistance)
            {
                closestDistance = distancia;
            }
        }
        return closestDistance == float.MaxValue ? -1f : closestDistance;
    }

    public float ConvertValue(float input)
    {
        // Rango de salida: de 20 a 7
        float outputMin = 25f;
        float outputMax = 10f;

        // Clamp el valor para evitar salirse del rango
        input = Mathf.Clamp(input, 5, 15);

        // Interpolación inversa
        float t = (input - 5) / (15 - 5);
        return Mathf.Lerp(outputMin, outputMax, t);
    }

    // Detectar cuando sale del suelo
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HotSpot" && hotspot != null)
        {
            isOnHotSpot = false;
            hotspot.HideControl();
            hotspot = null;
        }
        else if (other.tag == "FloorObjectSong" && objectSong != null)
        {
            isNearObjectSong = false;
           // objectSong.HideControl();
            objectSong = null;
        }
        else if (other.gameObject.CompareTag("FinishLevel") /*&& finishLevel != null*/)
        {
            isOnFinishLevel = false;
            finishLevel.HideControl();
        }else if (other.gameObject.CompareTag("PlatformMove") && movingPlatform != null)
        {
            movingPlatform.ResetEffect();
            movingPlatform = null;
        }if (other.CompareTag("HearingSound"))
        {
            HeardSound heard = other.GetComponent<HeardSound>();
            if (heard != null)
            {
                hearingSound[heard.heardSound] = false;
            }
        }
        else if (other.gameObject.CompareTag("Collectionable"))
        {
            collecionable = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HearingSound"))
        {
            HeardSound heard = other.GetComponent<HeardSound>();
            if (heard != null)
            {
                hearingSound[heard.heardSound] = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HotSpot")
        {
            isOnHotSpot = true;
            hotspot = other.GetComponent<HotSpot>();
            hotspot.ShowControl();

            if (currentIndexHotSpot <= hotspot.numHotSpot)
            {
                startPosition = transform.position;
                startToLeft = hotspot.continueLeft;
            }

            GameManager.instance.Save();
        }
        else if (other.tag == "FloorObjectSong")
        {
            isNearObjectSong = true;
            objectSong = other.GetComponent<ObjectSong>();
        }
        else if (other.tag == "EntryBoss" && !onBoss)
        {
            startPosition = this.transform.position;
            onBoss = true;
            bossLight = other.GetComponent<BossLight>();
            StartCoroutine(bossLight.Blink());

            cinemachine.PlayTimelineLevel5();

        }
        else if (other.gameObject.CompareTag("Ball"))
        {
            BallBounceHandler ballScript = other.gameObject.GetComponent<BallBounceHandler>();

            if (ballScript != null)
            {
                // Llamar a una función dentro del script si es necesario
                ballScript.TurnOffLight();
            }
            other.gameObject.SetActive(false);

            SphereCollider colliderBall = other.gameObject.GetComponent<SphereCollider>();
            ballLauch = false;


            if (colliderBall != null) colliderBall.isTrigger = false;

            sliderBall.gameObject.SetActive(false);
            updateSliderBall = false;
            timerSliderBall = 0f;
            sliderBall.value = 1f;

            StopCoroutine(corutineHideball);
        }
        else if (other.gameObject.CompareTag("FinishLevel"))
        {
            isOnFinishLevel = true;
            finishLevel = other.GetComponent<FinishLevel>();
            finishLevel.ShowControl();
            if (GameManager.instance.helpControls)
            {
                // StartCoroutine(finishLevel.ShowAdvice());
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null && !enemy.isStunned)
                Dead();
            else if (other.gameObject.layer == LayerMask.NameToLayer("Machacadores"))
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                float dot = Vector3.Dot(direction, Vector3.up);

                Debug.Log(dot);
                // Si dot > 0.5, el machacador está "encima" de ti (viniendo desde arriba)
                if (dot > 0.75f || dot < -0.75f)
                {
                    Dead(); // Te aplastó
                }
            }
            else if (other.gameObject.name.Contains("Cylinder"))
            {
                Dead();
            }
        }
        else if (other.gameObject.CompareTag("PlatformMove"))
        {
            if (other.gameObject.TryGetComponent<PlatformMove>(out PlatformMove mover))
            {
                movingPlatform = mover;
                movingPlatform.ActivateEffect();
            }
        }
        else if (other.gameObject.CompareTag("ObjetoCancion"))
        {

            if (isNearObjectSong && objectSong != null)
            {
                Color color = objectSong.TakeItem();

                SpawnImage(color);

                sequence.Add(objectSong.audioSource.clip);

                audioSourceEffectPlayer.clip = audioTakeNote;
                audioSourceEffectPlayer.Play();

                if (other.gameObject.name == "BolaCancionGreen")
                {
                    noteGreen = true;
                }
                if (other.gameObject.name == "BolaCancionRed")
                {
                    noteRed = true;
                }
                if (other.gameObject.name == "BolaCancionYellow")
                {
                    noteYellow = true;
                }
                if (other.gameObject.name == "BolaCancionBlue")
                {
                    noteBlue = true;
                }
            }
        }
        else if (other.gameObject.CompareTag("DefeatBoss"))
        {
            GameManager.instance.defeatBoss = true;
        }
        else if (other.gameObject.CompareTag("Animation2") && !GameManager.instance.isSeenCinematic2)
        {
            anim.SetBool("IsWalking", false);
            GameManager.instance.canMove = false;
            canvasTransform.gameObject.SetActive(false);
            cineMachine.PlayTimelineLevel2();
            other.gameObject.SetActive(false);
            GameManager.instance.isSeenCinematic2 = true;
        }
        else if (other.gameObject.CompareTag("Animation3") && !GameManager.instance.isSeenCinematic3)
        {
            anim.SetBool("IsWalking", false);
            GameManager.instance.canMove = false;
            canvasTransform.gameObject.SetActive(false);
            cineMachine.PlayTimelineLevel3();
            other.gameObject.SetActive(false);
            GameManager.instance.isSeenCinematic3 = true;
        }
        else if (other.gameObject.CompareTag("Animation4") && !GameManager.instance.isSeenCinematic4)
        {
            anim.SetBool("IsWalking", false);
            GameManager.instance.canMove = false;
            canvasTransform.gameObject.SetActive(false);
            cineMachine.PlayTimelineLevel4();
            other.gameObject.SetActive(false);
            GameManager.instance.isSeenCinematic4 = true;

        }
        else if (other.gameObject.CompareTag("Collectionable"))
        {
            collecionable = other.GetComponent<Colleccionable>();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float direction = Vector3.Dot(hit.normal, Vector3.down);
        if (direction < 0.1f && direction > -0.1f)
        {
            currentVelocity.x = 0;
        }

        if (Vector3.Dot(hit.normal, Vector3.down) > 0.5f)
        {
            isHit = true;
        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = hit.gameObject.GetComponent<Enemy>();
            if(enemy != null && !enemy.isStunned)
                Dead();
            else if (hit.gameObject.layer == LayerMask.NameToLayer("Machacadores"))
            {
                if (Vector3.Dot(hit.normal, Vector3.down) > 0.75f)
                {
                    Dead();
                }
            }
        }
    }



    public Vector3 GetStartPosition()
    {
        return startPosition;
    }

    public bool GetNoteGreen()
    {
        return noteGreen;
    }

    public bool GetNoteBlue()
    {
        return noteBlue;
    }

    public bool GetNoteRed()
    {
        return noteRed;
    }

    public bool GetNoteYellow()
    {
        return noteYellow;
    }
    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
    }

    public void SetNoteGreen(bool note)
    {
        noteGreen = note;
    }

    public void SetNoteBlue(bool note)
    {
        noteBlue = note;
    }

    public void SetNoteRed(bool note)
    {
        noteRed = note;
    }

    public void SetNoteYellow(bool note)
    {
        noteYellow = note;
    }
}

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

public class PlayerMovement : MonoBehaviour
{
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
    public AudioClip aduioJump;

    public List<Enemy> listEnemy = new List<Enemy>();
    public List<GameObject> listSong = new List<GameObject>();
    public AudioSource audioSourceMusic;

    public GameObject imagePrefab;  // Arrastra aquí el prefab en el Inspector
    public Transform canvasTransform; 
    public float yOffset = 50f;  // Distancia desde la parte baja del Canvas
    public float spacing = 100f; // Espacio entre imágenes

    public SpriteRenderer sprite;
    public GameObject layout;

    private CharacterController controller;
    private Vector3 moveInput;
    private bool isOnHotSpot;
    private HotSpot hotspot;

    private bool isPressJumping;
    private float launchForce = 5f; // Fuerza con la que se lanza la bola

    private bool updateSliderBall = false;
    private float timerSliderBall = 0f;

    private Vector3 startPosition;
    private float verticalVelocity;
    private bool isMoving;
    private bool isHit = false;
    private Vector3 currentVelocity;
    private bool jumpCooldown;
    private int currentIndex = 0;
    // private int indexBallImage = 0;
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

    private PlatformMove plataformaMovimiento;
    private Vector3 positionEscape;

    //Animaciones
    public Animator anim;
    private bool mirandoDerecha = true;

    public CinemachineAnimation cineMachine;

    private bool noteGreen = false;
    private bool noteYellow = false;
    private bool noteBlue = false;
    private bool noteRed = false;

    void Start()
    {
        sequence = new List<AudioClip>();

        GameManager.instance.playerMovement = this;
        if (GameManager.instance.Load())
        {
            ContinueGame();
        }
        else
            startPosition = transform.position;

        controller = GetComponent<CharacterController>();
        audioSourceEffectPlayer = GetComponent<AudioSource>();
        
        ballLauch = false;

        foreach (RawImage image in notes)
        {
            image.color = Color.white;      
        }

        inputMap = new PlayerMap();
        inputMap.Enable();

        inputMap.Player.Movement.performed += DirKeysPerformed;
        inputMap.Player.Movement.canceled += DirKeysPerformed;
        inputMap.Player.Interact.performed += InterectPerformed;
        inputMap.Player.Interact.canceled += InterectCanceled;
        inputMap.Player.Sequence.performed += SequencePerformed;
        inputMap.Player.Sphere.performed += SpherePerformed;
        inputMap.Player.Jump.performed += JumpPerformed;
        inputMap.Player.Jump.canceled += JumpCanceled;
        inputMap.Player.Options.performed += OptionsPerformed;
        inputMap.Player.Sound1.performed += Sound1Performed;
        inputMap.Player.Sound2.performed += Sound2Performed;
        inputMap.Player.Sound3.performed += Sound3Performed;
        inputMap.Player.Sound4.performed += Sound4Performed;
        inputMap.Player.Sound5.performed += Sound5Performed;
        inputMap.Player.Sound6.performed += Sound6Performed;
    }

    void Update()
    {
        moveInput.x = inputValues.x;
        moveInput.Normalize(); // Evita moverse más rápido en diagonal

        if (moveInput.magnitude > 0.1f)
        {
            isMoving = true;
            anim.SetBool("IsWalking", true);
            
        }

        else
        {
            isMoving = false;
            anim.SetBool("IsWalking", false);
        }
            

        // Si está tocando el suelo (Floor), desactivamos la gravedad
        if (!controller.isGrounded)
        {
            if (isPressJumping)
                verticalVelocity += (gravityScale - gravityScale/3) * Time.deltaTime;
            else
                verticalVelocity += gravityScale * Time.deltaTime;

        }

        //Move the player
        if (isMoving)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, moveInput * moveSpeed, Time.deltaTime * 10f);
            anim.SetBool("IsWalking", true);

           
        }
        else
        {
            //Player inertia
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * 5f);
        }

        if(isHit && verticalVelocity > 0)
        {
            verticalVelocity = 0;
            
        }

        if (verticalVelocity < -30)
            verticalVelocity = -30;

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
    }

    public void OnEnable()
    {
        souning = false;
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

            this.transform.position = startPosition;
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

        if ((controller.isGrounded || GameManager.instance.playerMovePlatform) && !jumpCooldown)
            Jump();
        

    }


    public void JumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isPressJumping = false;
        anim.SetBool("IsJumping", false);
    }

    public void OptionsPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!plataformaMovimiento)
        {
            if (menuPause.activeSelf)
            {
                menuPause.gameObject.SetActive(false);

                layout.gameObject.SetActive(true);
                sprite.gameObject.SetActive(true);
                sliderBall.gameObject.SetActive(true);
                this.transform.position = positionEscape;

                if (hotspot != null)
                    hotspot.ShowControl();
                if (finishLevel != null)
                    finishLevel.ShowControl();

                GameManager.instance.SaveMusic();
            }
            else
            {
                layout.gameObject.SetActive(false);
                sprite.gameObject.SetActive(false);
                sliderBall.gameObject.SetActive(false);
                positionEscape = this.transform.position;

                menuPause.gameObject.SetActive(true);
                if (hotspot != null)
                    hotspot.HideControl();
                if (finishLevel != null)
                    finishLevel.HideControl();

            }
        }
    }
    public void InterectCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

       
        
           anim.SetBool("IsHitting", false);
        


    }

    public void InterectPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       
        if (isOnFinishLevel && finishLevel != null && !finishLevel.doorOpen)
        {
            anim.SetBool("IsHitting", true);

            /* bool correct = CheckSequence();
             if (correct)
             {
                 StartCoroutine(finishLevel.RotateOverTime());
                 finishLevel.HideControl();
                 finishLevel.doorOpen = true;
             }
             finishLevel.SoundDoor(correct);*/


        }
        else if ( isOnHotSpot && hotspot != null)
        {
            anim.SetBool("IsHitting", true);
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
        if (!ballLauch)
        {
            LaunchBall();
            //imagesBall[indexBallImage].gameObject.SetActive(false);
           // indexBallImage++;
        }
    }

    public void TakeSoundPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isNearObjectSong && objectSong != null)
        {
            Color color = objectSong.TakeItem();

            SpawnImage(color);
            
            sequence.Add(objectSong.audioSource.clip);

            //objectSong.HideControl();
        }
    }

    public void Sound1Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 0;

        if(sequence.Count > sound && !souning)
        {
            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());

            if (plataformaMovimiento != null)
            {
                plataformaMovimiento.MovePlatform(1);
            }
        }
    }
    public void Sound2Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 1;

        if (sequence.Count > sound && !souning)
        {
            souning = true;
            audioSourceSequence.clip = sequence[sound];
            audioSourceSequence.Play();
            SoundToDoor(sequence[sound]);
            StartCoroutine(WaitForSoundToEnd());

            if (plataformaMovimiento != null)
            {
                plataformaMovimiento.MovePlatform(2);
            }
        }
    }
    public void Sound3Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        sound = 2;

        if (sequence.Count > sound && !souning)
        {
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
       /* if (bossLight != null)
            bossLight.TriggerLightFade();*/

        if (sequence.Count > sound && !souning)
        {
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
        text.text = (spawnedImages.Count + 1).ToString();
        spawnedImages.Add(newImage);

        // Ajustar la posición de todas las imágenes
        UpdateImagePositions();

        if(color.a == 150)
        {

        }
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
            rectTransform.localPosition = localPos;
        }
    }

    // Método para aplicar el salto
    void Jump()
    {
        audioSourceEffectPlayer.clip = aduioJump;
        audioSourceEffectPlayer.Play();
        anim.SetBool("IsJumping", true);
        jumpCooldown = true;
        isHit = false;
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
            ballLauch = true;

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

            StartCoroutine(HideBall(newBall));
            updateSliderBall = true;
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
        
        controller.enabled = false;
        transform.position = startPosition;
        controller.enabled = true;
        imageDamage.gameObject.SetActive(true);
        StartCoroutine(TrasparentDamage(3f));
    }

    private IEnumerator TrasparentDamage(float duration)
    {
        Color startColor = imageDamage.color;
        float startAlpha = startColor.a;

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
            finishLevel.ClearSequence();
        }else if (other.gameObject.CompareTag("PlatformMove") && plataformaMovimiento != null)
        {
            plataformaMovimiento.ResetEffect();
            plataformaMovimiento = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "HotSpot") {
            isOnHotSpot = true;
            hotspot = other.GetComponent<HotSpot>();
            hotspot.ShowControl();
            startPosition = transform.position;
            GameManager.instance.Save();
        }
        else if(other.tag == "FloorObjectSong")
        {
            isNearObjectSong = true;
            objectSong = other.GetComponent<ObjectSong>();
            //objectSong.ShowControl();
        }else if(other.tag == "EntryBoss" && !onBoss)
        {
            startPosition = this.transform.position;
            onBoss = true;
            bossLight = other.GetComponent<BossLight>();
            StartCoroutine(bossLight.Blink());

        }
        else if(other.gameObject.CompareTag("Ball"))
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

            
            updateSliderBall = false;
            timerSliderBall = 0f;
            sliderBall.value = 1f;

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
        }else if (other.gameObject.CompareTag("PlatformMove"))
        {
            if (other.gameObject.TryGetComponent<PlatformMove>(out PlatformMove mover))
            {
                plataformaMovimiento = mover;
                plataformaMovimiento.ActivateEffect();
            }
        }
        else if (other.gameObject.CompareTag("ObjetoCancion"))
        {

            if (isNearObjectSong && objectSong != null)
            {
                Color color = objectSong.TakeItem();

                SpawnImage(color);

                sequence.Add(objectSong.audioSource.clip);

                //objectSong.HideControl();

                if(other.gameObject.name == "BolaCancionGreen")
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
        }else if (other.gameObject.CompareTag("DefeatBoss"))
        {
            GameManager.instance.defeatBoss = true;
        }else if (other.gameObject.CompareTag("Animation2"))
        {
            canvasTransform.gameObject.SetActive(false);
            cineMachine.PlayTimelineLevel2();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Animation3"))
        {
            canvasTransform.gameObject.SetActive(false);
            cineMachine.PlayTimelineLevel3();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Animation4"))
        {
            canvasTransform.gameObject.SetActive(false);
            cineMachine.PlayTimelineLevel4();
            Destroy(other.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Vector3.Dot(hit.normal, Vector3.down) > 0.5f)
        {
            isHit = true;
        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            Dead();
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

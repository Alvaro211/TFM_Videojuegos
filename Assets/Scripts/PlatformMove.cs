using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlatformMove : MonoBehaviour
{
    public enum Direction { Up, Right, Down, Left }
    public Direction moveDirection = Direction.Up;
    public bool activatedPlatform1;

    public float moveDistance = 2f;     // Distancia del movimiento
    public float moveSpeed = 2f;        // Velocidad en unidades por segundo

    public GameObject platform;
    public SpriteRenderer sprite;
    public Material activatedMaterial;
    public Material inActivatedMaterial;
    public List<Light> lights;

    public TextMeshPro[] textoAyuda;
    public float timeToAdvise = 5;

    private Vector3 initialPosition;
    private Vector3 positionToReturn;
    private Vector3 targetPosition;
    private bool isMoved = false;
    private bool isMoving = false;

    private Renderer objRenderer;
    private Coroutine corutine;
    private AudioSource audio;

    private float timeInTrigger;
    private bool isMovedOneTime = false;
    private bool movePlayer = false;
    private bool advise = false;

    private void Start()
    {
        initialPosition = platform.transform.position;
        targetPosition = initialPosition;
        positionToReturn = initialPosition;

        objRenderer = platform.GetComponent<Renderer>();
        audio = GetComponent<AudioSource>();

        if (sprite != null)
        {
            sprite.color = new Color(0.33f, 0.33f, 0.33f);
        }
    }

    private void Update()
    {
        if (advise)
        {
            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                textoAyuda[0].gameObject.SetActive(true);
                textoAyuda[1].gameObject.SetActive(false);
                textoAyuda[2].gameObject.SetActive(false);
            }
            else if (GameManager.instance.idiom == GameManager.Language.Valencian)
            {
                textoAyuda[0].gameObject.SetActive(false);
                textoAyuda[1].gameObject.SetActive(true);
                textoAyuda[2].gameObject.SetActive(false);
            }
            else
            {
                textoAyuda[0].gameObject.SetActive(false);
                textoAyuda[1].gameObject.SetActive(false);
                textoAyuda[2].gameObject.SetActive(true);
            }
        }
    }

    public void MovePlatform(int index)
    {
        if (isMoving || (index == 1 && !activatedPlatform1) || (index == 2 && activatedPlatform1)) return; // Evita activar el movimiento si ya se está moviendo

        audio.Play();

        isMovedOneTime = true;

        isMoved = !isMoved;
        targetPosition = isMoved
            ? initialPosition + GetDirectionVector(moveDirection) * moveDistance
            : initialPosition;

        StartCoroutine(MoveToPosition(targetPosition));
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 destination)
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        CharacterController controller = player.GetComponent<CharacterController>();

        Vector3 lastPlatformPos = platform.transform.position;

        GameManager.instance.playerMovePlatform = true;
        isMoving = true;
        while (Vector3.Distance(platform.transform.position, destination) > 0.01f)
        {
            Vector3 currentPlatformPos = platform.transform.position;
            Vector3 nextPlatformPos = Vector3.MoveTowards(currentPlatformPos, destination, moveSpeed * Time.deltaTime);
            Vector3 platformDelta = nextPlatformPos - currentPlatformPos;

            
            platform.transform.position = nextPlatformPos;

            // Si el jugador está encima de la plataforma, moverlo también
            // (aquí asumimos que siempre lo está; si necesitás precisión, podés usar un raycast o trigger)
            // Mover plataforma
            if (movePlayer)
                controller.Move(platformDelta);

            lastPlatformPos = nextPlatformPos;
            yield return null;
        }

        platform.transform.position = destination; // Ajuste final
        isMoving = false;
        GameManager.instance.playerMovePlatform = false;
    }

    private Vector3 GetDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Vector3.up;
            case Direction.Right: return Vector3.right;
            case Direction.Down: return Vector3.down;
            case Direction.Left: return Vector3.left;
            default: return Vector3.zero;
        }
    }

    public void ActivateEffect()
    {
        if (corutine != null)
        {
            StopCoroutine(corutine);
            corutine = null;
        }

        // Cambiar el material si hay uno asignado
        if (activatedMaterial != null && objRenderer != null)
        {
            objRenderer.material = activatedMaterial;
        }

        if(sprite != null)
        {
            sprite.color = new Color(1f, 1f, 1f);
        }

        foreach(Light light in lights)
        {
            light.gameObject.SetActive(true);
        }
    }

    public void ResetEffect()
    {
        isMoved = false;
        corutine = StartCoroutine(ResetPlatform());
        if (inActivatedMaterial != null && objRenderer != null)
        {
            objRenderer.material = inActivatedMaterial;
        }

        if (sprite != null)
        {
            sprite.color = new Color(0.33f, 0.33f, 0.33f);
        }
    }

    public IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(5);
        // Cambiar al material estándar de Unity
        if (objRenderer != null)
        {
            objRenderer.material = inActivatedMaterial;
        }

        if (sprite != null)
        {
            sprite.color = new Color(0.33f, 0.33f, 0.33f);
        }

        foreach (Light light in lights)
        {
            light.gameObject.SetActive(false);
        }

        StartCoroutine(MoveToPosition(positionToReturn));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movePlayer = true;

            timeInTrigger += Time.deltaTime;
            if (timeInTrigger >= timeToAdvise && !isMovedOneTime)
            {
                advise = true;

                if (GameManager.instance.idiom == GameManager.Language.Spanish)
                    textoAyuda[0].gameObject.SetActive(true);
                else if (GameManager.instance.idiom == GameManager.Language.Valencian)
                    textoAyuda[1].gameObject.SetActive(true);
                else
                    textoAyuda[2].gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movePlayer = false;
            advise = false;
            textoAyuda[0].gameObject.SetActive(false);
            textoAyuda[1].gameObject.SetActive(false);
            textoAyuda[2].gameObject.SetActive(false);
            timeInTrigger = 0f;
        }
    }
}

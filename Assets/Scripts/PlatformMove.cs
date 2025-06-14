using System.Collections;
using System.Collections.Generic;
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
    public Material activatedMaterial;
    public Material inActivatedMaterial;
    public List<Light> lights;

    private Vector3 initialPosition;
    private Vector3 positionToReturn;
    private Vector3 targetPosition;
    private bool isMoved = true;
    private bool isMoving = false;

    private Renderer objRenderer;
    private Coroutine corutine;

    private void Start()
    {
        initialPosition = platform.transform.position;
        targetPosition = initialPosition;
        positionToReturn = initialPosition;

        objRenderer = platform.GetComponent<Renderer>();
    }



    public void MovePlatform(int index)
    {
        if (isMoving || (index == 1 && !activatedPlatform1) || (index == 2 && activatedPlatform1)) return; // Evita activar el movimiento si ya se está moviendo

        isMoved = !isMoved;
        targetPosition = isMoved
            ? initialPosition + GetDirectionVector(moveDirection) * moveDistance
            : initialPosition;

        StartCoroutine(MoveToPosition(targetPosition, true));
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 destination, bool movePlayer)
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

        foreach(Light light in lights)
        {
            light.gameObject.SetActive(true);
        }
    }

    public void ResetEffect()
    {
        isMoved = !isMoved;
        corutine = StartCoroutine(ResetPlatform());
        if (inActivatedMaterial != null && objRenderer != null)
        {
            objRenderer.material = inActivatedMaterial;
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

        foreach (Light light in lights)
        {
            light.gameObject.SetActive(false);
        }

        StartCoroutine(MoveToPosition(positionToReturn, false));
    }
}

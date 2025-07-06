using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    public float patrolDistance = 8f; // Distancia que avanzar?en Z
    public float waitTime = 2f;         // Tiempo de espera en cada punto
    public float searchRadius = 10;
    public bool horizontal = false;
    public bool isStunned = false;

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingForward = true;
    private bool chasingBall = false;
    private bool chasingPlayer = false;

    private SpriteRenderer sprite;

    public Animator anim;

    public SpriteRenderer imagenEstado;
    public Sprite interrogacion;
    public Sprite exclamacion;

    public AudioSource audio;
    public AudioClip audioIdle;
    public AudioClip audioChasing;

    private Transform player;

    private Coroutine currentRoutine;
    private string currentRoutineName = "";

    private float tiempo = 0f; 
    private bool wasPausedByTimeScale = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position; // Guarda la posición inicial
        if (!horizontal)
            targetPosition = startPosition + new Vector3(0, 0, patrolDistance);
        else
            targetPosition = startPosition + new Vector3(patrolDistance, 0, 0);

        // Moverse al primer destino
        agent.SetDestination(targetPosition);

        GameObject playerObj = GameObject.Find("Jugador");
        if (playerObj != null)
            player = playerObj.transform;

        sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();

        anim = this.transform.GetChild(0).transform.GetComponent<Animator>();
    }

    void Update()
    {

        bool isChasing = chasingPlayer || chasingBall;

        if (Time.timeScale == 0f)
        {
            if (audio.isPlaying)
            {
                audio.Pause();
                wasPausedByTimeScale = true;
            }
            return; // No ejecutar más lógica si el juego está pausado
        }

        if (Time.timeScale == 1f && wasPausedByTimeScale)
        {
            audio.UnPause();
            wasPausedByTimeScale = false;
        }

        // Tu lógica de reproducción de audio
        if (isChasing && (audio.clip != audioChasing || !audio.isPlaying))
        {
            audio.clip = audioChasing;
            audio.loop = false; // O true, según tu caso
            audio.Play();
        }
        else if (!isChasing && !audio.isPlaying)
        {
            audio.clip = audioIdle;
            audio.loop = true;
            audio.Play();
        }

        if (chasingBall)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
                imagenEstado.sprite = interrogacion;
            else
                imagenEstado.sprite = exclamacion;
            imagenEstado.gameObject.SetActive(true);
        }
        else if (chasingPlayer)
        {
            imagenEstado.sprite = exclamacion;
            imagenEstado.gameObject.SetActive(true);
        }
        else
        {
            imagenEstado.gameObject.SetActive(false);
        }

        if (sprite != null)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            imagenEstado.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (!chasingBall && !chasingPlayer)
        {
            anim.SetBool("IsIdle", agent.remainingDistance <= agent.stoppingDistance);
        }

        if (chasingBall && (agent.destination.x - transform.position.x) < 0.3f)
        {
            isStunned = true;
        }
        else
            isStunned = false;


        if (player != null && !chasingBall)
        {
            if (tiempo < 0.5f)
                tiempo += Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= searchRadius)
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(player.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    GameManager.instance.vibration.VibrarMando((0.5f + tiempo), 0.5f);

                    chasingPlayer = true;
                    anim.SetBool("IsChasing", true);
                    anim.SetBool("IsIdle", false);
                    agent.SetDestination(player.position);
                    agent.speed = 7;
                    if (currentRoutine != null)
                        StopCoroutine(currentRoutine);
                    currentRoutine = StartCoroutine(WaitPlayerAndReturn());
                    currentRoutineName = "WaitPlayerAndReturn";
                    return;
                }
            }
        }
        else
        {
            tiempo = 0f;
        }

        if (!chasingBall && !chasingPlayer)
        {
            anim.SetBool("IsIdle", agent.remainingDistance <= agent.stoppingDistance);
        }


        if (!chasingBall && !chasingPlayer) // Solo patrullar si no est?yendo a la bola
        {
            if (/*!waiting && !agent.pathPending && */agent.remainingDistance <= agent.stoppingDistance /*&& agent.velocity.magnitude == 0*/)
            {
                anim.SetBool("IsIdle", true);
                if (currentRoutine != null && currentRoutineName != "ChangeDirection")
                {
                    StopCoroutine(currentRoutine);
                }

                if (currentRoutineName != "ChangeDirection")
                {
                    currentRoutine = StartCoroutine(ChangeDirection());
                    currentRoutineName = "ChangeDirection";
                }
            }
        }

        if (!chasingBall && !chasingPlayer)
        {
            anim.SetBool("IsIdle", agent.remainingDistance <= agent.stoppingDistance);
        }


        
    }


    private IEnumerator ChangeDirection()
    {
        if (chasingBall || chasingPlayer) yield break;

        yield return new WaitForSeconds(waitTime); // Espera antes de cambiar dirección


        // Cambiar destino
        if (!chasingBall && !chasingPlayer)
        {
            if (patrolDistance != 0)
                anim.SetBool("IsIdle", false);
            agent.SetDestination(movingForward ? startPosition : targetPosition);
            movingForward = !movingForward;

            ComprobarDireccionSprite();
        }

        currentRoutineName = "";
    }

    public void MoveToBall(Vector3 ballPosition)
    {

        if (Vector3.Distance(transform.position, targetPosition) < searchRadius && !chasingPlayer)
        {
            //// Verifica si la posición es alcanzable
            NavMeshHit hit;
            if (NavMesh.SamplePosition(ballPosition, out hit, 10f, NavMesh.AllAreas))
            {
                chasingBall = true; // Se dirige a la bola

                anim.SetBool("IsChasing", true);
                anim.SetBool("IsIdle", false);
                agent.SetDestination(hit.position);


                ComprobarDireccionSprite();

                if (currentRoutine != null)
                    StopCoroutine(currentRoutine);
                currentRoutine = StartCoroutine(WaitBallAndReturn());
                currentRoutineName = "WaitBallAndReturn";
            }
        }
    }

    private IEnumerator WaitBallAndReturn()
    {
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        anim.SetBool("IsConfuse", true);
        yield return new WaitForSeconds(5f); // Espera 3 segundos en la bola
        chasingBall = false;
        chasingPlayer = false;

        anim.SetBool("IsChasing", false);
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsConfuse", false);


        agent.ResetPath();
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ContinuePatrol(false));
        currentRoutineName = "ContinuePatrol";
    }

    private IEnumerator WaitPlayerAndReturn()
    {
        ComprobarDireccionSprite();

        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
        chasingBall = false;
        chasingPlayer = false;
       
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsChasing", false);
        agent.speed = 3f;
        agent.ResetPath();
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ContinuePatrol(true));
        currentRoutineName = "ContinuePatrol";
    }

    private IEnumerator ContinuePatrol(bool hasSeenPlayer)
    {
        // Después de esperar, vuelve a su ruta original
        anim.SetBool("IsIdle", false);
        yield return new WaitForSeconds(hasSeenPlayer ? 0.5f : waitTime); // Espera antes de comenzar el patrullaje
        agent.SetDestination(movingForward ? startPosition : targetPosition);
        ComprobarDireccionSprite();
        movingForward = !movingForward;
    }

    private void ComprobarDireccionSprite()
    {
        float directionToPlayer = agent.destination.x - transform.position.x;

        if ((directionToPlayer > 0 && sprite.flipX) || (directionToPlayer < 0 && !sprite.flipX))
        {
            sprite.flipX = !sprite.flipX;
        }
    }


}

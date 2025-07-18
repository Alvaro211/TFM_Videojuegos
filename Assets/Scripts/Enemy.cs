using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
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

    public Light light;
    private Vector3 offsetLight = new Vector3(0, 0, -8f);

    private Transform player;

    private Coroutine currentRoutine;
    private string currentRoutineName = "";

    private float tiempo = 0f; 
    private bool wasPausedByTimeScale = false;


    public float minInnerAngle = 30f;
    public float minOuterAngle = 40f;
    public float reductionSpeed = 15f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position; // Guarda la posici�n inicial
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
        if (light != null)
        {
            light.transform.position = this.transform.position + offsetLight;

            float newInnerAngle = Mathf.MoveTowards(light.innerSpotAngle, minInnerAngle, reductionSpeed * Time.deltaTime);
            float newOuterAngle = Mathf.MoveTowards(light.spotAngle, minOuterAngle, reductionSpeed * Time.deltaTime);

            light.innerSpotAngle = newInnerAngle;
            light.spotAngle = newOuterAngle;
        }

        bool isChasing = chasingPlayer || chasingBall;

        if (Time.timeScale == 0f && GameManager.instance.canMove)
        {
            if (audio.isPlaying)
            {
                audio.Pause();
                wasPausedByTimeScale = true;
            }
            return; // No ejecutar m�s l�gica si el juego est� pausado
        }

        if (Time.timeScale == 1f && wasPausedByTimeScale)
        {
            audio.UnPause();
            wasPausedByTimeScale = false;
        }

        // Tu l�gica de reproducci�n de audio
        if (isChasing && (audio.clip != audioChasing || !audio.isPlaying) && GameManager.instance.canMove)
        {
            audio.clip = audioChasing;
            audio.loop = false; // O true, seg�n tu caso
            audio.Play();


            light.spotAngle = 70;
            light.innerSpotAngle = 45;
        }
        else if (!isChasing && !audio.isPlaying && GameManager.instance.canMove && GameManager.instance.musicEnemy)
        {
            if(audio.clip != audioIdle)
                audio.clip = audioIdle;
            //audio.loop = true;
            audio.Play();

            light.spotAngle = 50;
            light.innerSpotAngle = 35;
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

        yield return new WaitForSeconds(waitTime); // Espera antes de cambiar direcci�n


        // Cambiar destino
        if (!chasingBall && !chasingPlayer)
        {
            if (patrolDistance != 0)
                anim.SetBool("IsIdle", false);

            Vector3 destination = movingForward ? startPosition : targetPosition;

            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(destination);
            }

            movingForward = !movingForward;

            ComprobarDireccionSprite();
        }

        currentRoutineName = "";
    }

    public void MoveToBall(Vector3 ballPosition)
    {
        if (Vector3.Distance(transform.position, targetPosition) < searchRadius && !chasingPlayer)
        {
            NavMeshPath path = new NavMeshPath();

            if (agent.CalculatePath(ballPosition, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                chasingBall = true;
                anim.SetBool("IsChasing", true);
                anim.SetBool("IsIdle", false);

                agent.SetDestination(ballPosition);
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
        // Despu�s de esperar, vuelve a su ruta original
        anim.SetBool("IsIdle", false);
        yield return new WaitForSeconds(hasSeenPlayer ? 0.5f : waitTime); // Espera antes de comenzar el patrullaje
        
        Vector3 destination = movingForward ? startPosition : targetPosition;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(destination);
        }
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

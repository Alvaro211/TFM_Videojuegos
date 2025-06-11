using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float patrolDistance = 8f; // Distancia que avanzará en Z
    public float waitTime = 2f;         // Tiempo de espera en cada punto
    public float searchRadius = 10;
    public bool horizontal = false;
    public bool isStunned = false;

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingForward = true; 
    private bool waiting = false;
    private bool chasingBall = false;
    private bool chasingPlayer = false;

    private SpriteRenderer sprite;

    public  Animator anim;

    private Transform player;

    private Coroutine currentRoutine;
    private string currentRoutineName = "";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position; // Guarda la posición inicial
        if(!horizontal) 
            targetPosition = startPosition + new Vector3(0, 0, patrolDistance);
        else
            targetPosition = startPosition + new Vector3(patrolDistance, 0, 0);

        // Moverse al primer destino
        agent.SetDestination(targetPosition);

        GameObject playerObj = GameObject.Find("Jugador");
        if (playerObj != null)
            player = playerObj.transform;

        sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (sprite != null)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
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
            

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= searchRadius)
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(player.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    chasingPlayer = true;
                    anim.SetBool("IsChasing",true);
                    agent.SetDestination(player.position);
                    agent.speed = 10;
                    if (currentRoutine != null)
                        StopCoroutine(currentRoutine);
                    currentRoutine = StartCoroutine(WaitPlayerAndReturn());
                    currentRoutineName = "WaitPlayerAndReturn";
                    return;
                }
            }
        }

        if (!chasingBall && !chasingPlayer)
        {
            anim.SetBool("IsIdle", agent.remainingDistance <= agent.stoppingDistance);
        }


        if (!chasingBall && !chasingPlayer) // Solo patrullar si no está yendo a la bola
        {
            if (/*!waiting && !agent.pathPending && */agent.remainingDistance <= agent.stoppingDistance /*&& agent.velocity.magnitude == 0*/)
            {
                anim.SetBool("IsIdle", true);
                if (currentRoutine != null && currentRoutineName != "ChangeDirection") { 
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

        waiting = true; // Evita múltiples llamadas


        

        yield return new WaitForSeconds(waitTime); // Espera antes de cambiar dirección
       

        // Cambiar destino
        if (!chasingBall && !chasingPlayer)
        {
            anim.SetBool("IsIdle", false);
            agent.SetDestination(movingForward ? startPosition : targetPosition);
            movingForward = !movingForward;

            ComprobarDireccionSprite();
        }

        waiting = false;

        currentRoutineName = "";
    }

    public void MoveToBall(Vector3 ballPosition)
    {

        if (Vector3.Distance(transform.position, targetPosition) < searchRadius && !chasingPlayer)
        {
            //// Verifica si la posición es alcanzable
            NavMeshHit hit;
            if (NavMesh.SamplePosition(ballPosition, out hit, 1.0f, NavMesh.AllAreas))
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
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
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
        agent.speed = 5f;
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

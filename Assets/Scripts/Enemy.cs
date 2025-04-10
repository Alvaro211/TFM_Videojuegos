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

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingForward = true; 
    private bool waiting = false;
    private bool chasingBall = false;

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
    }

    void Update()
    {
        if (!chasingBall) // Solo patrullar si no está yendo a la bola
        {
            if (!waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.magnitude == 0)
            {
                StartCoroutine(ChangeDirection());
            }
        }
    }

    private IEnumerator ChangeDirection()
    {
        if (chasingBall) yield break;

        waiting = true; // Evita múltiples llamadas
        yield return new WaitForSeconds(waitTime); // Espera antes de cambiar dirección

        // Cambiar destino
        if (!chasingBall)
        {
            agent.SetDestination(movingForward ? startPosition : targetPosition);
            movingForward = !movingForward;
        }

        waiting = false;
    }

    public void MoveToBall(Vector3 ballPosition)
    {
        Debug.Log(transform.position.x - targetPosition.x);

        if (Vector3.Distance(transform.position, targetPosition) < searchRadius)
        {
            //// Verifica si la posición es alcanzable
            NavMeshHit hit;
            if (NavMesh.SamplePosition(ballPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                chasingBall = true; // Se dirige a la bola
                agent.SetDestination(hit.position);
                StartCoroutine(WaitAndReturn());
            }
        }
    }

    private IEnumerator WaitAndReturn()
    {
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
        yield return new WaitForSeconds(5f); // Espera 3 segundos en la bola
        chasingBall = false;
        agent.ResetPath();  
        StartCoroutine(ContinuePatrol());
    }

    private IEnumerator ContinuePatrol()
    {
        // Después de esperar, vuelve a su ruta original
        yield return new WaitForSeconds(waitTime); // Espera antes de comenzar el patrullaje
        agent.SetDestination(movingForward ? startPosition : targetPosition);
        movingForward = !movingForward;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float patrolDistanceZ = 8f; // Distancia que avanzará en Z
    public float waitTime = 2f;         // Tiempo de espera en cada punto
    public float searchRadius = 10;

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
        targetPosition = startPosition + new Vector3(0, 0, patrolDistanceZ);

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
        waiting = true; // Evita múltiples llamadas
        yield return new WaitForSeconds(waitTime); // Espera antes de cambiar dirección

        // Cambiar destino
        agent.SetDestination(movingForward ? startPosition : targetPosition);
        movingForward = !movingForward; // Alterna entre ida y vuelta
        waiting = false;
    }

    public void MoveToBall(Vector3 ballPosition)
    {
        if (Vector3.Distance(transform.position, targetPosition) < searchRadius &&
            Mathf.Abs(ballPosition.y - transform.position.y) <= 0.5f)
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

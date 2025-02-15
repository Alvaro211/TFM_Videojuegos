using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float patrolDistanceZ = 8f; // Distancia que avanzar� en Z
    public float waitTime = 2f;         // Tiempo de espera en cada punto

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingForward = true; 
    private bool waiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position; // Guarda la posici�n inicial
        targetPosition = startPosition + new Vector3(0, 0, patrolDistanceZ);

        // Moverse al primer destino
        agent.SetDestination(targetPosition);
    }

    void Update()
    {
        if (!waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.magnitude == 0)
        {
            StartCoroutine(ChangeDirection());
        }
    }

    private IEnumerator ChangeDirection()
    {
        waiting = true; // Evita m�ltiples llamadas
        yield return new WaitForSeconds(waitTime); // Espera antes de cambiar direcci�n

        // Cambiar destino
        agent.SetDestination(movingForward ? startPosition : targetPosition);
        movingForward = !movingForward; // Alterna entre ida y vuelta
        waiting = false;
    }
}

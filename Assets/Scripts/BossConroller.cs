using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossConroller : MonoBehaviour
{
    public float velocityBoss = 2f;
    public float distance = 3f;
    public GameObject prefabBola;
    public GameObject player;
    public float offsetY = 1f;
    public float velocityMunition = 20f;
    public float timeWaitShoot = 5f;

    private Vector3 positionInitial;
    private int direcction = 1;

    void Start()
    {
        positionInitial = transform.position;
        Invoke("Shoot", timeWaitShoot);
    }

    void Update()
    {
        if (GameManager.instance.defeatBoss) {
            ReturnToStartPosition();
            CancelInvoke("Shoot");
        }
        else
        {
            Movement();
        }
        
    }

    void Shoot()
    {
        if (prefabBola != null && player != null)
        {
            Vector3 positionMunition = transform.position - new Vector3(0, offsetY, 0);
            GameObject munition = Instantiate(prefabBola, positionMunition, Quaternion.identity);

            Vector3 direccion = (player.transform.position - positionMunition).normalized;

            Rigidbody rb = munition.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direccion * velocityMunition;
            }

            Collider col1 = GetComponent<Collider>();
            Collider col2 = munition.GetComponent<Collider>();
            if (col1 != null && col2 != null)
            {
                Physics.IgnoreCollision(col1, col2);
            }
        }

        Invoke("Shoot", timeWaitShoot);
    }

    void Movement()
    {
        transform.Translate(Vector3.right * direcction * velocityBoss * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - positionInitial.x) >= distance)
            direcction *= -1;
    }

    void ReturnToStartPosition()
    {
        float returnSpeed = velocityBoss * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, positionInitial + new Vector3(-11, 0, 0), returnSpeed);
    }
}

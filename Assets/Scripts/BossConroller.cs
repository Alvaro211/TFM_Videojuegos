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

    public bool onAnimation = true;

    public AudioSource audio;
    public AudioClip audioBossIdle;
    public AudioClip audioBossShout;

    public Credits credits;

    [SerializeField] GameObject Deathprefabricated;

    private Vector3 positionInitial;
    private int direcction = 1;
    private Animator animator;
    private bool firstTime = true;

    void Start()
    {
        positionInitial = transform.position;

        animator = this.transform.GetChild(0).GetComponent<Animator>();
        
        onAnimation = true;
    }

    void Update()
    {
        if (GameManager.instance.defeatBoss) {
            ReturnToStartPosition();
            CancelInvoke("Shoot");
            animator.SetBool("Start", false);
        }
        else
        {
            if (!onAnimation) {
                Movement();

                if (firstTime)
                {
                    Invoke("Shoot", timeWaitShoot);
                    firstTime = false;
                }

                if (!audio.isPlaying)
                {
                    audio.clip = audioBossIdle;
                    audio.loop = true;
                    audio.Play();
                }
            }
            else
            {
                this.transform.position = positionInitial;
            }
        }


    }

    public void StartAnimation()
    {
        animator.SetBool("Start", true);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
           collision.gameObject.SetActive(false);
           GameManager.instance.canMove = false;
           credits.Invoke("ShowFloatingMessage", 1f);

            var myObj = GameObject.Instantiate(Deathprefabricated);
            myObj.transform.position = transform.position;
            gameObject.SetActive(false);

        }
    }

    public void PlayBossShout()
    {
        if (audio == null || audioBossShout == null) return;

        audio.Stop(); // Detener el audio actual
        audio.clip = audioBossShout;
        audio.loop = false;
        audio.Play();
    }
}

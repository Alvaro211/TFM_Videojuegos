using UnityEngine;
using System.Collections;

public class CubeFalling : MonoBehaviour
{
    public float fastCubeSpeed = 5f;
    public float fallDistance = 2f;
    public float riseSpeed = 2f;
    public float waitTime = 1f; // Tiempo de espera arriba

    private Rigidbody fastCubeRb;
    private Vector3 fastCubeStartPos;
    private bool fastCubeFalling = true;
    private bool isWaiting = false;
    private bool sound = false;
    private AudioSource audio;

    void Start()
    {
        if (gameObject != null)
        {
            fastCubeRb = gameObject.AddComponent<Rigidbody>();
            fastCubeRb.useGravity = false;
            fastCubeRb.freezeRotation = true;
            fastCubeStartPos = gameObject.transform.position;
            audio = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (fastCubeRb != null && !isWaiting)
        {
            if (fastCubeFalling)
            {
                if (sound)
                {
                    sound = false;
                    audio.Play();
                }


                fastCubeRb.velocity = new Vector3(0, -fastCubeSpeed, 0);
                if (fastCubeStartPos.y - fastCubeRb.transform.position.y >= fallDistance)
                {
                    fastCubeFalling = false;
                }
            }
            else
            {
                fastCubeRb.velocity = new Vector3(0, riseSpeed, 0);
                if (fastCubeRb.transform.position.y >= fastCubeStartPos.y)
                {
                    fastCubeRb.transform.position = fastCubeStartPos;
                    StartCoroutine(WaitBeforeFalling()); // Espera antes de volver a caer
                }
            }
        }
    }

    private IEnumerator WaitBeforeFalling()
    {
        isWaiting = true;
        fastCubeRb.velocity = Vector3.zero; // Detener el cubo
        yield return new WaitForSeconds(waitTime); // Espera 1 segundo
        fastCubeFalling = true;
        isWaiting = false;
        sound = true;
    }
}

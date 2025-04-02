using UnityEngine;

public class CollectiblePlatform : MonoBehaviour
{
    public GameObject platform;
    public float riseSpeed = 2f;
    public float targetHeight = 5f;

    private int collectCount = 0;
    private bool isRising = false;
    private Vector3 startPosition;

    void Start()
    {
        if (platform != null)
        {
            startPosition = platform.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            collectCount++;
            Destroy(other.gameObject);

            if (collectCount >= 3)
            {
                isRising = true;
            }
        }
    }

    void Update()
    {
        if (isRising && platform != null)
        {
            if (platform.transform.position.y < startPosition.y + targetHeight)
            {
                platform.transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            }
        }
    }
}
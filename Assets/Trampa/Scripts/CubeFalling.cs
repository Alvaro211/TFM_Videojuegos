using UnityEngine;

public class CubeFalling : MonoBehaviour
{
    public float fastCubeSpeed = 5f;
    public float slowCubeSpeed = 2f;
    public float fallDistance = 2f; // ºı…Ÿœ¬¬‰æ‡¿Î
    public float riseSpeed = 2f;

    private Rigidbody fastCubeRb;
    private Rigidbody slowCubeRb;
    private Vector3 fastCubeStartPos;
    private Vector3 slowCubeStartPos;
    private bool fastCubeFalling = true;
    private bool slowCubeFalling = true;

    void Start()
    {
        GameObject fastCube = GameObject.Find("FastCube");
        GameObject slowCube = GameObject.Find("SlowCube");

        if (fastCube != null)
        {
            fastCubeRb = fastCube.AddComponent<Rigidbody>();
            fastCubeRb.useGravity = false;
            fastCubeStartPos = fastCube.transform.position;
        }

        if (slowCube != null)
        {
            slowCubeRb = slowCube.AddComponent<Rigidbody>();
            slowCubeRb.useGravity = false;
            slowCubeStartPos = slowCube.transform.position;
        }
    }

    void Update()
    {
        if (fastCubeRb != null)
        {
            if (fastCubeFalling)
            {
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
                    fastCubeFalling = true;
                }
            }
        }

        if (slowCubeRb != null)
        {
            if (slowCubeFalling)
            {
                slowCubeRb.velocity = new Vector3(0, -slowCubeSpeed, 0);
                if (slowCubeStartPos.y - slowCubeRb.transform.position.y >= fallDistance)
                {
                    slowCubeFalling = false;
                }
            }
            else
            {
                slowCubeRb.velocity = new Vector3(0, riseSpeed, 0);
                if (slowCubeRb.transform.position.y >= slowCubeStartPos.y)
                {
                    slowCubeRb.transform.position = slowCubeStartPos;
                    slowCubeFalling = true;
                }
            }
        }
    }
}
using UnityEngine;

public class AutoSpikeTrap : MonoBehaviour
{
    public GameObject spikes; // 地刺对象
    public float riseSpeed = 2f; // 地刺升起速度
    public float fallSpeed = 2f; // 地刺下降速度
    public float stayTime = 1f; // 地刺停留时间
    public float triggerInterval = 2f; // 触发间隔时间

    private bool isRising = false;
    private bool isFalling = false;
    private float timer = 0f;
    private float triggerTimer = 0f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        // 记录地刺的初始位置
        initialPosition = spikes.transform.position;
        // 计算地刺升起后的目标位置
        targetPosition = initialPosition + new Vector3(0, 2f, 0);
    }

    void Update()
    {
        triggerTimer += Time.deltaTime;

        // 检查是否到了触发时间
        if (triggerTimer >= triggerInterval)
        {
            isRising = true;
            triggerTimer = 0f;
        }

        if (isRising)
        {
            // 地刺升起
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, targetPosition, riseSpeed * Time.deltaTime);
            if (spikes.transform.position == targetPosition)
            {
                isRising = false;
                timer = 0f;
            }
        }
        else if (!isFalling)
        {
            // 地刺停留
            timer += Time.deltaTime;
            if (timer >= stayTime)
            {
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            // 地刺下降
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, initialPosition, fallSpeed * Time.deltaTime);
            if (spikes.transform.position == initialPosition)
            {
                isFalling = false;
            }
        }
    }
}
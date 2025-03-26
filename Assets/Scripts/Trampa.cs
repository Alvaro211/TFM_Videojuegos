using UnityEngine;

public class AutoSpikeTrap : MonoBehaviour
{
    public GameObject spikes; // �ش̶���
    public float riseSpeed = 2f; // �ش������ٶ�
    public float fallSpeed = 2f; // �ش��½��ٶ�
    public float stayTime = 1f; // �ش�ͣ��ʱ��
    public float triggerInterval = 2f; // �������ʱ��

    private bool isRising = false;
    private bool isFalling = false;
    private float timer = 0f;
    private float triggerTimer = 0f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        // ��¼�ش̵ĳ�ʼλ��
        initialPosition = spikes.transform.position;
        // ����ش�������Ŀ��λ��
        targetPosition = initialPosition + new Vector3(0, 2f, 0);
    }

    void Update()
    {
        triggerTimer += Time.deltaTime;

        // ����Ƿ��˴���ʱ��
        if (triggerTimer >= triggerInterval)
        {
            isRising = true;
            triggerTimer = 0f;
        }

        if (isRising)
        {
            // �ش�����
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, targetPosition, riseSpeed * Time.deltaTime);
            if (spikes.transform.position == targetPosition)
            {
                isRising = false;
                timer = 0f;
            }
        }
        else if (!isFalling)
        {
            // �ش�ͣ��
            timer += Time.deltaTime;
            if (timer >= stayTime)
            {
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            // �ش��½�
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, initialPosition, fallSpeed * Time.deltaTime);
            if (spikes.transform.position == initialPosition)
            {
                isFalling = false;
            }
        }
    }
}
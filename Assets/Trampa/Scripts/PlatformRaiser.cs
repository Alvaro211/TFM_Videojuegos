using UnityEngine;

// This script is used to control the platform's rise logic
public class PlatformRaiser : MonoBehaviour
{
 
    private bool[] keysPressed = new bool[4];
   
    private int keysPressedCount = 0;

    // Speed of platform ascent
    public float raiseSpeed = 2f;
    // Target height of platform ascent
    public float raiseHeight = 5f;
    // Target Y-coordinate of platform ascent
    private float targetY;

 
    private void Start()
    {
    
        targetY = transform.position.y + raiseHeight;
    }

    
    private void Update()
    {
        // Checks to see if the player has collected at least 4 balls.
        if (CollectibleBall.collectedCount >= 4)
        {
           
            CheckKeyPresses();
           
            if (keysPressedCount == 4)
            {
                // Call the platform up method
                RaisePlatform();
            }
        }
    }

    // ������ּ� 1��2��3��4 �İ������
    private void CheckKeyPresses()
    {
        // ������ּ� 1 �Ƿ񱻰��£���֮ǰδ�����¹�
        if (Input.GetKeyDown(KeyCode.Alpha1) && !keysPressed[0])
        {
            // ������ּ� 1 �ѱ�����
            keysPressed[0] = true;
            // �����Ѱ��µ����ּ�����
            keysPressedCount++;
        }
        // ������ּ� 2 �Ƿ񱻰��£���֮ǰδ�����¹�
        if (Input.GetKeyDown(KeyCode.Alpha2) && !keysPressed[1])
        {
            // ������ּ� 2 �ѱ�����
            keysPressed[1] = true;
            // �����Ѱ��µ����ּ�����
            keysPressedCount++;
        }
        // ������ּ� 3 �Ƿ񱻰��£���֮ǰδ�����¹�
        if (Input.GetKeyDown(KeyCode.Alpha3) && !keysPressed[2])
        {
            // ������ּ� 3 �ѱ�����
            keysPressed[2] = true;
            // �����Ѱ��µ����ּ�����
            keysPressedCount++;
        }
        // ������ּ� 4 �Ƿ񱻰��£���֮ǰδ�����¹�
        if (Input.GetKeyDown(KeyCode.Alpha4) && !keysPressed[3])
        {
            // ������ּ� 4 �ѱ�����
            keysPressed[3] = true;
            // �����Ѱ��µ����ּ�����
            keysPressedCount++;
        }
    }

    // ƽ̨�����ķ���
    private void RaisePlatform()
    {
        // ���ƽ̨��ǰ�� Y �����Ƿ�С��Ŀ�� Y ����
        if (transform.position.y < targetY)
        {
            // ��С��Ŀ�� Y ���꣬�������ƶ�ƽ̨
            transform.Translate(Vector3.up * raiseSpeed * Time.deltaTime);
        }
    }
}